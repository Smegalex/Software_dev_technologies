using System.Collections.Generic;
using FlexibleAutomationTool.Core.Models;

namespace FlexibleAutomationTool.Core.Repositories
{
    public interface IExecutionHistoryRepository
    {
        void Add(int ruleId, System.DateTime executedAt, string status, string message);
        IEnumerable<LogEntry> GetAll();
        IEnumerable<LogEntry> GetByRuleId(int ruleId);
    }
}
