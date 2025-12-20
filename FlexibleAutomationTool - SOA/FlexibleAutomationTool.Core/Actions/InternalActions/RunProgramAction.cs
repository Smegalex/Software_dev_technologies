using System.Diagnostics;

namespace FlexibleAutomationTool.Core.Actions.InternalActions
{
    public class RunProgramAction : ActionBase
    {
        public string Path { get; set; } = "";
        public string? Arguments { get; set; }

        public override void Execute()
        {
            try
            {
                var psi = new ProcessStartInfo(Path)
                {
                    Arguments = Arguments ?? string.Empty,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException($"Failed to start process '{Path}': {ex.Message}", ex);
            }
        }

        public override bool Validate() => !string.IsNullOrWhiteSpace(Path);
    }
}
