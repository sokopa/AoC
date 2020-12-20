using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    public class Day20 : ASolution
    {
        private Dictionary<int, Tile> _tiles;

        private int picSize;

        public Day20() : base(20, 2020, "")
        {
            _tiles = new Dictionary<int, Tile>();
            var tiles = Input.Split("\n\n\n\n");
            foreach (var tile in tiles)
            {
                var parsedTile = Tile.FromInput(tile.SplitByNewline());
                _tiles.Add(parsedTile.Id, parsedTile);
            }

            picSize = (int) Math.Sqrt(tiles.Length);
            
        }

        public Day20(string input) : base(20, 2020, "")
        {
            _tiles = new Dictionary<int, Tile>();
            var tiles = input.Split("\n\n");
            foreach (var tile in tiles)
            {
                var parsedTile = Tile.FromInput(tile.SplitByNewline());
                _tiles.Add(parsedTile.Id, parsedTile);
            }

            picSize = (int) Math.Sqrt(tiles.Length);
        }

        private Dictionary<string, int> _edgeCounts;
        protected override string SolvePartOne()
        {
            // corner puzzle pieces only have 2 edges matched, the other two unmatched.
            // Pass 1: populate the edge map
            _edgeCounts = new Dictionary<string, int>();

            foreach (var tile in _tiles.Values)
            {
                var edges = tile.AllPossibleBaseEdges();
                edges.ToList().ForEach(e => AddEdgeToDict(_edgeCounts, e));
            }

            var cornerProduct = 1L;
            foreach (var tile in _tiles)
            {
                var edges = tile.Value.GetEdges();
                var count = GetEdgeCountFromDict(_edgeCounts, edges.North) 
                            + GetEdgeCountFromDict(_edgeCounts, edges.East) 
                            + GetEdgeCountFromDict(_edgeCounts, edges.South) 
                            + GetEdgeCountFromDict(_edgeCounts, edges.West);
                if (count == 6 || count == 12) // 4 occurrences of its own edges, + 2 for the matching ones
                {
                    cornerProduct *= tile.Key;
                    tile.Value.IsCorner = true;
                    // tile.Value.IsEdge = true;
                }
                if (count == 7 || count == 14) // 4 occurrences of its own edges, + 3 for the matching ones
                {
                    tile.Value.IsEdge = true;
                }
            }

            Console.WriteLine(cornerProduct);

            return cornerProduct.ToString();
            
            void AddEdgeToDict(Dictionary<string, int> ec, string edge)
            {
                var rev = edge.Reverse();
                if (ec.ContainsKey(edge))
                    ec[edge]++;
                else if (ec.ContainsKey(rev))
                    ec[rev]++;
                else ec[edge] = 1;
            }
            int GetEdgeCountFromDict(Dictionary<string, int> ec, string edge)
            {
                if (!ec.TryGetValue(edge, out var val)) ec.TryGetValue(edge.Reverse(), out val);
                return val;
            }
        }
        
        protected override string SolvePartTwo()
        {
            // // find a corner, try to place it in one corner, by taking into account its edges
            // (var startX, var startY) = FindStartingTile();
            // var processed = new HashSet<int>();
            // processed.Add(finalPositions[startX, startY].Id);

            var finalPositions = GetFinalPositions();

            // merge the picture without the borders, take into account the orientation/flipping (oh god)
            var completePicture = new List<string>();
            for (int row = 0; row < picSize; row++)
            {
                // tileSize is 10
                for (int tileRow = 1; tileRow < 9; tileRow++)
                {
                    var sb = new StringBuilder();
                    for (int col = 0; col < picSize; col++)
                    {
                        var tile = finalPositions[row, col];
                        sb.Append(tile.Row(tileRow, borders:false));
                    }
                    completePicture.Add(sb.ToString());    
                }
            }

            // it's correct now, although flipped. but correct. no need to print and check anymore
            // foreach (var line in completePicture)
            // {
            //     Console.WriteLine(line);
            // }

            var bigPic = new Tile(completePicture.ToArray());

            var monsterHigh = 0;
            foreach (var d in IterationHelper.AllRotations)
            {
                foreach (var f in IterationHelper.AllFlips)
                {
                    var monsterCount = HasMonster(bigPic, d, f);
                    monsterHigh = monsterCount > monsterHigh ? monsterCount : monsterHigh;
                }
            }

            var allHashes = CountHashes(bigPic);
            var answer = allHashes - monsterHigh * 15;
            
            return answer.ToString();
        }

        private int CountHashes(Tile tile)
        {
            int count = 0;
            for (int row = 0; row < tile.Size; row++)
            for (int col = 0; col < tile.Size; col++)
                count += tile[row, col] == '#' ? 1 : 0;
            return count;
        }
        
        private int HasMonster(Tile tile, Rotation r, bool flipped)
        {
            var newTile = new Tile(tile.Content);
            newTile.Rotation = r;
            newTile.Flipped = flipped;
            int countMonsters = 0;
            
            // check if monster exists.
            //           1111111111
            // 01234567890123456789              
            //                   #  |0
            // #    ##    ##    ### |1
            //  #  #  #  #  #  #    |2
            
            // look through the tile for the monster pattern
            for (int row = 0; row < tile.Size - 2; row++)
            {
                for (int col = 0; col < tile.Size - 19; col++)
                {
                    if (
                           newTile[row, col + 18] == '#'
                        && newTile[row + 1, col] == '#'
                        && newTile[row + 1, col + 5] == '#'
                        && newTile[row + 1, col + 6] == '#'
                        && newTile[row + 1, col + 11] == '#'
                        && newTile[row + 1, col + 12] == '#'
                        && newTile[row + 1, col + 17] == '#'
                        && newTile[row + 1, col + 18] == '#'
                        && newTile[row + 1, col + 19] == '#'
                        && newTile[row + 2, col + 1] == '#'
                        && newTile[row + 2, col + 4] == '#'
                        && newTile[row + 2, col + 7] == '#'
                        && newTile[row + 2, col + 10] == '#'
                        && newTile[row + 2, col + 13] == '#'
                        && newTile[row + 2, col + 16] == '#'
                    )
                    {
                        countMonsters++;
                    }
                }
            }
            
            return countMonsters;
        }

        public Tile[,] GetFinalPositions()
        {
            var finalPositions = new Tile[picSize, picSize];
            var remainingTiles = _tiles.Values.ToList();

            for (int row = 0; row < picSize; row++)
            {
                for (int col = 0; col < picSize; col++)
                {
                    var north = row == 0 ? null : finalPositions[row - 1, col].GetEdges().South;
                    var west = col == 0 ? null : finalPositions[row, col - 1].GetEdges().East;
                
                    var tile = SearchInRemaining(remainingTiles, north, west);
                    finalPositions[row, col] = tile;
                    remainingTiles = remainingTiles.Where(t => t.Id != tile.Id).ToList();
                }
            }

            return finalPositions;
        }

        private Tile SearchInRemaining(IEnumerable<Tile> remaining, string northEdge, string westEdge)
        {
            
            foreach (var tile in remaining)
            {
                foreach (var d in IterationHelper.AllRotations)
                {
                    foreach (var f in IterationHelper.AllFlips)
                    {
                        tile.Rotation = d;
                        tile.Flipped = f;
                        var edges = tile.GetEdges();
                        var northMatches = northEdge != null
                            ? northEdge == edges.North
                            : !remaining.Any(other =>
                                other.Id != tile.Id && other.AllPossibleEdges().Contains(edges.North));
                        var westMatches = westEdge != null
                            ? westEdge == edges.West
                            : !remaining.Any(other =>
                                other.Id != tile.Id && other.AllPossibleEdges().Contains(edges.West));
                        if (northMatches && westMatches) return tile;
                    }
                }
            }

            throw new Exception();
        }
    }
}