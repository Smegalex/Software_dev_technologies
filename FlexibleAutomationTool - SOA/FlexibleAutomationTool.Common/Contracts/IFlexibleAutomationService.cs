using System.Collections.Generic;

namespace FlexibleAutomationTool.Common.Contracts
{
    public interface IFlexibleAutomationService
    {
        IEnumerable<RuleDto> GetRules();
        void ExecuteRule(int id, string? decryptedPayload = null);
    }
}