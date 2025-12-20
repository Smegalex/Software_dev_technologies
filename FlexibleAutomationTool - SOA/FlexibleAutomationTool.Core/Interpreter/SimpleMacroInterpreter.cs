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
            var context = new InterpreterContext(macroText ?? string.Empty);
            if (string.IsNullOrWhiteSpace(macroText))
                return context.Actions;

            var root = new SequenceExpression();

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
                    root.Add(new RunExpression(path, args));
                }
                else if (line.StartsWith("MSG ", StringComparison.OrdinalIgnoreCase))
                {
                    var msg = line.Substring(4).Trim();
                    root.Add(new MessageExpression(msg));
                }
            }
            root.Interpret(context);

            return context.Actions;
        }
    }
}
