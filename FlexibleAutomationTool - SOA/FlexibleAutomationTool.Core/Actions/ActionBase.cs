
using System.ComponentModel;
using System;

namespace FlexibleAutomationTool.Core.Actions
{

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ActionBase
    {
        [Browsable(false)]
        public int Id { get; set; }

        public bool IsEnabled { get; set; } = true;

        public abstract void Execute();

        public abstract bool Validate();

        [Browsable(false)]
        public virtual bool CanUndo => false;

        public virtual void Undo()
        {
            throw new NotSupportedException("Undo is not supported for this action.");
        }

        public override string ToString() => GetType().Name;
    }
}
