using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day03 : ASolution
    {
        private List<string> _terrain;
        public Day03() : base(03, 2020, "")
        {
            _terrain = Input.SplitByNewline().ToList();
        }

        protected override string SolvePartOne()
        {
            // How many trees (#) I'll encounter in a 3 right, 1 down movement. The lines are repeating.
            // I'll use mod with the length of the line
            var encountered = TreesEncountered();

            return encountered.ToString();
        }

        private int TreesEncountered(int right = 3, int down = 1)
        {
            int encountered = 0;
            int index = 0;
            for (var i = 0; i < _terrain.Count; i += down)
            {
                var line = _terrain[i];
                if (line[index % line.Length] == '#') encountered++;
                index += right;
            }
            return encountered;
        }

        protected override string SolvePartTwo()
        {
            return (TreesEncountered(1,1) 
                    * TreesEncountered(3,1) 
                    * TreesEncountered(5, 1) 
                    * TreesEncountered(7,1) 
                    * TreesEncountered(1,2))
                .ToString();
        }
    }
}
