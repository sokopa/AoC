using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day23 : ASolution
    {
        private List<int> _cups;
        public Day23() : base(23, 2020, "")
        {
            _cups = new List<int>
            {
                5, 8, 3, 9, 7, 6, 2, 4, 1
            };
            
            // _cups = new List<int>
            // {
            //     3,8,9,1,2,5,4,6,7
            // };
        }

        protected override string SolvePartOne()
        {
            var max_cup = 9;
            var ind = new Dictionary<int, CircularList>();

            var head = new CircularList(_cups[0]);
            ind[_cups[0]] = head;
            var curr = head;
            
            foreach (var cup in _cups)
            {
                if (cup == _cups[0]) continue;
                curr = curr.InsertNext(cup);
                ind[cup] = curr;
            }

            var currCupNum = head.Data;
            for (int i = 0; i < 100; i++)
            {
                var currentCup = ind[currCupNum];
                var c1 = currentCup.DeleteNext();
                var c2 = currentCup.DeleteNext();
                var c3 = currentCup.DeleteNext();

                var target = Subtract(currCupNum, new[] {c1.Data, c2.Data, c3.Data});
                var targetNode = ind[target];
                targetNode.InsertNext(c3);
                targetNode.InsertNext(c2);
                targetNode.InsertNext(c1);
                currCupNum = currentCup.Next.Data;
            }

            var start = ind[1].Next;
            var sb = new StringBuilder();
            start.Traverse(start, node => sb.Append(node.Data));
            return sb.ToString().Replace("1", "");


        }

        private int Subtract(int curr,  int[] notIn, int maxCup = 9)
        {
            var target = (curr - 1) % maxCup;
            if (target <= 0) target = maxCup;
            while (notIn.Any(c => c == target))
            {
                target = (target - 1) % maxCup;
                if (target <= 0) target = maxCup;
            }
            return target;
        }
       
        protected override string SolvePartTwo()
        {
            var max_cup = 1000000;
            var ind = new Dictionary<int, CircularList>();

            var head = new CircularList(_cups[0]);
            ind[_cups[0]] = head;
            var curr = head;
            
            foreach (var cup in _cups)
            {
                if (cup == _cups[0]) continue;
                curr = curr.InsertNext(cup);
                ind[cup] = curr;
            }

            for (int i = 10; i <= max_cup; i++)
            {
                curr = curr.InsertNext(i);
                ind[i] = curr;
            }
            var currCup = head;
            for (int i = 0; i < 10000000; i++)
            {
                var c1 = currCup.DeleteNext();
                var c2 = currCup.DeleteNext();
                var c3 = currCup.DeleteNext();

                var target = Subtract(currCup.Data, new[] {c1.Data, c2.Data, c3.Data}, max_cup);
                var targetNode = ind[target];
                targetNode.InsertNext(c3);
                targetNode.InsertNext(c2);
                targetNode.InsertNext(c1);
                currCup = currCup.Next;
            }

            var firstStar = ind[1].Next;
            var secondStar = ind[1].Next.Next;
            long mul = (long)firstStar.Data * (long)secondStar.Data;

            return mul.ToString();
        }
    }
}
