using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day17 : ASolution
    {
        private string[] input;

        public Day17() : base(17, 2020, "")
        {
//             DebugInput = @".#.
// ..#
// ###";
            input = Input.SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            var grid = new Grid(input);
            var active = 0;
            for (var i = 1; i <= 6; i++)
            {
                active = grid.RunOneGeneration();
                Console.WriteLine($"Active After Round {i}: [{active}]");
            }

            return active.ToString();
        }

        protected override string SolvePartTwo()
        {
            var grid = new Grid(input, true);
            var active = 0;
            for (var i = 1; i <= 6; i++)
            {
                active = grid.RunOneGeneration(true);
                Console.WriteLine($"Active After Round {i}: [{active}]");
            }

            return active.ToString();
        }
    }

    internal enum CellState
    {
        Inactive = '.',
        Active = '#'
    }

    internal class Grid
    {
        // x, y, z
        private HashSet<Cell> _cube;

        public Grid(string[] input, bool is4d = false)
        {
            _cube = new HashSet<Cell>(CellComparer.Instance);
            for (var row = 0; row < input.Length; row++) // row = y                     
            for (var col = 0; col < input[row].Length; col++) // col = x
            {
                var toAdd = new Cell(col, row, 0, (CellState) input[row][col]);
                ForceAdd(_cube, toAdd);
                foreach (var neighbor in CreateExpansion(toAdd, is4d))
                    if (!_cube.Contains(neighbor))
                        _cube.Add(neighbor);
            }
        }

        private void ForceAdd(HashSet<Cell> set, Cell item)
        {
            if (set.Contains(item)) set.Remove(item);

            set.Add(item);
        }

        public int RunOneGeneration(bool is4d = false)
        {
            var activeAfterRound = 0;
            var nextGen = new HashSet<Cell>(CellComparer.Instance);
            foreach (var item in _cube)
            {
                var neighbors = CountActiveNeighbors(item, is4d);
                GC.Collect();
                var newState = item.State switch
                {
                    CellState.Inactive when neighbors == 3 => CellState.Active,
                    CellState.Active when neighbors == 2 => CellState.Active,
                    CellState.Active when neighbors == 3 => CellState.Active,
                    _ => CellState.Inactive
                };
                if (newState == CellState.Active) activeAfterRound++;
                ForceAdd(nextGen, new Cell(item.X, item.Y, item.Z, item.W, newState));
                // pseudo-expand the surrounding space, add missing neighbors
                foreach (var neighbor in CreateExpansion(item, is4d))
                    if (!nextGen.Contains(neighbor))
                        nextGen.Add(neighbor);
            }

            _cube = nextGen;
            return activeAfterRound;
        }

        private int CountActiveNeighbors(Cell cell, bool is4d = false)
        {
            var count = 0;
            var wMult = is4d ? 1 : 0;
            for (var xTarget = cell.X - 1; xTarget <= cell.X + 1; xTarget++)
            for (var yTarget = cell.Y - 1; yTarget <= cell.Y + 1; yTarget++)
            for (var zTarget = cell.Z - 1; zTarget <= cell.Z + 1; zTarget++)
            for (var wTarget = cell.W - 1 * wMult; wTarget <= cell.W + 1 * wMult; wTarget++)
                if ((xTarget != cell.X || yTarget != cell.Y || zTarget != cell.Z || wTarget != cell.W)
                    && ContainsActiveAt(xTarget, yTarget, zTarget, wTarget))
                    count++;

            return count;
        }

        private IEnumerable<Cell> CreateExpansion(Cell cell, bool is4d = false)
        {
            var wMult = is4d ? 1 : 0;
            for (var xTarget = cell.X - 1; xTarget <= cell.X + 1; xTarget++)
            for (var yTarget = cell.Y - 1; yTarget <= cell.Y + 1; yTarget++)
            for (var zTarget = cell.Z - 1; zTarget <= cell.Z + 1; zTarget++)
            for (var wTarget = cell.W - 1 * wMult; wTarget <= cell.W + 1 * wMult; wTarget++)
                if (xTarget != cell.X || yTarget != cell.Y || zTarget != cell.Z || wTarget != cell.W)
                {
                    var toAdd = new Cell(xTarget, yTarget, zTarget, wTarget, CellState.Inactive);
                    yield return toAdd;
                }
        }

        private bool ContainsActiveAt(int x, int y, int z, int w = 0)
        {
            var toCheck = new Cell(x, y, z, w);
            if (_cube.TryGetValue(toCheck, out var actual))
                if (actual.State == CellState.Active)
                    return true;
            return false;
        }
    }

    internal class CellComparer : IEqualityComparer<Cell>
    {
        public bool Equals(Cell x, Cell y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.X == y.X && x.Y == y.Y && x.Z == y.Z && x.W == y.W;
        }

        public int GetHashCode(Cell obj)
        {
            return HashCode.Combine(obj.X, obj.Y, obj.Z, obj.W);
        }

        public static readonly CellComparer Instance = new CellComparer();
    }

    internal class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }
        public CellState State { get; set; }

        public Cell(int x, int y, int z, int w = 0, CellState state = CellState.Inactive)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
            State = state;
        }

        public Cell(int x, int y, int z, CellState state = CellState.Inactive, int w = 0)
            : this(x, y, z, w, state)
        {
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z},{W}) : {State}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }
    }
}