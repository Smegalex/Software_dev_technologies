using FlexibleAutomationTool.Core.Interfaces;

namespace FlexibleAutomationTool.UI.Services
{
    public class WinFormsMessageBoxService : IMessageBoxService
    {
        public void Show(string message, string? title = null)
        {
            MessageBox.Show(message, title ?? "Info");
        }
    }

}
