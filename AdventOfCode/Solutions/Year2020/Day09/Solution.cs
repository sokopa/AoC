using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day09 : ASolution
    {
        private List<long> _input;
        public Day09() : base(09, 2020, "")
        {
            _input = Input.SplitByNewline().Select(long.Parse).ToList();
        }

        protected override string SolvePartOne()
        {
            int preamble = 25;
            int windowStart = 0;

            for (int i = preamble; i < _input.Count; i++)
            {
                var combinationsByTwo = SumsByTwo(_input.GetRange(windowStart++, preamble));
                var num = _input[i];
                if (!combinationsByTwo.Contains(num)) return num.ToString();
            }
            
            return null;
        }

        private HashSet<long> SumsByTwo(List<long> input)
        {
            var set = new HashSet<long>();
            for (int i = 0; i < input.Count - 1; i++)
            {
                var a = input[i];
                for (int j = i + 1; j < input.Count; j++)
                {
                    var b = input[j];
                    set.Add(a + b);
                }
            }

            return set;
        }
        

        protected override string SolvePartTwo()
        {
            int preamble = 25;
            int windowStart = 0;
            long number = 0;
            int index = preamble;
            for (;index < _input.Count; index++)
            {
                var combinationsByTwo = SumsByTwo(_input.GetRange(windowStart++, preamble));
                number = _input[index];
                if (!combinationsByTwo.Contains(number)) break;
            }

            if (number != 0 && index != 0)
            {
                // calculate
                var start_end = SubArraySum(_input, number);
                var subArray = _input.GetRange(start_end.start, start_end.end - start_end.start);
                
                return (subArray.Min() + subArray.Max()).ToString();
            }
            
            return null;
        }


        private (int start, int end) SubArraySum(List<long> list, long targetSum)
        {
            var currentSum = list[0];
            var start = 0;
            for (int i = 1; i < list.Count; i++)
            {
                while (currentSum > targetSum && start < i - 1)
                {
                    currentSum -= list[start];
                    start++;
                }

                if (currentSum == targetSum)
                {
                    int end = i - 1;
                    return (start, end);
                }
                
                if (i < _input.Count)
                    currentSum += list[i];
            }

            return (0, 0);
        }
    }
}
