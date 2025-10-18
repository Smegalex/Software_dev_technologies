using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexibleAutomationTool.Core.Repositories;
using FlexibleAutomationTool.Core.Models;

namespace FlexibleAutomationTool.Core.Services
{
        public class AutomationEngine(IRuleRepository repo, Scheduler scheduler, ServiceManager svcManager, Logger logger)
    {
            private readonly IRuleRepository _repo = repo;
            private readonly Scheduler _scheduler = scheduler;
            private readonly ServiceManager _svcManager = svcManager;
            private readonly Logger _logger = logger;
        
        public void CreateRule(Rule rule)
            {
                _repo.Add(rule);
                _logger.Log(rule.Name, "created");
            }

            public void ExecuteRule(Rule rule)
            {
                rule.Execute();
                _logger.Log(rule.Name, "executed");
            }

            public void Start() => _scheduler.Start();
            public void Stop() => _scheduler.Stop();
            public IEnumerable<LogEntry> GetHistory() => _logger.GetAll();
        }
    }

