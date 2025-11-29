using FlexibleAutomationTool.Core.Interfaces;

namespace FlexibleAutomationTool.Core.Factories
{
    public interface IPlatformFactory
    {
        IMessageBoxService CreateMessageBoxService();
    }
}
