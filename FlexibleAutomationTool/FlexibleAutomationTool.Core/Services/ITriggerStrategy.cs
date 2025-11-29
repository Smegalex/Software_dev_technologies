using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using System.Collections.Generic;

namespace FlexibleAutomationTool.Core.Services
{
    public interface ITriggerStrategy
    {
        IEnumerable<Rule> GetReadyRules(IRuleRepository repository);
    }
}
