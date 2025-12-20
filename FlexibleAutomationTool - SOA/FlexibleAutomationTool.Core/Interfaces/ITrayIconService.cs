namespace FlexibleAutomationTool.Core.Interfaces
{
    public interface ITrayIconService
    {
        void ShowNotification(string title, string? message = null);
        void SetTooltip(string text);
    }
}
