namespace AdventOfCode.Solutions.Year2020
{
    internal class NumberNode : Node
    {
        private readonly long _number;

        public NumberNode(long number)
        {
            _number = number;
        }

        public override Token Token => Token.Number;

        public override long Evaluate()
        {
            return _number;
        }
    }
}