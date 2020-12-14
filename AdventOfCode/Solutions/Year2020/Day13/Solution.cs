using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day13 : ASolution
    {
        private int _earliestTime;
        private string _frequencies;

        public Day13() : base(13, 2020, "")
        {
            var lines = Input.SplitByNewline();
            var first = lines.FirstOrDefault();
            _earliestTime = int.Parse(first);
            var second = lines.LastOrDefault();
            _frequencies = second;
        }

        protected override string SolvePartOne()
        {
            var freqsClean = _frequencies.Split(",")
                .Where(f => f != "x").Select(int.Parse).OrderBy(i => i);
            var minDiff = freqsClean.Select(f => (f, FirstAfterTarget(f, _earliestTime)))
                .OrderBy(pair => pair.Item2).FirstOrDefault();
            return (minDiff.f * (minDiff.Item2 - _earliestTime)).ToString();
        }

        private int FirstAfterTarget(int freq, int target)
        {
            var curr = freq;
            while (curr < target) curr += freq;
            return curr;
        }

        protected override string SolvePartTwo()
        {
            var restrictions = _frequencies.Split(",").Select((c, i) => (i, c))
                .Where(i => i.c != "x")
                .Select(ic => new {offset = ic.i, frequency = int.Parse(ic.c)}).ToList();

            var prodOfAll = restrictions.Select(i => (long) i.frequency).Aggregate((a, c) => a * c);
            var t = 725850285300475; // this is from Wolfram Alphs

            // Chinese Remainder Theorem
            // All restrictions are (t+offset) % freq = 0 or, t % freq = offset
            
            var n = new List<long>();
            var a = new List<long>();
            foreach (var r in restrictions)
            {
                n.Add(r.frequency);
                a.Add(r.frequency - r.offset);
            }

            var t2 = ChineseRemainderTheorem.Solve(n, a);
            
            Console.WriteLine("T2: " + t2);
            return t.ToString();

            return "Not found";
        }


        public static class ChineseRemainderTheorem
        {
            public static long Solve(IList<long> n, IList<long> a)
            {
                long prod = n.Aggregate(1L, (i, j) => i * j);
                long p;
                long sm = 0;
                for (int i = 0; i < n.Count; i++)
                {
                    p = prod / n[i];
                    sm += a[i] * (long)BigInteger.ModPow(p, n[i] - 2, n[i]) * p;
                }

                return sm % prod;
            }
        }
    }
}