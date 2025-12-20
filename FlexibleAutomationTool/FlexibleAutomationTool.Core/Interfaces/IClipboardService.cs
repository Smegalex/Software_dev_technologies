namespace FlexibleAutomationTool.Core.Interfaces
{
    public interface IClipboardService
    {
        void SetText(string text);
        string? GetText();
    }
}
