namespace AdventOfCode.Solutions.Year2020
{
    internal abstract class Node
    {
        public abstract Token Token { get; }
        public abstract long Evaluate();
    }
}