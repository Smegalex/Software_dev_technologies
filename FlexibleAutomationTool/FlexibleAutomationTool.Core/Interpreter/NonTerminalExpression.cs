using System.Collections.Generic;

namespace FlexibleAutomationTool.Core.Interpreter
{
    public class SequenceExpression : IExpression
    {
        private readonly List<IExpression> _children = new();

        public SequenceExpression() { }

        public void Add(IExpression expr) => _children.Add(expr);

        public void Interpret(InterpreterContext context)
        {
            foreach (var child in _children)
            {
                child.Interpret(context);
            }
        }
    }
}
