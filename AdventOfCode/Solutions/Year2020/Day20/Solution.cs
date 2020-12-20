using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day20 : ASolution
    {
        private Dictionary<int, Tile> _tiles;

        public Day20() : base(20, 2020, "")
        {
//             DebugInput = @"Tile 2311:
// ..##.#..#.
// ##..#.....
// #...##..#.
// ####.#...#
// ##.##.###.
// ##...#.###
// .#.#.#..##
// ..#....#..
// ###...#.#.
// ..###..###
//
// Tile 1951:
// #.##...##.
// #.####...#
// .....#..##
// #...######
// .##.#....#
// .###.#####
// ###.##.##.
// .###....#.
// ..#.#..#.#
// #...##.#..
//
// Tile 1171:
// ####...##.
// #..##.#..#
// ##.#..#.#.
// .###.####.
// ..###.####
// .##....##.
// .#...####.
// #.##.####.
// ####..#...
// .....##...
//
// Tile 1427:
// ###.##.#..
// .#..#.##..
// .#.##.#..#
// #.#.#.##.#
// ....#...##
// ...##..##.
// ...#.#####
// .#.####.#.
// ..#..###.#
// ..##.#..#.
//
// Tile 1489:
// ##.#.#....
// ..##...#..
// .##..##...
// ..#...#...
// #####...#.
// #..#.#.#.#
// ...#.#.#..
// ##.#...##.
// ..##.##.##
// ###.##.#..
//
// Tile 2473:
// #....####.
// #..#.##...
// #.##..#...
// ######.#.#
// .#...#.#.#
// .#########
// .###.#..#.
// ########.#
// ##...##.#.
// ..###.#.#.
//
// Tile 2971:
// ..#.#....#
// #...###...
// #.#.###...
// ##.##..#..
// .#####..##
// .#..####.#
// #..#.#..#.
// ..####.###
// ..#.#.###.
// ...#.#.#.#
//
// Tile 2729:
// ...#.#.#.#
// ####.#....
// ..#.#.....
// ....#..#.#
// .##..##.#.
// .#.####...
// ####.#.#..
// ##.####...
// ##..#.##..
// #.##...##.
//
// Tile 3079:
// #.#.#####.
// .#..######
// ..#.......
// ######....
// ####.#..#.
// .#...#.##.
// #.#####.##
// ..#.###...
// ..#.......
// ..#.###...";

            _tiles = new Dictionary<int, Tile>();
            var tiles = Input.Split("\n\n");
            foreach (var tile in tiles)
            {
                var parsedTile = new Tile(tile.SplitByNewline());
                _tiles.Add(parsedTile.Id, parsedTile);
            }
        }


        protected override string SolvePartOne()
        {
            // corner puzzle pieces only have 2 edges matched, the other two unmatched.
            // Pass 1: populate the edge map
            var edgeCounts = new Dictionary<string, int>();

            foreach (var tile in _tiles.Values)
            {
                var edges = tile.GetEdges();
                AddEdgeToDict(edgeCounts, edges.North);
                AddEdgeToDict(edgeCounts, edges.East);
                AddEdgeToDict(edgeCounts, edges.South);
                AddEdgeToDict(edgeCounts, edges.West);
            }

            var cornerProduct = 1L;
            foreach (var tile in _tiles)
            {
                var edges = tile.Value.GetEdges();
                var count = GetEdgeCountFromDict(edgeCounts, edges.North) 
                            + GetEdgeCountFromDict(edgeCounts, edges.East) 
                            + GetEdgeCountFromDict(edgeCounts, edges.South) 
                            + GetEdgeCountFromDict(edgeCounts, edges.West);
                if (count == 6) // 4 occurences of its own edges, + 2 for the matching ones
                    cornerProduct *= tile.Key;
            }

            return cornerProduct.ToString();
            
            void AddEdgeToDict(Dictionary<string, int> edgeCounts, string edge)
            {
                var rev = edge.Reverse();
                if (edgeCounts.ContainsKey(edge))
                    edgeCounts[edge]++;
                else if (edgeCounts.ContainsKey(rev))
                    edgeCounts[rev]++;
                else edgeCounts[edge] = 1;
            }
            int GetEdgeCountFromDict(Dictionary<string, int> edgeCounts, string edge)
            {
                if (!edgeCounts.TryGetValue(edge, out var val)) edgeCounts.TryGetValue(edge.Reverse(), out val);
                return val;
            }
        }
        
        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}