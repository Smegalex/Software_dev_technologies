using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using System.Collections.Generic;

namespace FlexibleAutomationTool.Core.Services
{
    public class PollingTriggerStrategy : ITriggerStrategy
    {
        public IEnumerable<Rule> GetReadyRules(IRuleRepository repository)
        {
            foreach (var r in repository.GetAll())
            {
                if (r.CheckTrigger())
                    yield return r;
            }
        }
    }
}
