using FlexibleAutomationTool.Core.Models;
using System;
using System.Collections.Generic;

namespace FlexibleAutomationTool.Core.Facades
{
    public interface IAutomationFacade
    {
        event Action<Rule>? RuleTriggered;
        event Action<Rule>? RuleExecuted;
        event Action<Rule, Exception>? RuleFailed;

        void Start();
        void Stop();

        IEnumerable<LogEntry> GetHistory();

        void CreateMacroRule(string name, string macroText);
        void CreateRule(Rule rule);
    }
}
