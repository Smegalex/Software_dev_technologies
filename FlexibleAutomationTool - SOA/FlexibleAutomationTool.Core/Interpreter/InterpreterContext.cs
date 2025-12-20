using System.Collections.Generic;
using FlexibleAutomationTool.Core.Actions;

namespace FlexibleAutomationTool.Core.Interpreter
{
    public class InterpreterContext
    {
        public InterpreterContext(string input)
        {
            Input = input;
            Actions = new List<ActionBase>();
        }

        public string Input { get; }

        public List<ActionBase> Actions { get; }
    }
}
