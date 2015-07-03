using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ExpressionParser.Main.Tokens;

namespace ExpressionParser.Main
{
    public class Tokenizer<TEntity>
    {
        private readonly StringReader _reader;
        private string _text;

        public Tokenizer(string text)
        {
            _text = text;
            _reader = new StringReader(text);
        }

        public IEnumerable<Token> Tokenize()
        {
            var tokens = new List<Token>();
            var parenthesisDepth = 0;
            while (_reader.Peek() != -1)
            {
                while (Char.IsWhiteSpace((char)_reader.Peek()))
                {
                    _reader.Read();
                }

                if (_reader.Peek() == -1)
                    break;

                var c = (char)_reader.Peek();
                switch (c)
                {
                    case '(':
                        parenthesisDepth++;
                        tokens.Add(new OpenParenthesisToken(parenthesisDepth));
                        _reader.Read();
                        break;
                    case ')':
                        tokens.Add(new ClosedParenthesisToken(parenthesisDepth));
                        parenthesisDepth--;
                        _reader.Read();
                        break;

                    // try to check if it is a predicate
                    case '=':
                        tokens.Add(new EqualsToken());
                        _reader.Read();
                        break;
                    case '!':
                        // it most continue with a =, otherwise error
                        _reader.Read();
                        if ((char) _reader.Peek() == '=')
                        {
                            tokens.Add(new NotEqualsToken());
                            _reader.Read();
                        }
                        else
                        {
                            ThrowError();
                        }
                        break;
                    case '<':
                        _reader.Read();
                        if ((char) _reader.Peek() == '=')
                        {
                            tokens.Add(new LessOrEqualsToken());
                            _reader.Read();
                        }
                        else
                        {
                            tokens.Add(new LessThanToken());
                        }
                        break;
                    case '>':
                        _reader.Read();
                        if ((char) _reader.Peek() == '=')
                        {
                            tokens.Add(new GreaterOrEqualsToken());
                            _reader.Read();
                        }
                        else
                        {
                            tokens.Add(new GreaterThanToken());
                        }
                        break;

                    case '\'':
                        // valuetoken
                        var valuetoken = ParseValueToken();
                        tokens.Add(valuetoken);
                        break;
                    //case 'i': // to check 'in'
                    default:
                        if (Char.IsLetter(c))
                        {
                            var token = ParseKeyword(parenthesisDepth);
                            tokens.Add(token);
                        }
                        else
                        {
                            ThrowError();
                        }
                        break;
                }
            }
            return tokens;
        }

        private Token ParseValueToken()
        {
            var text = new StringBuilder();
            _reader.Read(); // move one step from '
            while ((char) _reader.Peek() != '\'')
            {
                text.Append((char)_reader.Read());
            }
            _reader.Read(); // skip over the the closing '
            return new PropertyValueToken(text.ToString());
        }

        private void ThrowError()
        {
            var remainingText = _reader.ReadToEnd() ?? string.Empty;
            throw new Exception(string.Format("Unknown grammar found at position {0} : '{1}'",
                _text.Length - remainingText.Length, remainingText));
        }

        private Token ParseKeyword(int depth)
        {
            var text = new StringBuilder();
            while (Char.IsLetter((char)_reader.Peek()))
            {
                text.Append((char)_reader.Read());
            }

            var potentialKeyword = text.ToString().ToLower();

            switch (potentialKeyword)
            {
                case "and":
                    return new AndToken(depth);
                case "or":
                    return new OrToken(depth);
                default:
                    return CheckPropertyToken(potentialKeyword);
            }
        }

        private Token CheckPropertyToken(string potentialKeyword)
        {
            var acceptedKeywords = typeof (TEntity).GetProperties();
            if (acceptedKeywords.Select(x => x.Name.ToLower()).Contains(potentialKeyword))
            {
                var prop =
                    typeof (TEntity).GetProperty(potentialKeyword,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                return new ObjectPropertyToken(prop);
            }
            throw new Exception("Expected keyword (And, Or) but found " + potentialKeyword);
        }
    }
}