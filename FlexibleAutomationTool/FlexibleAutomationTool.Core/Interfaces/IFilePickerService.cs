namespace FlexibleAutomationTool.Core.Interfaces
{
    public interface IFilePickerService
    {
        string? PickFile(string filter = "All files|*.*");
    }
}
