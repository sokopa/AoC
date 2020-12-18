using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;

namespace AdventOfCode.Solutions.Year2020
{

    class Day18 : ASolution
    {
        private string[] input;
        public Day18() : base(18, 2020, "")
        {
//             DebugInput = @"1 + (2 * 3) + (4 * (5 + 6))
// 2 * 3 + (4 * 5)
// 5 + (8 * 3 + 9 + 3 * 4 * 3)
// 5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))
// ((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2";
            input = Input.SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            var calc = new WeirdCalculator();
            long acc = 0;
            foreach (var line in input)
            {
                var result = calc.Calculate(line);
                //Console.WriteLine($"[{line}] = {result}");
                acc += result;
            }
            
            return acc.ToString();
        }

        protected override string SolvePartTwo()
        {
            var calc = new WeirdCalculator();
            long acc = 0;
            foreach (var line in input)
            {
                var result = calc.Calculate2(line);
                Console.WriteLine($"[{line}] = {result}");
                acc += result;
            }
            
            return acc.ToString();
        }
    }

    internal class WeirdCalculator
    {
        public long Calculate(string line)
        {
            var tokenizer = new Tokenizer(line);
            var parser = new Parser(tokenizer);
            var node = parser.Parse();
            return node.Evaluate();
        }
        
        public long Calculate2(string line)
        {
            var tokenizer = new Tokenizer(line);
            var parser = new Parser(tokenizer);
            var node = parser.Parse2();
            return node.Evaluate();
        }
        
    }

    // inspiration from https://github.com/toptensoftware/SimpleExpressionEngine/tree/Step6Functions/SimpleExpressionEngine
    internal class Parser
    {
        private readonly Tokenizer _tokenizer;

        public Parser(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public Node Parse()
        {
            var expr = ParseAddMul();

            return expr;
        }

        public Node Parse2()
        {
            var expr = ParseMul();
            return expr;
        }

        Node ParseMul()
        {
            var lhs = ParseAdd();
            while (true)
            {
                Func<long, long, long> op = null;
                var currToken = _tokenizer.Token;
                if (currToken == Token.Mul)
                {
                    op = (a, b) => a * b;
                }
                
                if (op == null) return lhs;
                
                _tokenizer.NextToken();

                var rhs = ParseAdd();

                lhs = new BinaryNode(lhs, rhs, currToken, op);
            }
        }

        Node ParseAdd()
        {
            var lhs = ParseLeaf(ParseMul);
            while (true)
            {
                Func<long, long, long> op = null;
                var currToken = _tokenizer.Token;
                if (currToken == Token.Add)
                {
                    op = (a, b) => a + b;
                }
                
                if (op == null) return lhs;
                
                _tokenizer.NextToken();

                var rhs = ParseLeaf(ParseMul);

                lhs = new BinaryNode(lhs, rhs, currToken, op);
            }
        }

        Node ParseAddMul()
        {
            var lhs = ParseLeaf(ParseAddMul);
            while (true)
            {
                Func<long, long, long> op = null;
                var currToken = _tokenizer.Token;
                if (currToken == Token.Add)
                {
                    op = (a, b) => a + b;
                }
                else if (currToken == Token.Mul)
                {
                    op = (a, b) => a * b;
                }

                if (op == null) return lhs;
                
                _tokenizer.NextToken();

                var rhs = ParseLeaf(ParseAddMul);

                lhs = new BinaryNode(lhs, rhs, currToken, op);
            }
        }

        Node ParseLeaf(Func<Node> nextLevelParser)
        {
            if (_tokenizer.Token == Token.Number)
            {
                var node = new NumberNode(_tokenizer.Number);
                _tokenizer.NextToken();
                return node;
            }

            if (_tokenizer.Token == Token.OpenParen)
            {
                _tokenizer.NextToken();
                var node = nextLevelParser();
                _tokenizer.NextToken();
                return node;
            }

            throw new SyntaxErrorException();
        }
    }
}
