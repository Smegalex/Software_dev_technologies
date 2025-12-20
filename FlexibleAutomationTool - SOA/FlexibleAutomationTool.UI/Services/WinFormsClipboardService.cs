using FlexibleAutomationTool.Core.Interfaces;
using System.Windows.Forms;

namespace FlexibleAutomationTool.UI.Services
{
    public class WinFormsClipboardService : IClipboardService
    {
        public string? GetText()
        {
            try { return Clipboard.GetText(); } catch { return null; }
        }

        public void SetText(string text)
        {
            try { Clipboard.SetText(text); } catch { }
        }
    }
}
