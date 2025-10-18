using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Triggers
{
    public class TimeTrigger : Trigger
    {
        // run at a specific hour/minute
        public int Hour { get; set; }
        public int Minute { get; set; }

        private DateTime _lastRun = DateTime.MinValue;

        public override bool ShouldExecute()
        {
            var now = DateTime.Now;
            if (now.Hour == Hour && now.Minute == Minute)
            {
                if (_lastRun.Date != now.Date || _lastRun.Hour != now.Hour || _lastRun.Minute != now.Minute)
                {
                    _lastRun = now;
                    return true;
                }
            }
            return false;
        }
        public override bool Validate() => Hour >= 0 && Hour < 24 && Minute >= 0 && Minute < 60;
    }
    }
