using FlexibleAutomationTool.Core.Actions;
using FlexibleAutomationTool.Core.Actions.InternalActions;
using FlexibleAutomationTool.Core.Facades;
using FlexibleAutomationTool.Core.Interpreter;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlexibleAutomationTool.Core.Services
{
    public class AutomationEngine : IAutomationFacade
    {
        private readonly IRuleRepository _repo;
        private readonly Scheduler _scheduler;
        private readonly ServiceManager _serviceManager;
        private readonly Logger _logger;
        private readonly IInterpreter _interpreter;
        private readonly ICommandDispatcher _dispatcher;
        private readonly Factories.IPlatformFactory _factory;
        private readonly IExecutionHistoryRepository? _historyRepo;

        public event Action<Rule>? RuleTriggered;
        public event Action<Rule>? RuleExecuted;
        public event Action<Rule, Exception>? RuleFailed;

        public AutomationEngine(IRuleRepository repo, Scheduler scheduler, ServiceManager svcManager, Logger logger, IInterpreter interpreter, ICommandDispatcher dispatcher, FlexibleAutomationTool.Core.Factories.IPlatformFactory factory, IExecutionHistoryRepository? historyRepo = null)
        {
            _repo = repo;
            _scheduler = scheduler;
            _serviceManager = svcManager;
            _logger = logger;
            _interpreter = interpreter;
            _dispatcher = dispatcher;
            _factory = factory;
            _historyRepo = historyRepo;

            _scheduler.RuleReady += OnRuleReady;
        }

        public void CreateRule(Rule rule)
        {
            _repo.Add(rule);
            _logger.Log(rule.Name, "Created");
            _historyRepo?.Add(rule.Id, DateTime.UtcNow, "Created", "Rule created via UI");
        }

        public void CreateMacroRule(string name, string macroText)
        {
            var actions = _interpreter.ParseMacro(macroText);
            var actionList = new List<ActionBase>(actions);

            // Inject platform services into actions that require them
            var msgService = _factory.CreateMessageBoxService();
            foreach (var a in actionList)
            {
                if (a is MessageBoxAction mba)
                {
                    mba.MessageBoxService = msgService;
                }
            }

            var macro = new MacroAction();
            // populate the BindingList from the parsed action list
            foreach (var a in actionList)
                macro.Actions.Add(a);

            var rule = new Rule { Name = name, Action = macro, Trigger = new Triggers.EventTrigger() };
            _repo.Add(rule);
            _logger.Log(name, "Created macro rule");
            _historyRepo?.Add(rule.Id, DateTime.UtcNow, "Created", "Macro rule created");
        }

        public void Start()
        {
            _scheduler.Start();
            _logger.Log("Automation Engine", "Started");
            _historyRepo?.Add(0, DateTime.UtcNow, "Started", "Automation Engine started");
        }
        public void Stop()
        {
            _scheduler.Stop();
            _logger.Log("Automation Engine", "Stopped");
            _historyRepo?.Add(0, DateTime.UtcNow, "Stopped", "Automation Engine stopped");
        }

        private void OnRuleReady(Rule rule)
        {
            try
            {
                RuleTriggered?.Invoke(rule);
                _logger.Log(rule.Name, "Rule triggered");
                _historyRepo?.Add(rule.Id, DateTime.UtcNow, "Triggered", "Rule triggered by scheduler");

                // Inject platform services into any actions in the rule before execution
                InjectPlatformServicesIntoAction(rule.Action);

                // Use dispatcher to execute the rule
                _dispatcher.Dispatch(rule);

                RuleExecuted?.Invoke(rule);
                _logger.Log(rule.Name, "Rule executed");
                _historyRepo?.Add(rule.Id, DateTime.UtcNow, "Executed", "Rule executed successfully");
            }
            catch (Exception ex)
            {
                RuleFailed?.Invoke(rule, ex);
                // Log full exception details so stack trace and inner exceptions are available in history
                _logger.Log(rule.Name, $"Error executing rule: {ex}");
                _historyRepo?.Add(rule.Id, DateTime.UtcNow, "Failed", ex.ToString());
            }
        }

        private void InjectPlatformServicesIntoAction(Actions.ActionBase action)
        {
            if (action == null) return;

            // If action is a MessageBoxAction, set the service
            if (action is MessageBoxAction mba)
            {
                // Prefer service from factory
                var svc = _factory.CreateMessageBoxService();
                if (svc != null)
                    mba.MessageBoxService = svc;
            }

            // If action is MacroAction, recurse into children
            if (action is MacroAction mac)
            {
                foreach (var child in mac.Actions)
                {
                    InjectPlatformServicesIntoAction(child);
                }
            }
        }

        public IEnumerable<LogEntry> GetHistory() => _logger.GetAll();
    }
}

