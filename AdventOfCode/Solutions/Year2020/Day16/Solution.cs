using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    public class TicketRule
    {
        public string FieldName { get; set; }
        public List<Range> Ranges { get; set; }

        public bool IsInRange(int num)
        {
            return Ranges.Any(r => r.ContainsValue(num));
        } 
        
        public static TicketRule FromLine(string line)
        {
            var split = line.Split(":");
            var ranges = split[1].Split("or").Select(i =>Range.FromString(i.Trim()));
            var tr = new TicketRule
            {
                FieldName = split[0].Trim(),
                Ranges = ranges.ToList()
            };
            return tr;
        }
    }

    public class Ticket
    {
        public int[] Data { get; set; }
        public bool IsValid { get; set; } = true; // mexri apodeixews enantiou
        public Ticket(string line)
        {
            Data = line.ToIntArray(delimiter:",");
        }
        
    }

    public class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public static Range FromString(string val)
        {
            var split = val.Split("-");
            return new Range(int.Parse(split[0]), int.Parse(split[1]));
        }
        
        public override string ToString()
        {
            return $"[{Min} - {Max}]";
        }

        public bool ContainsValue(int val)
        {
            return val >= Min && val <= Max;
        }
    }
    
    class Day16 : ASolution
    {
        private List<TicketRule> _rules;
        private Ticket _myTicket;
        private List<Ticket> _otherTickets; 
        public Day16() : base(16, 2020, "")
        {
//             DebugInput = @"class: 0-1 or 4-19
// row: 0-5 or 8-19
// seat: 0-13 or 16-19
//
// your ticket:
// 11,12,13
//
// nearby tickets:
// 3,9,18
// 15,1,5
// 5,14,9";
            _rules = new List<TicketRule>();
            _otherTickets = new List<Ticket>();
            ParseInput(Input.SplitByNewline());
        }

        private void ParseInput(string[] input)
        {
            var mode = 0;
            foreach (var line in input)
            {
                if (line.Contains("your ticket"))
                {
                    mode = 1;
                    continue;
                }

                if (line.Contains("nearby tickets"))
                {
                    mode = 2;
                    continue;
                }
                
                switch (mode)
                {
                    case 0:
                        _rules.Add(TicketRule.FromLine(line));
                        break;
                    case 1:
                        _myTicket = new Ticket(line);
                        break;
                    case 2:
                        _otherTickets.Add(new Ticket(line));
                        break;
                }
            }
        }

        protected override string SolvePartOne()
        {
            var allValidNums = new HashSet<int>();
            foreach (var rule in _rules)
            {
                foreach (var ruleRange in rule.Ranges)
                {
                    for (int i = ruleRange.Min; i <= ruleRange.Max; i++)
                    {
                        allValidNums.Add(i);
                    }
                }
            }

            long sum = 0;
            foreach (var ticket in _otherTickets)
            {
                foreach (var num in ticket.Data)
                {
                    if (!allValidNums.Contains(num))
                    {
                        sum += num;
                        ticket.IsValid = false;
                    }
                }
            }
            return sum.ToString();
        }

        protected override string SolvePartTwo()
        {
            var validTickets = _otherTickets.Where(t => t.IsValid).ToList();

            var ruleMatches = PopulatePossibleRuleMatches(validTickets);
            // I found, per rule, which fields are valid. I'll try to eliminate the conflicts by sieving.
            CleanupRuleFieldMatchings(ruleMatches);

            var product = 1L;
            for (var index = 0; index < _rules.Count; index++)
            {
                var rule = _rules[index];
                if (rule.FieldName.StartsWith("departure"))
                {
                    var indexOfData = ruleMatches[index].First();
                    product *= _myTicket.Data[indexOfData];
                }
            }

            return product.ToString();
            return null;
        }

        private Dictionary<int, List<int>> PopulatePossibleRuleMatches(List<Ticket> validTickets)
        {
            var ruleMatches = new Dictionary<int, List<int>>();
            
            var countOfFields = validTickets.ElementAt(0).Data.Length;
            for (var index = 0; index < _rules.Count; index++)
            {
                var rule = _rules[index];
                var matches = new List<int>();
                for (int i = 0; i < countOfFields; i++)
                {
                    var match = true;
                    if (!rule.IsInRange(_myTicket.Data[i]))
                    {
                        match = false;
                        continue;
                    }

                    foreach (var ticket in validTickets)
                    {
                        if (!rule.IsInRange(ticket.Data[i]))
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        matches.Add(i);
                    }
                }

                ruleMatches.Add(index, matches);
            }

            return ruleMatches;
        }

        private static void CleanupRuleFieldMatchings(Dictionary<int, List<int>> ruleMatches)
        {
            do
            {
                var orderedRulesPerNumOfMatches = ruleMatches.OrderBy(r => r.Value.Count);
                foreach (var pair in orderedRulesPerNumOfMatches)
                {
                    if (ruleMatches[pair.Key].Count == 1)
                    {
                        var indexToRemoveFromOthers = ruleMatches[pair.Key].First();
                        for (int i = 0; i < ruleMatches.Count; i++)
                        {
                            var rm = ruleMatches.ElementAt(i);
                            if (rm.Key == pair.Key) continue;
                            ruleMatches[rm.Key] = ruleMatches[rm.Key]
                                .Where(i => i != indexToRemoveFromOthers).ToList();
                        }
                    }
                }
            } while (ruleMatches.Any(rm => rm.Value.Count > 1));
        }
    }
}
