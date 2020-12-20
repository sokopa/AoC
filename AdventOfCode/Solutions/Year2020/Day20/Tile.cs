using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    public class Tile
    {
        public static Tile FromInput(string[] data)
        {
            var tile = data.First().Split(" ")[1];
            var tileId = tile.Substring(0, tile.Length - 1);

            return new Tile(data.Skip(1).ToArray(), int.Parse(tileId));
        }

        public Tile(string[] image, int id = 0)
        {
            Id = id;
            Content = image;
            Grid = Content.Select(s => s.ToArray()).ToArray();
            Rotation = Rotation.None;
            Flipped = false;
        }

        public Tile()
        {
        }

        public int Id { get; private set; }
        public string[] Content { get; private set; }
        public char[][] Grid { get; set; }

        // it suffices, as it's symmetrical
        public int Size => Content.Length;
        
        public bool IsCorner { get; set; } = false;
        public bool IsEdge { get; set; } = false;

        public Rotation Rotation { get; set; } = Rotation.None;
        public bool Flipped { get; set; } = false;

        public (string North, string East, string South, string West) GetEdges()
        {
            return (Row(0), Col(Size - 1), Row(Size - 1), Col(0));
        }
        
        public HashSet<string> AllPossibleBaseEdges()
        {
            var set = new HashSet<string>();
            var edges = GetEdges();
            set.Add(edges.North);
            set.Add(edges.South);
            set.Add(edges.East);
            set.Add(edges.West);
            return set;
        }
        
        public HashSet<string> AllPossibleEdges()
        {
            var set = new HashSet<string>();
            var edges = GetEdges();
            set.Add(edges.North);
            set.Add(edges.South);
            set.Add(edges.East);
            set.Add(edges.West);
            set.Add(edges.North.Reverse());
            set.Add(edges.South.Reverse());
            set.Add(edges.East.Reverse());
            set.Add(edges.West.Reverse());
            return set;
        }

        private (string North, string East, string South, string West) GetEdgesWithFlipAndRotation()
        {
            var edges = GetEdgesWithRotation();
            return edges;
            // return Flipped switch
            // {
            //     false => (edges.North, edges.East, edges.South, edges.West),
            //     true => // the axis is vertical. west/east are switched, n/s reversed
            //         (edges.North.Reverse(), edges.West, edges.South.Reverse(), edges.East)
            // };
        }

        private (string North, string East, string South, string West) GetEdgesWithRotation()
        {
            var edges = GetEdgesWithoutRotation();

            return (Rotation) switch
            {
                Rotation.None => (edges.North, edges.East, edges.South, edges.West),
                Rotation.By90 => (edges.East, edges.South.Reverse(), edges.West, edges.North.Reverse()),
                Rotation.By180 => (edges.South.Reverse(), edges.West.Reverse(), edges.North.Reverse(), edges.East.Reverse()),
                Rotation.By270 => (edges.West.Reverse(), edges.North, edges.East.Reverse(), edges.South),
                _ => throw new ArgumentException(nameof(Rotation))
            };
        }

        private (string North, string East, string South, string West) GetEdgesWithoutRotation()
        {
            var north = Content[0];
            var south = Content[^1];
            var west = string.Concat(Content.Select(s => s[0]));
            var east = string.Concat(Content.Select(s => s[^1]));
            return (north, east, south, west);
        }

        private string Line(int row, int col, int directionRow, int directionCol)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                sb.Append(this[row, col]);
                row += directionRow;
                col += directionCol;
            }
            return sb.ToString();
        }

        public char this[int row, int col]
        {
            get
            {
                // rotate
                if (Rotation == Rotation.By90)
                {
                    (row, col) = Rotate90(row, col);
                }
                if (Rotation == Rotation.By180)
                {
                    (row, col) = Rotate90(row, col);
                    (row, col) = Rotate90(row, col);
                }
                if (Rotation == Rotation.By270)
                {
                    (row, col) = Rotate90(row, col);
                    (row, col) = Rotate90(row, col);
                    (row, col) = Rotate90(row, col);
                }
                
                if (Flipped)
                {
                    col = Size - 1 - col;
                }
                return Content[row][col];
            }
        }

        private (int row, int col) Rotate90(int row, int col)
        {
            return (col, Size - 1 - row);
        }
        
        public string Row(int row, bool borders = true)
        {
            var rowContent = Line(row, 0, 0, 1);
            if (borders) return rowContent;
            else return rowContent.Substring(1, rowContent.Length - 2);
        }
        
        public string Col(int col, bool borders = true)
        {
            var colContent = Line(0, col, 1, 0);
            if (borders) return colContent;
            else return colContent.Substring(1, colContent.Length - 2);
        }
        

    }

    public enum Rotation
    {
        None,
        By90,
        By180,
        By270
    }

    public class IterationHelper
    {
        public static Rotation[] AllRotations = new[] {Rotation.None, Rotation.By90, Rotation.By180, Rotation.By270};

        public static bool[] AllFlips = new[] {false, true};
    }


    public enum FlipDirection
    {
        None,
        Horizontal,
        Vertical
    }
}