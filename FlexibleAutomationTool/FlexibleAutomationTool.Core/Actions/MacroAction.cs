using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Collections.ObjectModel;

namespace FlexibleAutomationTool.Core.Actions
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MacroAction : ActionBase
    {
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingList<ActionBase> Actions { get; set; } = new BindingList<ActionBase>();

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

        public override string ToString() => $"{GetType().Name} ({Actions?.Count ?? 0} actions)";
    }
}
