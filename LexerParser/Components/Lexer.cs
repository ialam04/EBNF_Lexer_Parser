using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;

namespace ProgrammingLanguagesProject.Components
{

    public enum TokenType
    {
        Program, // "program"
        EndProgram, // "end_program"
        If, // "if"
        EndIf, // "end_if"
        Loop, // "loop"
        EndLoop, // "end_loop"
        Identifier, // Variable names
        Number, // Numeric literals
        AssignmentOp, // "="
        PlusOp, // "+"
        MinusOp, // "-"
        MultiplyOp, // "*"
        DivideOp, // "/"
        ModuloOp, // "%"
        EqualOp, // "=="
        NotEqualOp, // "!="
        GreaterOp, // ">"
        LessOp, // "<"
        GreaterEqOp, // ">="
        LessEqOp, // "<="
        OpenParen, // "("
        CloseParen, // ")"
        Semicolon, // ";"
        Colon, // ":"
        EOF, // End of file/input
        Invalid,
        InvalidStart,
        InvalidEnd,
        InvalidOp
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    
    
    public class Lexer
    {
        public string Input { get; private set; }
        private int _index = 0;
        private List<Token> _tokens = new List<Token>();

        public Lexer(string input)
        {
            Input = input;
            _index = 0;
            _tokens = new List<Token>();
        }

        public IEnumerable<Token> Tokenize()
        {

            while (_index < Input.Length - 1)
            {
                
                if (char.IsWhiteSpace(Input[_index]))
                {
                    _index++;
                }
                
                else if (_index >= Input.Length)
                {
                    _tokens.Add(new Token(TokenType.EOF, ""));
                    break;
                }

                else if (_index < Input.Length && char.IsLetter(Input[_index]))
                {
                    LexKeywordOrIdentifier();
                    
                }

                else if (_index < Input.Length && char.IsDigit(Input[_index]))
                {
                    LexNumber();
                    
                }

                else if (_index < Input.Length && (Input[_index] == '=' || Input[_index] == '+' || Input[_index] == '-' ||
                                              Input[_index] == '*' ||
                                              Input[_index] == '/' || Input[_index] == '%' || Input[_index] == '!' ||
                                              Input[_index] == '>' ||
                                              Input[_index] == '<'))
                {
                    LexOperator();
                    
                }

                else if (_index < Input.Length && (Input[_index] == '(' || Input[_index] == ')'))
                {
                    LexParenthesis();
                    
                }

                else if (_index < Input.Length && (Input[_index] == ';' || Input[_index] == ':'))
                {
                    LexPunctuation();
                    
                }
                else if (_index < Input.Length)
                {
                    _tokens.Add(new Token(TokenType.Invalid, Input[_index].ToString()));
                    _index++;
                }
                
                
            }

            return _tokens;
        }




        private void LexKeywordOrIdentifier()
        {
            var word = string.Empty;
            while (_index < Input.Length && (char.IsLetterOrDigit(Input[_index]) || Input[_index] == '_')) 
            {
                word += Input[_index];
                _index++;
            }
           

            switch (word)
            {
                case "if":
                    _tokens.Add(new Token(TokenType.If, word));
                    break;
                case "end_if":
                    _tokens.Add(new Token(TokenType.EndIf, word));
                    break;
                case "loop":
                    _tokens.Add(new Token(TokenType.Loop, word));
                    break;
                case "end_loop":
                    _tokens.Add(new Token(TokenType.EndLoop, word));
                    break;
                case "end_program":
                    _tokens.Add(new Token(TokenType.EndProgram, word));
                    break;
                case "program":
                    _tokens.Add(new Token(TokenType.Program, word));
                    break;
                default:
                    _tokens.Add(new Token(TokenType.Identifier, word));
                    break;
            }
        }

        private void LexNumber()
        {
            var number = string.Empty;
            while (_index < Input.Length && char.IsDigit(Input[_index]))
            {
                number += Input[_index];
                _index++;
            }

            _tokens.Add(new Token(TokenType.Number, number));
        }

        private void LexOperator()
        {
            var op = string.Empty;
            while (_index < Input.Length && (Input[_index] == '=' || Input[_index] == '+' || Input[_index] == '-' ||
                                             Input[_index] == '*' ||
                                             Input[_index] == '/' || Input[_index] == '%' || Input[_index] == '!' ||
                                             Input[_index] == '>' ||
                                             Input[_index] == '<'))
            {
                op += Input[_index];
                _index++;
            }

            switch (op)
            {
                case "=":
                    _tokens.Add(new Token(TokenType.AssignmentOp, op));
                    break;
                case "+":
                    _tokens.Add(new Token(TokenType.PlusOp, op));
                    break;
                case "-":
                    _tokens.Add(new Token(TokenType.MinusOp, op));
                    break;
                case "*":
                    _tokens.Add(new Token(TokenType.MultiplyOp, op));
                    break;
                case "/":
                    _tokens.Add(new Token(TokenType.DivideOp, op));
                    break;
                case "%":
                    _tokens.Add(new Token(TokenType.ModuloOp, op));
                    break;
                case "==":
                    _tokens.Add(new Token(TokenType.EqualOp, op));
                    break;
                case "!=":
                    _tokens.Add(new Token(TokenType.NotEqualOp, op));
                    break;
                case ">":
                    _tokens.Add(new Token(TokenType.GreaterOp, op));
                    break;
                case "<":
                    _tokens.Add(new Token(TokenType.LessOp, op));
                    break;
                case ">=":
                    _tokens.Add(new Token(TokenType.GreaterEqOp, op));
                    break;
                case "<=":
                    _tokens.Add(new Token(TokenType.LessEqOp, op));
                    break;
                default:
                    _tokens.Add(new Token(TokenType.InvalidOp, op));
                    break;
            }

        }

        private void LexParenthesis()
        {
            string parenthesis = Input[_index].ToString();
            _index++;

            switch (parenthesis)
            {
                case "(":
                    _tokens.Add(new Token(TokenType.OpenParen, parenthesis));
                    break;
                case ")":
                    _tokens.Add(new Token(TokenType.CloseParen, parenthesis));
                    break;
                default:
                    _tokens.Add(new Token(TokenType.Invalid, parenthesis));
                    break;
            }

        }

        private void LexPunctuation()
        {
            string punctuation = Input[_index].ToString();
            _index++;

            switch (punctuation)
            {
                case ";":
                    _tokens.Add(new Token(TokenType.Semicolon, punctuation));
                    break;
                case ":":
                    _tokens.Add(new Token(TokenType.Colon, punctuation));
                    break;
                default:
                    _tokens.Add(new Token(TokenType.Invalid, punctuation));
                    break;
            }
        }
    }
}