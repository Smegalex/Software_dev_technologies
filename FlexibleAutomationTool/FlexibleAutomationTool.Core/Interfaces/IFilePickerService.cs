namespace FlexibleAutomationTool.Core.Interfaces
{
    public interface IFilePickerService
    {
        // Returns null if user cancelled
        string? PickFile(string filter = "All files|*.*");
    }
}
