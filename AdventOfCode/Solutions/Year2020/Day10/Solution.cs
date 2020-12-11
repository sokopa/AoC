using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day10 : ASolution
    {
        private IEnumerable<long> _adapterJoltRatings;
        public Day10() : base(10, 2020, "")
        {
            _adapterJoltRatings = Input.SplitByNewline().Select(long.Parse);
        }

        protected override string SolvePartOne()
        {
            var maxJoltage = _adapterJoltRatings.Max();
            var deviceJoltage = maxJoltage + 3;
            var availableAdapters = _adapterJoltRatings.Append(deviceJoltage).OrderBy(i => i).ToList();

            var currentJoltage = 0l;
            var distribution = new Dictionary<int, long> {{1, 0L}, {2, 0L}, {3, 0L}};
            foreach (var jolt in availableAdapters)
            {
                var diff = (int)(jolt - currentJoltage);
                distribution[diff] += 1;
                currentJoltage = jolt;
            }
            /*do
            {
                var canConnectTo = new long[] {currentJoltage + 1, currentJoltage + 2, currentJoltage + 3};
                var iHave = availableAdapters.Intersect(canConnectTo).ToList();
                if (!iHave.Any())
                {
                    Console.WriteLine("No adapters available for starting voltage " + maxJoltage);
                }

                if (iHave.Count > 1)
                {
                    Console.WriteLine("More than 1 adapters available for starting voltage " + maxJoltage);
                }

                var nextInChain = iHave.FirstOrDefault();
                distribution[(int) (nextInChain - currentJoltage)]++;
                currentJoltage = nextInChain;
                if (currentJoltage == maxJoltage)
                {
                    break;
                }
            } while (true);*/

            var prod = distribution[1] * distribution[3];
            return prod.ToString();
        }

        protected override string SolvePartTwo()
        {
            // Dynamic Programming
            var memo = new long[_adapterJoltRatings.Max() + 1];
            // init
            memo[0] = 1;
            var sortedAdapters = _adapterJoltRatings.OrderBy(i => i);
            foreach (var j in sortedAdapters)
            {
                memo[j] = memo.ElementAtOrDefault((int) j - 3) + memo.ElementAtOrDefault((int) j - 2) +
                          memo.ElementAtOrDefault((int) j - 1);
            }

            foreach (var i in memo)
            {
                Console.WriteLine(i);
            }
            
            return memo[^1].ToString();
        }
    }
}
