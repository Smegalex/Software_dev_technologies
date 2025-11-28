using System.Diagnostics;

namespace FlexibleAutomationTool.Core.Actions.InternalActions
{
    public class OpenUrlAction : ActionBase
    {
        public string Url { get; set; } = "";

        public override void Execute()
        {
            Process.Start(new ProcessStartInfo(Url) { UseShellExecute = true });
        }

        public override bool Validate() => Url.StartsWith("http");
    }
}
