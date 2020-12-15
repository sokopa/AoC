using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day15 : ASolution
    {
        private List<int> _seed;
        public Day15() : base(15, 2020, "")
        {
            //DebugInput = "0,3,6";
            _seed = Input.Split(",").Select(int.Parse).ToList();
        }

        protected override string SolvePartOne()
        {
            return Solve(2020);
        }

        protected override string SolvePartTwo()
        {
            return Solve(30000000);
        }

        private string Solve(int target)
        {
            var lastSeen = new Dictionary<int, int>(); // key = value, value = round in which I saw it last
            var secondToLastSeen = new Dictionary<int, int>(); // almost same as before
            int round = 1;
            int prevNum = 0;
            // init
            foreach (var val in _seed)
            {
                if (lastSeen.ContainsKey(val))
                {
                    secondToLastSeen[val] = lastSeen[val];
                }

                lastSeen[val] = round;
                prevNum = val;
                round++;
            }

            for (; round <= target; round++)
            {
                // we know that lastSeen(val) = round-1;
                // check for secondToLastSeen
                var nextNumber = 0;
                if (secondToLastSeen.TryGetValue(prevNum, out var prevOccur))
                {
                    // i found it. I calculate the diff
                    var diff = lastSeen[prevNum] - prevOccur;
                    nextNumber = diff;
                }

                if (lastSeen.ContainsKey(nextNumber))
                {
                    secondToLastSeen[nextNumber] = lastSeen[nextNumber];
                }

                lastSeen[nextNumber] = round;
                prevNum = nextNumber;
            }

            return prevNum.ToString();
        }
    }
}
