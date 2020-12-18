using System.IO;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    internal class Tokenizer
    {
        private TextReader _tr;

        public Tokenizer(string input)
        {
            _tr = new StringReader(input);
            NextCharacter();
            NextToken();
        }

        public Token Token { get; private set; }
        public int Number { get; private set; }

        private char _currentChar;
        private void NextCharacter()
        {
            var ch = _tr.Read();
            _currentChar = ch < 0 ? '\0' : (char) ch;
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(_currentChar))
            {
                NextCharacter();
            }

            switch (_currentChar)
            {
                case '+':
                    NextCharacter();
                    Token = Token.Add;
                    return;
                
                case '*':
                    NextCharacter();
                    Token = Token.Mul;
                    return;
                
                case '(':
                    NextCharacter();
                    Token = Token.OpenParen;
                    return;
                
                case ')':
                    NextCharacter();
                    Token = Token.CloseParen;
                    return;
                
                case '\0':
                    Token = Token.EndOfLine;
                    return;
            }

            if (char.IsDigit(_currentChar))
            {
                var sb = new StringBuilder();
                while (char.IsDigit(_currentChar))
                {
                    sb.Append(_currentChar);
                    NextCharacter();
                }

                Number = int.Parse(sb.ToString());
                Token = Token.Number;
            }
        }
    }
}