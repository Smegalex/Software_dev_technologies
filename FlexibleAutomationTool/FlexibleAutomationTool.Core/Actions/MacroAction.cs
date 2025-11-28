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
            List<Exception> errors = null;

            foreach (var a in Actions)
            {
                try
                {
                    a.Execute();
                }
                catch (Exception ex)
                {
                    // collect exceptions to report after attempting all actions
                    errors ??= new List<Exception>();
                    errors.Add(ex);
                }
            }

            if (errors != null && errors.Count > 0)
            {
                throw new AggregateException("One or more actions in the macro failed.", errors);
            }
        }

        public override bool Validate() => Actions.Count > 0;
    }
}
