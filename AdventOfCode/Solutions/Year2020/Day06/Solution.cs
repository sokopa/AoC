using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day06 : ASolution
    {
        private List<HashSet<string>> _groups;
        private Dictionary<int, List<List<string>>> _personsPerGroup;
        public Day06() : base(06, 2020, "")
        {
            var set = new HashSet<string>();
            var persons = new List<List<string>>();
            _groups = new List<HashSet<string>>();
            _personsPerGroup = new Dictionary<int,List<List<string>>>();
            int group = 1;
            foreach (var line in Input.SplitByNewline(shouldPreserveEmptyLines: true))
            {
                if (line == String.Empty)
                {
                    _groups.Add(set);
                    _personsPerGroup.Add(group, persons);
                    group++;
                    set = new HashSet<string>();
                    persons = new List<List<string>>();
                }
                else
                {
                    ParseGroupQuestions(set, out var personAnswers, line);
                    persons.Add(personAnswers);
                }
            }

            _groups.Add(set);
            _personsPerGroup.Add(group, persons);
        }

        private void ParseGroupQuestions(HashSet<string> set, out List<string> personAnswers, string line)
        {
            personAnswers = line.ToList().Select(i => i.ToString()).ToList();
            personAnswers.ForEach(i => set.Add(i));
        }

        protected override string SolvePartOne()
        {
            return _groups.Select(g => g.Count).Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            var count = 0;
            foreach (var ppg in _personsPerGroup)
            {
                var groupId = ppg;
                var set = new HashSet<string>(ppg.Value[0]);
                foreach (var personAnswers in ppg.Value)
                {
                    set = personAnswers.Intersect(set).ToHashSet();
                }

                count += set.Count;
            }
            return count.ToString();
        }
    }
}
