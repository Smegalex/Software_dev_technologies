using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlexibleAutomationTool.Core.Models
{
    public class Rule
    {       
        [Browsable(false)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public FlexibleAutomationTool.Core.Triggers.Trigger Trigger { get; set; } = null!;
        public FlexibleAutomationTool.Core.Actions.ActionBase Action { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        public bool CheckTrigger()
        {
            return Trigger?.ShouldExecute() ?? false;
        }

        public void Execute()
        {
            Action.Execute();
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? base.ToString() : Name;
        }
    }
}
