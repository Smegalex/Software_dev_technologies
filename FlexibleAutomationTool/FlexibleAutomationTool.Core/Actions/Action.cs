using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Actions
{
    public abstract class Action
    {
        public int Id { get; set; }
        public abstract void Execute();
        public abstract bool Validate();
    }
}
