using FlexibleAutomationTool.Core.Interfaces;
using System.Windows.Forms;

namespace FlexibleAutomationTool.UI.Services
{
    public class WinFormsTrayIconService : ITrayIconService
    {
        private readonly NotifyIcon _notify = new() { Visible = true };

        public void SetTooltip(string text)
        {
            try { _notify.Text = text; } catch { }
        }

        public void ShowNotification(string title, string? message = null)
        {
            try { _notify.BalloonTipTitle = title; _notify.BalloonTipText = message ?? string.Empty; _notify.ShowBalloonTip(3000); } catch { }
        }
    }
}
