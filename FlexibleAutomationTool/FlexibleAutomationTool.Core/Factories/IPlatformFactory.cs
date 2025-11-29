using FlexibleAutomationTool.Core.Interfaces;

namespace FlexibleAutomationTool.Core.Factories
{
    public interface IPlatformFactory
    {
        IMessageBoxService CreateMessageBoxService();
        // Add other platform-specific creators here
    }
}
