using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Triggers
{
    public abstract class Trigger
    {
        public int Id { get; set; }
        public abstract bool ShouldExecute();
        public abstract bool Validate();
    }
}
