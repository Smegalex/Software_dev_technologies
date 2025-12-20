using System;
using FlexibleAutomationTool.Core.Actions.InternalActions;

namespace FlexibleAutomationTool.Core.Interpreter
{
    // Terminal expression for RUN command
    public class RunExpression : IExpression
    {
        private readonly string _path;
        private readonly string _args;

        public RunExpression(string path, string args)
        {
            _path = path;
            _args = args;
        }

        public void Interpret(InterpreterContext context)
        {
            context.Actions.Add(new RunProgramAction { Path = _path, Arguments = _args });
        }
    }

    // Terminal expression for MSG command
    public class MessageExpression : IExpression
    {
        private readonly string _message;

        public MessageExpression(string message)
        {
            _message = message;
        }

        public void Interpret(InterpreterContext context)
        {
            context.Actions.Add(new MessageBoxAction { Message = _message, Title = string.Empty });
        }
    }
}
