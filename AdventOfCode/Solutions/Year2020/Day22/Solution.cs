using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Day22 : ASolution
    {
        private List<int> _player1Cards;
        private List<int> _player2Cards;

        public Day22() : base(22, 2020, "")
        {
            var players = Input.Replace("\r\n", "\n").Split("\n\n");
            _player1Cards = players[0].SplitByNewline().Skip(1).Select(int.Parse).ToList();
            _player2Cards = players[1].SplitByNewline().Skip(1).Select(int.Parse).ToList();
        }

        protected override string SolvePartOne()
        {
            var p1Deck = new Queue<int>(_player1Cards);
            var p2Deck = new Queue<int>(_player2Cards);

            var round = 1;
            while (p1Deck.Count > 0 && p2Deck.Count > 0)
            {
                // execute a round.
                //Console.WriteLine($"-- Round {round++:000} --");
                //Console.WriteLine($"Player 1's deck: {string.Join(",", p1Deck.ToList())}");
                //Console.WriteLine($"Player 2's deck: {string.Join(",", p2Deck.ToList())}");
                var p1 = p1Deck.Dequeue();
                var p2 = p2Deck.Dequeue();
                //Console.WriteLine($"Player 1 plays: {p1}");
                //Console.WriteLine($"Player 1 plays: {p2}");
                if (p1 > p2)
                {
                    //Console.WriteLine("Player 1 wins the round!");
                    p1Deck.Enqueue(p1);
                    p1Deck.Enqueue(p2);
                }
                else
                {
                    //Console.WriteLine("Player 2 wins the round!");
                    p2Deck.Enqueue(p2);
                    p2Deck.Enqueue(p1);
                }
            }

            // we should now have an empty deck.
            var winningDeck = p1Deck.Count == 0 ? p2Deck : p1Deck;

            var winningList = winningDeck.ToList();
            var score = CardsScore(winningList);

            return score.ToString();
        }

        private int CardsScore(ICollection<int> cards)
        {
            var score = cards.Select((n, i) => n * (cards.Count - i)).Sum();
            return score;
        }

        // returns 1/2 depending on who won the recursive game
        private (int, int, int) RecursiveCombat(Queue<int> p1Deck, Queue<int> p2Deck, int game)
        {
            var p1History = new List<List<int>>();
            var p2History = new List<List<int>>();
            var winner = 1;
            var round = 1;

            while (p1Deck.Count > 0 && p2Deck.Count > 0)
            {
                // execute a round.
                //Console.WriteLine($"-- Round {round++:000} (Game {game:00}) --");
                var p1List = p1Deck.ToList();
                var p2List = p2Deck.ToList();
                //Console.WriteLine($"Player 1's deck: {string.Join(",", p1Deck.ToList())}");
                //Console.WriteLine($"Player 2's deck: {string.Join(",", p2Deck.ToList())}");
                if (CheckHistory(p1History, p1List) || CheckHistory(p2History, p2List))
                    return (1, CardsScore(p1List), CardsScore(p2List));

                p1History.Add(p1List);
                p2History.Add(p2List);

                var p1 = p1Deck.Dequeue();
                var p2 = p2Deck.Dequeue();
                // Console.WriteLine($"Player 1 plays: {p1}");
                // Console.WriteLine($"Player 1 plays: {p2}");
                if (p1 <= p1Deck.Count && p2 <= p2Deck.Count)
                    // recursive combat
                    (winner, _, _) = RecursiveCombat(new Queue<int>(p1Deck.ToList().Take(p1)), 
                        new Queue<int>(p2Deck.ToList().Take(p2)), game++);
                else
                    winner = p1 > p2 ? 1 : 2;

                if (winner == 1)
                {
                    p1Deck.Enqueue(p1);
                    p1Deck.Enqueue(p2);
                }
                else
                {
                    p2Deck.Enqueue(p2);
                    p2Deck.Enqueue(p1);
                }
            }

            return (winner, CardsScore(p1Deck.ToList()), CardsScore(p2Deck.ToList()));
        }

        protected override string SolvePartTwo()
        {
            var p1Deck = new Queue<int>(_player1Cards);
            var p2Deck = new Queue<int>(_player2Cards);
            var (w,p1,p2) = RecursiveCombat(p1Deck, p2Deck, 0);
            var score = w == 1 ? p1 : p2;
            
            
            return score.ToString();
        }

        private bool CheckHistory(List<List<int>> history, List<int> current)
        {
            return history.Any(item => item.SequenceEqual(current));
        }
    }
}