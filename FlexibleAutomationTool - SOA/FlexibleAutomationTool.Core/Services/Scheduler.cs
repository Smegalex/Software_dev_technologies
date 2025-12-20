using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FlexibleAutomationTool.Core.Services
{
    public class Scheduler
    {
        private Timer? _timer;
        private readonly IRuleRepository _repository;
        private readonly ITriggerStrategy _strategy;

        // Подія для Engine
        public event Action<Rule>? RuleReady;

        // Prevent overlapping invocations
        private int _isRunning = 0;

        // Lock to make Start/Stop thread-safe and avoid creating multiple timers
        private readonly object _startLock = new();

        public Scheduler(IRuleRepository repository, ITriggerStrategy strategy)
        {
            _repository = repository;
            _strategy = strategy;
        }

        public void Start()
        {
            // Make start thread-safe: only one thread may create the timer
            lock (_startLock)
            {
                if (_timer != null)
                    return;

                _timer = new Timer(CheckAllRules, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            }
        }

        public void Stop()
        {
            // Stop can run concurrently; use Interlocked.Exchange to swap out the timer reference
            var t = Interlocked.Exchange(ref _timer, null);
            t?.Dispose();
        }


        private void CheckAllRules(object? state)
        {
            // Prevent reentrant execution if previous run still in progress
            if (Interlocked.Exchange(ref _isRunning, 1) == 1)
                return;

            try
            {
                foreach (var r in _strategy.GetReadyRules(_repository))
                {
                    RuleReady?.Invoke(r);
                }
            }
            finally
            {
                Interlocked.Exchange(ref _isRunning, 0);
            }
        }
    }
}
