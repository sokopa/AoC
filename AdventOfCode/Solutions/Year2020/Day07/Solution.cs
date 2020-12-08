using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{
    
    class Day07 : ASolution
    {
        private Dictionary<string, Bag> _bags;
        public Day07() : base(07, 2020, "")
        {
            _bags = ParseBags(Input);
        }

        protected override string SolvePartOne()
        {
            var targetBag = _bags["shiny gold"];
            var parents = targetBag.ParentColors.ToHashSet().Count;
            return parents.ToString();
        }

        protected override string SolvePartTwo()
        {
            var shinyGold = _bags["shiny gold"];
            return shinyGold.Count.ToString();
        }

        private Dictionary<string, Bag> ParseBags(string input)
        {
            var bags = new Dictionary<string, Bag>();
            var rules = input.SplitByNewline().Select(ParseRule);
            foreach (var rule in rules)
            {
                var bag = bags.GetValueOrDefault(rule.BagType) ?? new Bag {BagType = rule.BagType};
                if (!bags.ContainsKey(bag.BagType))
                {
                    bags[bag.BagType] = bag;
                }

                foreach (var innerRule in rule.InnerRules)
                {
                    var otherBag = bags.GetValueOrDefault(innerRule.BagType) ?? new Bag {BagType = innerRule.BagType};
                    if (!bags.ContainsKey(otherBag.BagType))
                    {
                        bags[otherBag.BagType] = otherBag;
                    }
                    otherBag.Parents.Add(bag);
                    bag.Children.Add((otherBag, innerRule.Quantity));
                }
            }

            return bags;
        }
        
        private Rule ParseRule(string line)
        {
            var definitionParts =  line.Split("bags contain");
            var rule = new Rule
            {
                BagType = definitionParts[0].Trim(),
                InnerRules = new List<InnerRule>()
            };
            var contains = definitionParts[1].Split(",");
            var regex = new Regex(@"(\d+)\s(\w+\s\w+)\sbags?", RegexOptions.Compiled);
            foreach (var part in contains)
            {
                var m = regex.Match(part);
                if (m.Success)
                {
                    var innerRule = new InnerRule
                    {
                        Quantity = int.Parse(m.Groups[1].Value),
                        BagType = m.Groups[2].Value
                    };
                    rule.InnerRules.Add(innerRule);
                }
                
            }

            return rule;
        }

        private class Bag
        {
            public string BagType { get; set; }
            public List<(Bag b, int q)> Children = new List<(Bag b, int q)>();
            public List<Bag> Parents = new List<Bag>();

            public List<string> ParentColors => Parents.SelectMany(p => p.ParentColors.Concat(new[] { p.BagType })).ToList();
            public long Count => Children.Sum(c => c.q) + Children.Sum(c => c.b.Count * c.q);
        }
        
        private class Rule
        {
            public string BagType { get; set; }
            public List<InnerRule> InnerRules { get; set; }
        }

        private class InnerRule
        {
            public int Quantity { get; set; }
            public string BagType { get; set; }
        }
    }
}
