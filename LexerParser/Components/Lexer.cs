using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using System.Text;

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
        public string Input { get; private set; } // Input string
        private int _index = 0; // Current index in the input string
        private List<Token> _tokens = new List<Token>();  // List of tokens

        public Lexer(string input)
        {
            Input = input; // Set input string
            _index = 0; // Set index to 0
            _tokens = new List<Token>(); // Initialize list of tokens
        }

        public IEnumerable<Token> Tokenize()
        {

            while (_index < Input.Length) // Loop through the input string
            {
                
                if (char.IsWhiteSpace(Input[_index])) // Skip whitespace
                {
                    _index++; // Move to the next character
                }
                
                else if (_index >= Input.Length) // End of input
                {
                    _tokens.Add(new Token(TokenType.EOF, "")); // Add EOF token
                    break; // Break the loop
                }

                else if (_index < Input.Length && char.IsLetter(Input[_index])) // Check if the character is a letter
                {
                    LexKeywordOrIdentifier();
                    
                }

                else if (_index < Input.Length && char.IsDigit(Input[_index])) // Check if the character is a digit
                {
                    LexNumber(); 
                    
                }

                else if (_index < Input.Length && (Input[_index] == '=' || Input[_index] == '+' || Input[_index] == '-' ||
                                              Input[_index] == '*' ||
                                              Input[_index] == '/' || Input[_index] == '%' || Input[_index] == '!' ||
                                              Input[_index] == '>' ||
                                              Input[_index] == '<')) // Check if the character is an operator
                {
                    LexOperator();
                    
                }

                else if (_index < Input.Length && (Input[_index] == '(' || Input[_index] == ')')) // Check if the character is a parenthesis
                {
                    LexParenthesis();
                    
                }

                else if (_index < Input.Length && (Input[_index] == ';' || Input[_index] == ':')) // Check if the character is a punctuation
                {
                    LexPunctuation();
                    
                }
                else if (_index < Input.Length) // Invalid character
                {
                    _tokens.Add(new Token(TokenType.Invalid, Input[_index].ToString()));
                    _index++; // Move to the next character
                }
                
                
            }

            return _tokens; // Return the list of tokens
        }




        private void LexKeywordOrIdentifier()
        {
            var word = string.Empty; // Initialize an empty string
            while (_index < Input.Length && (char.IsLetterOrDigit(Input[_index]) || Input[_index] == '_'))  // Loop through the input string
            {
                word += Input[_index]; // Add the character to the word
                _index++; // Move to the next character
            }
           

            switch (word) // Check if the word is a keyword or an identifier and add the corresponding token
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
            var number = string.Empty; // Initialize an empty string
            while (_index < Input.Length && char.IsDigit(Input[_index])) // Loop through the input string
            {
                number += Input[_index]; // Add the character to the number
                _index++; // Move to the next character
            }

            _tokens.Add(new Token(TokenType.Number, number)); // Add the number token
        } 

        private void LexOperator()
        {
            var op = string.Empty; // Initialize an empty string
            while (_index < Input.Length && (Input[_index] == '=' || Input[_index] == '+' || Input[_index] == '-' ||
                                             Input[_index] == '*' ||
                                             Input[_index] == '/' || Input[_index] == '%' || Input[_index] == '!' ||
                                             Input[_index] == '>' ||
                                             Input[_index] == '<')) // Loop through the input string
            {
                op += Input[_index]; // Add the character to the operator
                _index++; // Move to the next character
            }

            switch (op) // Check the operator and add the corresponding token
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
            string parenthesis = Input[_index].ToString(); // Get the parenthesis
            _index++; // Move to the next character

            switch (parenthesis) // Check the parenthesis and add the corresponding token
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
            string punctuation = Input[_index].ToString(); // Get the punctuation
            _index++; // Move to the next character

            switch (punctuation) // Check the punctuation and add the corresponding token
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