using System.Diagnostics;

namespace FlexibleAutomationTool.Core.Actions.InternalActions
{
    public class RunProgramAction : ActionBase
    {
        public string Path { get; set; } = "";
        public string? Arguments { get; set; }

        public override void Execute()
        {
            Process.Start(Path, Arguments ?? "");
        }

        public override bool Validate() => !string.IsNullOrWhiteSpace(Path);
    }
}
