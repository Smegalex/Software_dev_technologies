using FlexibleAutomationTool.Core.Actions;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Services
{
    public class AutomationEngine
    {
        private readonly IRuleRepository _repo;
        private readonly Scheduler _scheduler;
        private readonly ServiceManager _serviceManager;
        private readonly Logger _logger;

        public event Action<Rule>? RuleTriggered;
        public event Action<Rule>? RuleExecuted;
        public event Action<Rule, Exception>? RuleFailed;

        public AutomationEngine(IRuleRepository repo, Scheduler scheduler, ServiceManager svcManager, Logger logger)
        {
            _repo = repo;
            _scheduler = scheduler;
            _serviceManager = svcManager;
            _logger = logger;

            _scheduler.RuleReady += OnRuleReady;
        }

        public void CreateRule(Rule rule)
        {
            _logger.Log(rule.Name, "Created");
        }


        public void Start() {
            _scheduler.Start();
            _logger.Log("Automation Engine", "Started");
        }
        public void Stop() { 
            _scheduler.Stop();
            _logger.Log("Automation Engine", "Stopped");
        }

        private void OnRuleReady(Rule rule)
        {
            try
            {
                RuleTriggered?.Invoke(rule);
                _logger.Log(rule.Name, "Rule triggered");

                rule.Execute();

                RuleExecuted?.Invoke(rule);
                _logger.Log(rule.Name, "Rule executed");
            }
            catch (Exception ex)
            {
                RuleFailed?.Invoke(rule, ex);
                // Log full exception details so stack trace and inner exceptions are available in history
                _logger.Log(rule.Name, $"Error executing rule: {ex}");
            }
        }

        public IEnumerable<LogEntry> GetHistory() => _logger.GetAll();
    }
}

