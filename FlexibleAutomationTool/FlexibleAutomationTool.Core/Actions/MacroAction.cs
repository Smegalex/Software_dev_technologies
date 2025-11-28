using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Actions
{
    public class MacroAction : ActionBase
    {
        public List<ActionBase> Actions { get; set; } = new();

        public override void Execute()
        {
            foreach (var a in Actions)
                a.Execute();
        }

        public override bool Validate() => Actions.Count > 0;
    }
}
