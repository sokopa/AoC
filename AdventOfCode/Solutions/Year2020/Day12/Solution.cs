using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day12 : ASolution
    {
        private List<Instruction> _navigation;
        public Day12() : base(12, 2020, "")
        {
            _navigation = Input.SplitByNewline().Select(Instruction.FromLine).ToList();
        }

        /*
         * Ship starts facing east. 0 degrees means:
         *     |
         *  -- > -- x
         *     |
         *     y
         */
        
        protected override string SolvePartOne()
        {
            var degrees = 0;
            var x = 0;
            var y = 0;
            foreach (var instruction in _navigation)
            {
                switch (instruction.Direction)
                {
                    case Direction.North:
                        y += instruction.By;
                        break;
                    case Direction.South:
                        y -= instruction.By;
                        break;
                    case Direction.East:
                        x += instruction.By;
                        break;
                    case Direction.West:
                        x -= instruction.By;
                        break;
                    case Direction.Left:
                        degrees = NormalizeOrientation(degrees - instruction.By);
                        break;
                    case Direction.Right:
                        degrees = NormalizeOrientation(degrees + instruction.By);
                        break;
                    case Direction.Forward:
                        var currentDirection = degrees switch
                        {
                            0 => (1, 0),
                            90 => (0, -1),
                            180 => (-1, 0),
                            270 => (0, 1)
                        };
                        x += currentDirection.Item1 * instruction.By;
                        y += currentDirection.Item2 * instruction.By;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }

        private int NormalizeOrientation(int deg)
        {
            var ret = deg % 360;
            if (ret < 0) ret += 360;
            return ret;
        }

        protected override string SolvePartTwo()
        {
            var shipX = 0;
            var shipY = 0;

            var wpX = 10;
            var wpY = 1;

            foreach (var i in _navigation)
            {
                switch (i.Direction)
                {
                    case Direction.North:
                        wpY += i.By;
                        break;
                    case Direction.South:
                        wpY -= i.By;
                        break;
                    case Direction.East:
                        wpX += i.By;
                        break;
                    case Direction.West:
                        wpX -= i.By;
                        break;
                    case Direction.Left:
                        (wpX, wpY) = RotateWaypoint(wpX, wpY, -i.By);
                        break;
                    case Direction.Right:
                        (wpX, wpY) = RotateWaypoint(wpX, wpY, i.By);
                        break;
                    case Direction.Forward:
                        shipX += wpX * i.By;
                        shipY += wpY * i.By;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return (Math.Abs(shipX) + Math.Abs(shipY)).ToString();
        }

        private (int, int) RotateWaypoint(int startX, int startY, int deg)
        {
            if (deg < 0) deg += 360;
            var howManyTimes = deg / 90;
            for (int i = 0; i < howManyTimes; i++)
            {
                (startX, startY) = Rotate90Deg(startX, startY);
            }
            return (startX, startY);
        }

        private (int, int) Rotate90Deg(int startX, int startY)
        {
            if (startX >= 0 && startY >= 0) return (startY, -startX);
            if (startX >= 0 && startY < 0) return (startY, -startX);
            if (startX < 0 && startY >= 0) return (startY, -startX);
            if (startX < 0 && startY < 0) return (startY, -startX);
            throw new Exception($"StartX: {startX}, StartY: {startY}");
        }

        private class Instruction
        {
            public Direction Direction { get; }
            public int By { get; }

            public Instruction(char direction, int amount)
            {
                Direction = (Direction)direction;
                By = amount;
            }
            
            public static Instruction FromLine(string line)
            {
                var d = line[0];
                var amount = int.Parse(line.Substring(1));
                return new Instruction(d, amount);
            }

            public override string ToString()
            {
                return $"{Direction} {By}";
            }
        }

        enum Direction
        {
            North = 'N',
            South = 'S',
            East = 'E',
            West = 'W',
            Left = 'L',
            Right = 'R',
            Forward = 'F'
        }
    
    }
}
