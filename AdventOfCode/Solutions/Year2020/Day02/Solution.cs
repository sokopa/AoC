using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class PasswordCheck
    {
        public int MinOccurences { get; set; }
        public int MaxOccurences { get; set; }
        public string Character { get; set; }
        public string Password { get; set; }
    }
    
    class Day02 : ASolution
    {
        private List<PasswordCheck> inputs;
        public Day02() : base(02, 2020, "")
        {
            inputs = new List<PasswordCheck>();
            foreach (var line in Input.SplitByNewline())
            {
                var pc = ParseLine(line);
                inputs.Add(pc);
            }
        }

        private PasswordCheck ParseLine(string line)
        {
            var splitByColon = line.Trim().Split(':');
            var pw = splitByColon[1].Trim();
            var characterToCheck = splitByColon[0][^1];
            var range = splitByColon[0].Substring(0, splitByColon[0].Length - 1).Split('-');
            var min = int.Parse(range[0]);
            var max = int.Parse(range[1]);
            return new PasswordCheck
            {
                MinOccurences = min,
                MaxOccurences = max,
                Character = characterToCheck.ToString(),
                Password = pw
            };
        }

        protected override string SolvePartOne()
        {
            var validPasswords = 0;
            foreach (var passwordCheck in inputs)
            {
                var occurences = passwordCheck.Password.Count(c => c.ToString() == passwordCheck.Character);
                if (occurences <= passwordCheck.MaxOccurences && occurences >= passwordCheck.MinOccurences)
                    validPasswords++;
            }

            return validPasswords.ToString();
        }

        protected override string SolvePartTwo()
        {
            var validPasswords = 0;
            foreach (var passwordCheck in inputs)
            {
                var charAtFirstPlace = passwordCheck.Password[passwordCheck.MinOccurences - 1].ToString() == passwordCheck.Character;
                var charAtLastPlace = passwordCheck.Password[passwordCheck.MaxOccurences - 1].ToString() == passwordCheck.Character;
                if (charAtFirstPlace ^ charAtLastPlace) validPasswords++;
            }

            return validPasswords.ToString();
        }
    }
}
