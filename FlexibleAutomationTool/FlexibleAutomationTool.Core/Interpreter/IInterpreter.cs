using System.Collections.Generic;
using FlexibleAutomationTool.Core.Actions;

namespace FlexibleAutomationTool.Core.Interpreter
{
    public interface IInterpreter
    {
        IEnumerable<ActionBase> ParseMacro(string macroText);
    }
}
