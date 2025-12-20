using FlexibleAutomationTool.Core.Models;

namespace FlexibleAutomationTool.Core.Services
{
    public interface ICommandDispatcher
    {
        void Dispatch(Rule rule);
    }
}
