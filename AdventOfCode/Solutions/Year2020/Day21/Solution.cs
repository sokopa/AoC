using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    public class Food
    {
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }

        public static Food FromInput(string input)
        {
            var split = input.Split(" (");
            var ingredients = split[0].Split(" ");
            var allergens = split[1].Replace("contains ", "").Replace(")", "").Split(", ");
            return new Food
            {
                Ingredients = ingredients.ToList(),
                Allergens = allergens.ToList()
            };
        }

        public override string ToString()
        {
            return $"{string.Join(" ", Ingredients)} ({string.Join(", ", Allergens)})";
        }
    }
    
    class Day21 : ASolution
    {
        private List<Food> _foods;
        public Day21() : base(21, 2020, "")
        {
//             DebugInput = @"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
// trh fvjkl sbzzf mxmxvkd (contains dairy)
// sqjhc fvjkl (contains soy)
// sqjhc mxmxvkd sbzzf (contains fish)";
            _foods = Input.SplitByNewline().Select(Food.FromInput).ToList();
        }

        protected override string SolvePartOne()
        {
            //_foods.ForEach(Console.WriteLine);
            var foodsByAllergen = _foods.SelectMany(food => food.Allergens.Select(a => (a, food)))
                .ToLookup(a => a.a, a => a.food);
            
            // find common ingredients in each food the allergen occurs
            var danger = foodsByAllergen.ToDictionary(g => g.Key, g =>
                g.Aggregate(g.First().Ingredients, (acc, curr) => acc.Intersect(curr.Ingredients).ToList()));

            var dangerousIngredients = danger.SelectMany(r => r.Value).Distinct().ToHashSet();
            var allIngredients = _foods.SelectMany(f => f.Ingredients);
            var safe = allIngredients.Where(i => !dangerousIngredients.Contains(i));
            Console.WriteLine(safe.Count());
            return safe.Count().ToString();
        }

        protected override string SolvePartTwo()
        {
            var foodsByAllergen = _foods.SelectMany(food => food.Allergens.Select(a => (a, food)))
                .ToLookup(a => a.a, a => a.food);
            var danger = foodsByAllergen.ToDictionary(g => g.Key, g =>
                g.Aggregate(g.First().Ingredients, (acc, curr) => acc.Intersect(curr.Ingredients).ToList()));

         
            // Kinda like Day16
            do
            {
                var orderedAllergensByIngCount = danger.OrderBy(a => a.Value.Count);
                foreach (var pair in orderedAllergensByIngCount)
                {
                    if (danger[pair.Key].Count == 1)
                    {
                        var removeFromOthers = danger[pair.Key].First();
                        foreach (var other in danger.Where(a => a.Value.Count > 1))
                        {
                            danger[other.Key].Remove(removeFromOthers);
                        }
                    }
                }
                
            } while (danger.Any(a => a.Value.Count > 1));

            var answer = string.Join(",", danger.OrderBy(g => g.Key).Select(g => g.Value.First()));
            Console.WriteLine(answer);
            return answer;
        }
    }
}
