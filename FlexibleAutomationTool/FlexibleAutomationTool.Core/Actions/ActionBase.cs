namespace FlexibleAutomationTool.Core.Actions
{
    using System.ComponentModel;
    using System;

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ActionBase
    {
        [Browsable(false)]
        public int Id { get; set; }

        // Allows UI or invoker to enable/disable command without changing implementation
        public bool IsEnabled { get; set; } = true;

        // Execute the command
        public abstract void Execute();

        // Validate configuration before saving or execution
        public abstract bool Validate();

        // Optional undo support - override in actions that can be undone
        [Browsable(false)]
        public virtual bool CanUndo => false;

        public virtual void Undo()
        {
            throw new NotSupportedException("Undo is not supported for this action.");
        }

        public override string ToString() => GetType().Name;
    }
}
