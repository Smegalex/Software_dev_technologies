namespace FlexibleAutomationTool.Core.Actions
{
    public abstract class ActionBase
    {
        public int Id { get; set; }
        public abstract void Execute();
        public abstract bool Validate();
    }
}
