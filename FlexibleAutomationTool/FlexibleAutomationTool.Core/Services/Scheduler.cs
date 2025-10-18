using FlexibleAutomationTool.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Services
{
    public class Scheduler
    {
        private Timer? _timer;
        private readonly IRuleRepository _repository;

        public Scheduler(IRuleRepository repository)
        {
            _repository = repository;
        }

        public void Start()
        {
            _timer = new Timer(CheckAllRules, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }

        public void Stop() => _timer?.Dispose();

        private void CheckAllRules(object? state)
        {
            foreach (var r in _repository.GetAll())
                r.CheckTrigger();
        }
    }
}
