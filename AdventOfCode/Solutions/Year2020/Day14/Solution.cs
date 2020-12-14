using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day14 : ASolution
    {
        private VM vm;

        public Day14() : base(14, 2020, "")
        {
            
            var lines = Input.SplitByNewline();
            vm = new VM(lines);
        }

        protected override string SolvePartOne()
        {
            vm.RunInitCode();
            var result = vm.GetMemorySum();
            return result.ToString();
        }

        protected override string SolvePartTwo()
        {
            vm.RunInitV2();
            var result = vm.GetMemorySum();
            return result.ToString();
        }

        private class VM
        {
            private Dictionary<long, long> _memory = new Dictionary<long, long>();
            private IEnumerable<string> _originalCode = new List<string>();
            private Regex rgx = new Regex(@"mem\[(\d+)\] = (\d+)", RegexOptions.Compiled);

            public VM(IEnumerable<string> code)
            {
                _originalCode = code;
            }

            public void RunInitCode()
            {
                var mask = string.Empty;
                foreach (var instruction in _originalCode)
                    if (instruction.StartsWith("mask"))
                    {
                        // update the mask
                        mask = instruction.Replace("mask = ", "");
                    }
                    else
                    {
                        var match = rgx.Match(instruction);
                        var memIndex = long.Parse(match.Groups[1].Value);
                        var value = long.Parse(match.Groups[2].Value);
                        var withMask = ApplyMask(value, mask);
                        _memory[memIndex] = withMask;
                    }
            }

            public void RunInitV2()
            {
                _memory = new Dictionary<long, long>();
                var mask = string.Empty;
                foreach (var instruction in _originalCode)
                    if (instruction.StartsWith("mask"))
                    {
                        // update the mask
                        mask = instruction.Replace("mask = ", "");
                    }
                    else
                    {
                        var match = rgx.Match(instruction);
                        var memIndex = long.Parse(match.Groups[1].Value);
                        var value = long.Parse(match.Groups[2].Value);
                        var withMask = ApplyMaskV2(memIndex, mask);
                        foreach (var m in withMask)
                        {
                            _memory[m] = value;
                        }
                    }
            }

            public long GetMemorySum()
            {
                return _memory.Values.Sum();
            }

            private static long ApplyMask(long num, string mask)
            {
                var binaryNum = Convert.ToString(num, 2).PadLeft(36, '0');
                var chars = binaryNum.Zip(mask, (original, m) =>
                    m switch
                    {
                        '1' => '1',
                        '0' => '0',
                        _ => original
                    }
                ).ToList();
                var result = string.Concat(chars);
                var converted = Convert.ToInt64(result, 2);
                return converted;
            }

            private static IEnumerable<long> ApplyMaskV2(long value, string mask)
            {
                var binaryNum = Convert.ToString(value, 2).PadLeft(36, '0');
                var chars = binaryNum.Zip(mask, (original, m) =>
                    m switch
                    {
                        '0' => original,
                        '1' => '1',
                        _ => 'X'
                    }
                ).ToList();
                var possibleNumbers = new List<string>();
                if (!chars.Contains('X')) possibleNumbers.Add(string.Concat(chars));
                var howManyXs = chars.Count(c => c == 'X');
                var howManyIWillAdd = (int)Math.Pow(2, howManyXs);
                var permutations = Enumerable.Range(0, howManyIWillAdd)
                    .Select(i => Convert.ToString(i,2).PadLeft(howManyXs, '0'));
                possibleNumbers.AddRange(ReplaceXs(string.Concat(chars), permutations));
                return possibleNumbers.Select(s =>
                {
                    var converted = Convert.ToInt64(s, 2);
                    return converted;
                });
            }

            private static IEnumerable<string> ReplaceXs(string mask, IEnumerable<string> permutations)
            {
                foreach (var permutation in permutations)
                {
                    yield return ReplaceXs(mask, permutation);
                }
            }

            private static string ReplaceXs(string mask, string value)
            {
                var maskModifiable = mask.ToCharArray();
                foreach (var t in value)
                {
                    maskModifiable[Array.IndexOf(maskModifiable, 'X')] = t;
                }
                return string.Concat(maskModifiable);
            }
        }
    }
}