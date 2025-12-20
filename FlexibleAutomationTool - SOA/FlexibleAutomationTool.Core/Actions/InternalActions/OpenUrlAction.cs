using System.Diagnostics;

namespace FlexibleAutomationTool.Core.Actions.InternalActions
{
    public class OpenUrlAction : ActionBase
    {
        public string Url { get; set; } = "";

        public override void Execute()
        {
            try
            {
                Process.Start(new ProcessStartInfo(Url) { UseShellExecute = true });
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException($"Failed to open URL '{Url}': {ex.Message}", ex);
            }
        }

        public override bool Validate() => Url.StartsWith("http");
    }
}
