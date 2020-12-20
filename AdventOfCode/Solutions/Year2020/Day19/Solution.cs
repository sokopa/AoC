using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{

    class Day19 : ASolution
    {
        private Dictionary<int,string> _rules;
        private string[] _messages;
        public Day19() : base(19, 2020, "")
        {
//             DebugInput = @"0: 4 1 5
// 1: 2 3 | 3 2
// 2: 4 4 | 5 5
// 3: 4 5 | 5 4
// 4: 'a'
// 5: 'b'
//
// ababbb
// bababa
// abbbab
// aaabbb
// aaaabbb";
            
            var lines = Input.SplitByNewline().ToList();
            _rules = lines.Where((l, i) => char.IsDigit(l[0]))
                .Select(s => (s.Split(": ")[0], s.Split(": ")[1]))
                .ToDictionary(a => int.Parse(a.Item1), a => a.Item2);
            _messages = lines.Where((l, i) => !char.IsDigit(l[0])).ToArray();
        }

        protected override string SolvePartOne()
        {
            var regexStr = CreateRegexFromRules();
            var regex = new Regex(regexStr);
            var count = _messages.Count(message => regex.IsMatch(message));
            return count.ToString();
        }

        protected override string SolvePartTwo()
        {
            _memoTable = new Dictionary<int, string>();
            var regexStr = CreateRegexFromRules(isPart2: true);
            var regex = new Regex(regexStr, RegexOptions.Compiled);
            var count = _messages.Count(message => regex.IsMatch(message));
            return count.ToString();
        }
        
        private string CreateRegexFromRules(bool isPart2 = false)
        {
            var regexForAll = $"^{CreateRegexFromRule(0, isPart2)}$";
            return regexForAll;
        }

        
        private static Dictionary<int, string> _memoTable = new Dictionary<int, string>(); 
        private string CreateRegexFromRule(int rule, bool isPart2 = false)
        {
            if (_memoTable.ContainsKey(rule)) return _memoTable[rule];

            var rules = _rules[rule];
            if (rules.Contains("\"")) 
            {
                var updatedRule = rules.Replace("\"", ""); // "a" -> a
                _memoTable[rule] = updatedRule;
                return updatedRule;
            }
            
            // just for debug
            if (rules.Contains("'")) 
            {
                var updatedRule = rules.Replace("'", ""); // 'a' -> a
                _memoTable[rule] = updatedRule;
                return updatedRule;
            }

            
            if (isPart2 && rule == 8)
            {
                //  8: 42 | 42 8 -> 42 or 42 42 or 42 42 42 ... -> 42+
                var str = $"({CreateRegexFromRule(42, isPart2)}+)";
                _memoTable[rule] = str;
                return str;
            }
            
            if (isPart2 && rule == 11)
            {
                // .NET Regex Balancing Group for a^n b^n
                //  11: 42 31 | 42 11 31 -> 42 31 or 42 (42 31) 31 --> (?'left'42)+(?'right-left'31)+(?(42)(?!))
                var str = $"(?'left'{CreateRegexFromRule(42, isPart2)})+(?'right-left'{CreateRegexFromRule(31, isPart2)})+(?({CreateRegexFromRule(42, isPart2)})(?!))";
                _memoTable[rule] = str;
                return str;
            }
            
            var sb = new StringBuilder("(");
            foreach (var rulePart in rules.Split(" "))
            {
                if (char.IsDigit(rulePart[0]))
                {
                    sb.Append(CreateRegexFromRule(int.Parse(rulePart), isPart2));
                }
                else if (rulePart == "|")
                {
                    sb.Append("|");
                }
            }

            sb.Append(")");
            _memoTable[rule] = sb.ToString();
            return sb.ToString();
        }
    }
}
