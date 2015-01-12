// ========================================================================
// Siemens Audiologische Technik GmbH
// Department H WS AU RD SW
// Gebbertstr. 125
// D-91058 Erlangen, Germany
// 
// Copyright (c) 2010
// ========================================================================
// /*------------------------- Revision-Info --------------------------------
// $HeaderUTC$	
// $NoKeywords$	
// *---------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MacroStaticAnalyzer.Business
{
    public static class TokenType
    {
        public const int ID = 256;
        public const int Literal = 257;
        public const int Operator = 258;
    }

    public class Token
    {
        public int Type;

        public Token(int type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return ((char)Type).ToString(CultureInfo.InvariantCulture);
        }
    }

    public class ID : Token
    {
        public string Lexeme;

        public ID(string lexeme)
            : base(TokenType.ID)
        {
            Lexeme = lexeme;
        }

        public override string ToString()
        {
            return Lexeme;
        }
    }

    public class Operator : Token
    {
        public string Op;

        public Operator(string op)
            : base(TokenType.Operator)
        {
            Op = op;
        }

        public override string ToString()
        {
            return Op;
        }
    }

    public class Literal : Token
    {
        public string Value;

        public Literal(string value)
            : base(TokenType.Literal)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class LexicalAnalyzer
    {
        private string _content;
        private int _cur;
        private List<char> Ignored = new List<char> { '\t', ' ', '\r', '\n' };
        private List<string> Operators;

        public LexicalAnalyzer()
        {
            var operators = "( ) [ ] { } = : ; . , + - * / != == < > <= >= -> && ||";
            Operators = new List<string>(operators.Split(' '));
        }

        public void Load(string content)
        {
            _content = content;
            _cur = 0;
        }

        public List<Token> ReadToEnd()
        {
            List<Token> result = new List<Token>();
            while (true)
            {
                var token = Read();
                if (token == null) { break; }
                Console.WriteLine(token.ToString());
                result.Add(token);
            }
            return result;
        }

        public Token Read()
        {
            string token = "";
            for (; ; _cur++)
            {
                if (_content.Length <= _cur)
                    return null;

                if (Ignored.Contains(Current()))
                {
                    continue;
                }

                // Ignore comments headed by "//".
                if (Current() == '/' && PeekNext() == '/')
                {
                    SkipCurrentLine();
                    continue;
                }

                // Ignore comments surrounded by "/* */"
                if (Current() == '/' && PeekNext() == '*')
                {
                    while (!(Current() == '*' && PeekNext() == '/'))
                    {
                        _cur++;
                    }
                    _cur += 2;
                    continue;
                }

                if (Regex.IsMatch(Current().ToString(CultureInfo.InvariantCulture), @"\w"))
                {
                    while (Regex.IsMatch(Current().ToString(CultureInfo.InvariantCulture), @"\w"))
                    {
                        token += Current();
                        _cur++;
                    }
                    return new ID(token);
                }

                if (Current() == '\"')
                {
                    do
                    {
                        _cur++;
                        token += Current();
                    } while (Current() != '\"');
                    _cur++; _cur++;
                    return new Literal(token);
                }

                if (Current() == '\'')
                {
                    do
                    {
                        _cur++;
                        token += Current();
                    } while (PeekNext() != '\'');
                    _cur++; _cur++;
                    return new Literal(token);
                }

                if (_content.Length > _cur + 1 && Operators.Contains(Current().ToString(CultureInfo.InvariantCulture) + PeekNext()))
                {
                    var op = new Operator(Current().ToString(CultureInfo.InvariantCulture) + PeekNext());
                    _cur += 2;
                    return op;
                }
                if (Operators.Contains(Current().ToString(CultureInfo.InvariantCulture)))
                {
                    var op = new Operator(Current().ToString(CultureInfo.InvariantCulture));
                    _cur++;
                    return op;
                }


                throw new Exception("Unrecognized token " + Current() + PeekNext());
            }
        }

        public void SkipCurrentLine()
        {
            // Read to the end of line
            while (!(Current() == '\r' && PeekNext() == '\n') && _content.Length > _cur + 1)
            {
                _cur++;
            }
        }

        private char Current()
        {
            return _content[_cur];
        }

        private char PeekNext()
        {
            return _content[_cur + 1];
        }
    }
}