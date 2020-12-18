using System;

namespace AdventOfCode.Solutions.Year2020
{
    internal class BinaryNode : Node
    {
        private readonly Node _lhs;
        private readonly Node _rhs;
        private readonly Func<long, long, long> _operation;

        public BinaryNode(Node lhs, Node rhs,  Token token, Func<long, long, long> operation)
        {
            _lhs = lhs;
            _rhs = rhs;
            _operation = operation;
            Token = token;
        }

        public override Token Token { get; }

        public override long Evaluate()
        {
            var lhsValue = _lhs.Evaluate();
            var rhsValue = _rhs.Evaluate();
            var res = _operation(lhsValue, rhsValue);
            return res;
        }
    }
}