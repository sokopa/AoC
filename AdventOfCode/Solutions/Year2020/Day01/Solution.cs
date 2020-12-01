using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day01 : ASolution
    {
        private List<int> _inputs;
        public Day01() : base(01, 2020, "")
        {
            var lines = Input.SplitByNewline();
            _inputs = new List<int>();
            foreach (var line in lines)
            {
                _inputs.Add(int.Parse(line));
            }
        }

        protected override string SolvePartOne()
        {
            var differences = new HashSet<int>();
            foreach (var expense in _inputs)
            {
                if (differences.Contains(2020 - expense))
                {
                    return (expense * (2020 - expense)).ToString();
                }

                differences.Add(expense);
            }

            return "None found";
        }

        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}
