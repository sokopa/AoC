using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day11 : ASolution
    {
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
            return null;
        }
    }

    public class GameOfSeatsSimulation
    {
        private State[,] floorplan;
        private Stack<State[,]> history;
        private int width;
        private int height;
        public void Init(string[] input)
        {
            width = input[0].Length;
            height = input.Length;
            floorplan = new State[width,height];
            history = new Stack<State[,]>();
            for (int r = 0; r < input.Length; r++)
            {
                var row = input[r];
                for (int c = 0; c < row.Length; c++)
                {
                    floorplan[c, r] = (State) row[c];
                }
            }
            //DrawBoard();
            Console.WriteLine($"Occupied seats: {NumberOfOccupiedSeats()}");
            history.Push(floorplan);
        }

        public bool Update()
        {
            var changed = false;
            var newPlan = new State[width, height];
            for (int row = 0; row < height; row++)
            for (int col = 0; col < width; col++)
            {
                var current = floorplan[col, row];
                var numOccupied = CountOccupiedAround(row, col);
                switch (current)
                {
                    case State.Empty when numOccupied == 0:
                        newPlan[col, row] = State.Occupied;
                        changed = true;
                        break;
                    case State.Occupied when numOccupied >= 4:
                        newPlan[col, row] = State.Empty;
                        changed = true;
                        break;
                    default:
                        newPlan[col, row] = current;
                        break;
                }
            }

            floorplan = newPlan;
            history.Push(floorplan);
            //DrawBoard();
            Console.WriteLine($"Occupied seats: {NumberOfOccupiedSeats()}");
            return changed;
        }

        private int CountOccupiedAround(int col, int row)
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
