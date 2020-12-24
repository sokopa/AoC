using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    public class Direction
    {
        public List<HexDirection> Order { get; set; }

        public (int, int) ProcessOrders()
        {
            var curr = (x: 0, y: 0);
            
            foreach (var hexDirection in Order)
            {
                // https://www.redblobgames.com/grids/hexagons/#neighbors-axial
                curr = hexDirection switch
                {
                    HexDirection.e => (curr.x + 1, curr.y),
                    HexDirection.se => (curr.x, curr.y + 1),
                    HexDirection.sw => (curr.x - 1, curr.y + 1),
                    HexDirection.w => (curr.x - 1, curr.y),
                    HexDirection.nw => (curr.x, curr.y - 1),
                    HexDirection.ne => (curr.x + 1, curr.y - 1),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return curr;
        }

        public static Direction FromLine(string line)
        {
            var ret = new Direction
            {
                Order = new List<HexDirection>()
            };
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                var s = c.ToString();
                if (s == "n" || s == "s")
                {
                    i++;
                    s += line[i];
                }

                HexDirection.TryParse<HexDirection>(s, out var hd);
                ret.Order.Add(hd);
            }

            return ret;
        }

        public static List<(int x, int y)> FindNeighbors((int x, int y) curr)
        {
            return new List<(int x, int y)>
            {
                (curr.x + 1, curr.y),
                (curr.x, curr.y + 1),
                (curr.x - 1, curr.y + 1),
                (curr.x - 1, curr.y),
                (curr.x, curr.y - 1),
                (curr.x + 1, curr.y - 1),
            };
        }
    }
    

    public enum HexDirection
    {
        e,
        se,
        sw,
        w,
        nw,
        ne
    }
    
    class Day24 : ASolution
    {
        private List<Direction> input;
        public Day24() : base(24, 2020, "")
        {
            input = Input.SplitByNewline().Select(Direction.FromLine).ToList();
        }

        protected override string SolvePartOne()
        {
            var flipped = new Dictionary<(int x, int y), bool>();
            foreach (var direction in input)
            {
                var finalPoint = direction.ProcessOrders();
                if (flipped.ContainsKey(finalPoint))
                {
                    flipped[finalPoint] = !flipped[finalPoint];
                }
                else
                {
                    flipped[finalPoint] = true;
                }
            }

            var flippedCount = flipped.Values.Count(b => b);
            return flippedCount.ToString();
        }

        protected override string SolvePartTwo()
        {
            // Hex conway crossover episode
            var flipped = new Dictionary<(int x, int y), bool>();
            foreach (var direction in input)
            {
                var finalPoint = direction.ProcessOrders();
                if (flipped.ContainsKey(finalPoint))
                {
                    flipped[finalPoint] = !flipped[finalPoint];
                }
                else
                {
                    flipped[finalPoint] = true;
                }
            }
            
            // remove whites
            flipped = flipped.Where(kvp => kvp.Value)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            for (int i = 1; i <= 100; i++)
            {
                // Day i
                var next = new Dictionary<(int x, int y), bool>();
                
                // populate whites around blacks
                foreach (var entry in flipped.ToList())
                {
                    var neighbors = Direction.FindNeighbors(entry.Key);
                    foreach (var neighbor in neighbors)
                    {
                        if (!flipped.ContainsKey(neighbor)) flipped[neighbor] = false;
                    }
                }
                foreach (var entry in flipped)
                {
                    var n = Direction.FindNeighbors(entry.Key);
                    var count = n.Select((p) => flipped.GetValueOrDefault(p,false) ? 1 : 0).Sum();
                    
                    if ((entry.Value && (count == 1 || count == 2))
                        || 
                        (!entry.Value && count == 2))
                    {
                        // turn to black
                        next[entry.Key] = true;
                    }
                    else
                    {
                        // turn to white
                        next[entry.Key] = false;
                    }
                }
                flipped = next.Where(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            var flippedCount = flipped.Values.Count(b => b);
            return flippedCount.ToString();
        }
    }
}
