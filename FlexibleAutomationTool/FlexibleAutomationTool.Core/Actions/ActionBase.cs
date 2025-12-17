namespace FlexibleAutomationTool.Core.Actions
{
    using System.ComponentModel;

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ActionBase
    {
        [Browsable(false)]
        public int Id { get; set; }
        public abstract void Execute();
        public abstract bool Validate();

        public override string ToString() => GetType().Name;
    }
}
