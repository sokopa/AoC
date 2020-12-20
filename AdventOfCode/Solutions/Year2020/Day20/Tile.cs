using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    public class Tile
    {
        public Tile(string[] data)
        {
            var tile = data.First().Split(" ")[1];
            var tileId = tile.Substring(0, tile.Length - 1);

            Id = int.Parse(tileId);
            Content = data.Skip(1).ToArray();
            Rotation = 0;
            FlipDirection = FlipDirection.None;
        }
        
        public int Id { get; private set; }
        public string[] Content { get; private set; }

        public int Width => Content[0].Length;
        public int Height => Content.Length;

        public int Rotation { get; set; } // 0: as is. 1: 90 deg CCW, 2: 180 deg CCW, 3: 270 deg CCW.
        public FlipDirection FlipDirection {get; set;}

        public Dictionary<Direction, Tile> Neighbors = new Dictionary<Direction, Tile>();

        public (string North, string East, string South, string West) GetEdges() => GetEdgesWithFlipAndRotation();
        
        private (string North, string East, string South, string West) GetEdgesWithFlipAndRotation()
        {
            var edges = GetEdgesWithRotation();
            return (this.FlipDirection) switch
            {
                FlipDirection.None => (edges.North, edges.East, edges.South, edges.West),
                FlipDirection.Horizontal => // the axis is horizontal. west/east are reversed, north/south are switched
                    (edges.South, edges.East.Reverse(), edges.North, edges.West.Reverse()),
                FlipDirection.Vertical => // the axis is vertical. west/east are switched, n/s reversed
                    (edges.North.Reverse(), edges.West, edges.South.Reverse(), edges.East),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private (string North, string East, string South, string West) GetEdgesWithRotation()
        {
            var edges = GetEdgesWithoutRotation();

            return (Rotation % 4) switch
            {
                0 => (edges.North, edges.East, edges.South, edges.West),
                1 => (edges.East, edges.South.Reverse(), edges.West, edges.North.Reverse()),
                2 => (edges.South.Reverse(), edges.West.Reverse(), edges.North.Reverse(), edges.East.Reverse()),
                3 => (edges.West.Reverse(), edges.North, edges.East.Reverse(), edges.South),
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
        
    }

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public enum FlipDirection
    {
        None,
        Horizontal,
        Vertical
    }
}