using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{

    class Day04 : ASolution
    {
        private List<Dictionary<string, string>> _passports;
        public Day04() : base(04, 2020, "")
        {
            var dict = new Dictionary<string, string>();
            _passports = new List<Dictionary<string, string>>();
            foreach (var line in Input.SplitByNewline(shouldPreserveEmptyLines:true))
            {
                if (line == String.Empty)
                {
                    _passports.Add(dict);
                    dict = new Dictionary<string, string>();
                }
                else
                {
                    AddPassportDataFromInput(dict, line);
                }
            }
            _passports.Add(dict);
        }

        private void AddPassportDataFromInput(Dictionary<string, string> passportData, string line)
        {
            var split = line.Split(" ");
            foreach (var kvp in split)
            {
                var splitByColon = kvp.Split(":");
                passportData.Add(splitByColon[0], splitByColon[1]);
            }
        }

        private readonly string[] mandatoryFields = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"}; 
        
        protected override string SolvePartOne()
        {
            Func<Dictionary<string, string>, bool> isValid = x =>
            {
                var intersection = x.Keys.ToList().Intersect(mandatoryFields).ToList();
                return intersection.Count == mandatoryFields.Length;
            };
            var count = _passports.Count(isValid);
            return count.ToString();
        }

        protected override string SolvePartTwo()
        {
            Func<Dictionary<string, string>, bool> isValid = x =>
            {
                if (!x.ContainsKey("byr")) return false;
                var byr = int.Parse(x["byr"]);
                if (byr < 1920 || byr > 2002) return false;

                if (!x.ContainsKey("iyr")) return false;
                var iyr = int.Parse(x["iyr"]);
                if (iyr < 2010 || iyr > 2020) return false;

                if (!x.ContainsKey("eyr")) return false;
                var eyr = int.Parse(x["eyr"]);
                if (eyr < 2020 || eyr > 2030) return false;

                if (!x.ContainsKey("hgt")) return false;
                var hgt = x["hgt"];
                var hgtNum = int.Parse(hgt.Substring(0, hgt.Length - 2));
                var hgtUnit = hgt.Substring(hgt.Length - 2, 2);
                if (hgtUnit != "cm" && hgtUnit != "in") return false;
                switch (hgtUnit)
                {
                    case "cm" when (hgtNum < 150 || hgtNum > 193):
                    case "in" when (hgtNum < 59 || hgtNum > 76):
                        return false;
                }

                if (!x.ContainsKey("hcl")) return false;
                var hcl = x["hcl"];
                if (!Regex.IsMatch(hcl, @"^#[a-fA-F0-9]{6}$")) return false;

                if (!x.ContainsKey("ecl")) return false;
                var ecl = x["ecl"];
                if (!Regex.IsMatch(ecl, @"^(amb|blu|brn|gry|grn|hzl|oth)$")) return false;

                if (!x.ContainsKey("pid")) return false;
                var pid = x["pid"];
                if (!Regex.IsMatch(pid, @"^[0-9]{9}$")) return false;

                return true;
            };
            var count = _passports.Count(isValid);
            return count.ToString();
        }
    }
}
