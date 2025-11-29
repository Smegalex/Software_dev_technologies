using System;
using System.Collections.Generic;
using FlexibleAutomationTool.Core.Actions;
using FlexibleAutomationTool.Core.Actions.InternalActions;

namespace FlexibleAutomationTool.Core.Interpreter
{
    public class SimpleMacroInterpreter : IInterpreter
    {
        public IEnumerable<ActionBase> ParseMacro(string macroText)
        {
            var list = new List<ActionBase>();
            if (string.IsNullOrWhiteSpace(macroText))
                return list;

            var lines = macroText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (line.StartsWith("RUN ", StringComparison.OrdinalIgnoreCase))
                {
                    var rest = line.Substring(4).Trim();
                    var parts = rest.Split(' ', 2);
                    var path = parts[0];
                    var args = parts.Length > 1 ? parts[1] : null;
                    list.Add(new RunProgramAction { Path = path, Arguments = args });
                }
                else if (line.StartsWith("MSG ", StringComparison.OrdinalIgnoreCase))
                {
                    var msg = line.Substring(4).Trim();
                    // Title not supported in shorthand
                    list.Add(new MessageBoxAction { Message = msg, Title = "" });
                }
                // add other simple commands as needed
            }

            return list;
        }
    }
}
