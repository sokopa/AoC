using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day11 : ASolution
    {
        // this reminds me a lot of Conway's game of life
        private GameOfSeatsSimulation sim;
        public Day11() : base(11, 2020, "")
        {
            sim = new GameOfSeatsSimulation();
            sim.Init(Input.SplitByNewline());
        }

        protected override string SolvePartOne()
        {
            var changed = false;
            do
            {
                changed = sim.Update();
            } while (changed);

            return sim.NumberOfOccupiedSeats().ToString();
        }

        protected override string SolvePartTwo()
        {
            var changed = false;
            do
            {
                changed = sim.Update(sim.CountFirstVisibleOccupiedAround, 5);
            } while (changed);

            return sim.NumberOfOccupiedSeats().ToString();
        }
    }

    public class GameOfSeatsSimulation
    {
        private State[,] floorplan;
        private int width;
        private int height;
        public void Init(string[] input)
        {
            width = input[0].Length;
            height = input.Length;
            floorplan = new State[width,height];
            for (int r = 0; r < input.Length; r++)
            {
                var row = input[r];
                for (int c = 0; c < row.Length; c++)
                {
                    floorplan[c, r] = (State) row[c];
                }
            }
        }

        public bool Update(Func<int, int, int> countOccupied = null, int threshold = 4)
        {
            if (countOccupied == null)
            {
                countOccupied = CountOccupiedAround;
            }
            var changed = false;
            var newPlan = new State[width, height];
            for (int row = 0; row < height; row++)
            for (int col = 0; col < width; col++)
            {
                var current = floorplan[col, row];
                var numOccupied = countOccupied(row, col);
                switch (current)
                {
                    case State.Empty when numOccupied == 0:
                        newPlan[col, row] = State.Occupied;
                        changed = true;
                        break;
                    case State.Occupied when numOccupied >= threshold:
                        newPlan[col, row] = State.Empty;
                        changed = true;
                        break;
                    default:
                        newPlan[col, row] = current;
                        break;
                }
            }

            floorplan = newPlan;
            //DrawBoard();
            //Console.WriteLine($"Occupied seats: {NumberOfOccupiedSeats()}");
            return changed;
        }

        public int CountOccupiedAround(int col, int row)
        {
            return IsNeighborOccupied(row, col, -1, -1)
                   + IsNeighborOccupied(row, col, -1, 0)
                   + IsNeighborOccupied(row, col, -1, +1)
                   + IsNeighborOccupied(row, col, 0, -1)
                   + IsNeighborOccupied(row, col, 0, +1)
                   + IsNeighborOccupied(row, col, +1, -1)
                   + IsNeighborOccupied(row, col, +1, 0)
                   + IsNeighborOccupied(row, col, +1, +1);
        }
        
        private int IsNeighborOccupied(int x, int y, int xOffset, int yOffset)
        {
            var result = 0;
            int newX = x + xOffset;
            int newY = y + yOffset;
            var outOfBounds = newX < 0 || newX >= width || newY < 0 || newY >= height;
            if (!outOfBounds)
            {
                result = floorplan[newX, newY] == State.Occupied ? 1 : 0;
            }
            return result;
        }

        public int CountFirstVisibleOccupiedAround(int col, int row)
        {
            return IsVisibleNeighborOccupied(row, col, -1, -1)
                   + IsVisibleNeighborOccupied(row, col, -1, 0)
                   + IsVisibleNeighborOccupied(row, col, -1, +1)
                   + IsVisibleNeighborOccupied(row, col, 0, -1)
                   + IsVisibleNeighborOccupied(row, col, 0, +1)
                   + IsVisibleNeighborOccupied(row, col, +1, -1)
                   + IsVisibleNeighborOccupied(row, col, +1, 0)
                   + IsVisibleNeighborOccupied(row, col, +1, +1);
        }

        private int IsVisibleNeighborOccupied(int x, int y, int xOffset, int yOffset)
        {
            var result = 0;
            int newX = x + xOffset;
            int newY = y + yOffset;
            var outOfBounds = newX < 0 || newX >= width || newY < 0 || newY >= height;
            if (!outOfBounds)
            {
                if (floorplan[newX, newY] == State.Floor)
                {
                    // if i take the sign, it will only add +1,0,-1 to the direction x or y is already looking to
                    result = IsVisibleNeighborOccupied(x, y, xOffset + Math.Sign(xOffset), yOffset + Math.Sign(yOffset));
                }
                else
                {
                    result = floorplan[newX, newY] == State.Occupied ? 1 : 0;
                }
            }
            return result;
        }

        private void DrawBoard()
        {
            var builder = new StringBuilder();
 
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    char c = (char) floorplan[x, y];
 
                    // Each cell is two characters wide.
                    builder.Append(c);
                }
                builder.Append('\n');
            }
 
            // Write the string to the console.
            
            Console.Write(builder.ToString());
        }

        public int NumberOfOccupiedSeats()
        {
            var count = 0;
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    count += floorplan[x, y] == State.Occupied ? 1 : 0; 
                }
            }
            return count;
        }
    }

    enum State
    {
        Floor = '.',
        Empty = 'L',
        Occupied = '#'
    }
}
