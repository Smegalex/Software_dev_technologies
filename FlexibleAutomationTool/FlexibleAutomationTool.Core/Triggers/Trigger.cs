using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlexibleAutomationTool.Core.Triggers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class Trigger
    {
        [Browsable(false)]
        public int Id { get; set; }
        public abstract bool ShouldExecute();
        public abstract bool Validate();

        public override string ToString() => GetType().Name;
    }
}
