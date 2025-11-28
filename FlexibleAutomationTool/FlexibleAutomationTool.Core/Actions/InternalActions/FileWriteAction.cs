namespace FlexibleAutomationTool.Core.Actions.InternalActions
{
    public class FileWriteAction : ActionBase
    {
        public string FilePath { get; set; } = "";
        public string Content { get; set; } = "";

        public override void Execute()
        {
            File.WriteAllText(FilePath, Content);
        }

        public override bool Validate() => !string.IsNullOrWhiteSpace(FilePath);
    }
}
