namespace FlexibleAutomationTool.Core.Interfaces
{
    public interface IMessageBoxService
    {
        void Show(string message, string? title = null);
    }
}
