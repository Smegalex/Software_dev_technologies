using FlexibleAutomationTool.Core.Interfaces;

namespace FlexibleAutomationTool.Core.Factories
{
    public interface IPlatformFactory
    {
        IMessageBoxService CreateMessageBoxService();
        IClipboardService CreateClipboardService();
        IFilePickerService CreateFilePickerService();
        ITrayIconService CreateTrayIconService();
    }
}
