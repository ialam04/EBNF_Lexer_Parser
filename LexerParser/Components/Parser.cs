namespace ProgrammingLanguagesProject.Components;

public class LexParser
{
    private readonly List<Token> _tokens = new List<Token>();
    private int current = 0;

    public LexParser(List<Token> tokens)
    {
        _tokens = tokens;
    }
    
    public void Parse()
    {
       ParseProgram(); // Start parsing the program
        
    }
    
    private void ParseProgram()
    {
        if (_tokens[current].Type == TokenType.Program) // Check if the first token is a program
        {
            if (current == _tokens.Count - 1 || _tokens[current+1].Type == TokenType.EndProgram) // Check if the program is empty or if the program is not empty, check if the next token is end_program
            {
                throw new Exception("Expected written program"); // Error of no written program
            }
            current++;
            ParseStatements(); // Parse the statements
            if(_tokens[current].Type == TokenType.EndProgram) // Check if the last token is end_program
            {
                current++;
                // Parse successful
            }
            else
            {
                // Error
                throw new Exception("Expected end of program"); // Error of no end of program
            }
        }
        else
        {
            // Error
            throw new Exception("Expected program"); // Error of no program start
        }
        
    }

    private void ParseStatements()
    {
        while(current < _tokens.Count - 1) // Loop through the tokens
        {
            if(_tokens[current].Type == TokenType.Identifier || _tokens[current].Type == TokenType.If || _tokens[current].Type == TokenType.Loop) // Check if the token is an identifier, if, or loop
            {
                ParseStatement(); // Parse the statement
            }
            else
            {
                // Error
                throw new Exception("Invalid Token Entered at index " + current + ": " +_tokens[current].Type); // Error of invalid token
            }
        }
        
    }
    
    private void ParseStatement()
    {
        if (_tokens[current].Type == TokenType.Identifier) // Check if the token is an identifier
        {
            ParseAssignment(); // Parse the assignment
            if(_tokens[current].Type == TokenType.Semicolon) // Check if the next token is a semicolon
            {
                if (current < _tokens.Count - 1) // Check if the current index is not the last index
                {
                    current++; // Move to the next token
                }
            }
            else
            {
                // Error
                throw new Exception("Expected semicolon at index " + current); // Error of no semicolon
            }
        }
        
        else if (_tokens[current].Type == TokenType.If) // Check if the token is an if
        {
            current++; // Move to the next token
            ParseIf(); // Parse the if statement
            if(_tokens[current].Type == TokenType.EndIf) // Check if the next token is end_if
            {
                if (current < _tokens.Count - 1) // Check if the current index is not the last index
                {
                    current++; // Move to the next token
                }
            }
            else
            {
                throw new Exception("Expected end if at index " + current); // Error of no end if
            }
        }
        
        else if (_tokens[current].Type == TokenType.Loop) //Check if token is a loop
        {
            current++; // Move to the next token
            ParseLoop(); // Parse the loop
            if(_tokens[current].Type == TokenType.EndLoop) // Check if the next token is end_loop
            {
                if (current < _tokens.Count - 1) // Check if the current index is not the last index
                {
                    current++; // Move to the next token
                }
            }
            else
            {
                // Error
                throw new Exception("Expected end loop at index " + current); // Error of no end loop
            }
        }
            
        else
        {
            // Error
            throw new Exception("Invalid Token Entered at index " + current + ": " +_tokens[current].Type); // Error of invalid token
        }
        
    }
    
    private void ParseAssignment() 
    {
        
        ParseIdentifier(); // Parse the identifier
        if (_tokens[current].Type == TokenType.AssignmentOp) // Check if the token is an assignment operator
        {
            current++; // Move to the next token
            ParseExpression(); // Parse the expression
        }
        else
        {
            // Error
            throw new Exception("Expected assignment operator at index " + current); // Error of no assignment
        }
        
    }
    
    private void ParseExpression() 
    {
        ParseTerm(); // Parse the term
        while (_tokens[current].Type == TokenType.PlusOp || _tokens[current].Type == TokenType.MinusOp) // Check if the token is a plus or minus operator
        {
            current++; // Move to the next token
            ParseTerm(); // Parse the term
        }
        
    }
    
    private void ParseTerm()
    {
        ParseFactor(); // Parse the factor
        while (_tokens[current].Type == TokenType.MultiplyOp || _tokens[current].Type == TokenType.DivideOp || _tokens[current].Type == TokenType.ModuloOp) // Check if the token is a multiply, divide, or modulo operator
        {
            current++; // Move to the next token
            ParseFactor(); // Parse the factor
        }
    }
    
    private void ParseFactor()
    {
        if(_tokens[current].Type == TokenType.Number) // Check if the token is a number
        {
            ParseNumber(); // Parse the number
        }
        else if(_tokens[current].Type == TokenType.Identifier) // Check if the token is an identifier
        {
            ParseIdentifier(); // Parse the identifier
        }
        else if(_tokens[current].Type == TokenType.OpenParen) // Check if the token is an open parenthesis
        {
            current++; // Move to the next token
            ParseExpression(); // Parse the expression
            if(_tokens[current].Type == TokenType.CloseParen) // Check if the next token is a close parenthesis
            {
                if(current < _tokens.Count - 1) // Check if the current index is not the last index
                {
                    current++; // Move to the next token
                }
            }
            else
            {
                throw new Exception("Expected right parenthesis at index " + current); // Error of no right parenthesis
            }
        }
        else
        {
            throw new Exception("Expected number, identifier, or left parenthesis at index " + current); // Error of invalid token
        }
    }
    
    private void ParseNumber()
    {
        current++; // Move to the next token
    }
    
    private void ParseIdentifier()
    {
        current++; // Move to the next token
    }
    
    private void ParseIf()
    {
        if(_tokens[current].Type == TokenType.OpenParen) // Check if the token is an open parenthesis
        {
            current++; // Move to the next token
            ParseLogicalExpression(); // Parse the logical expression
            if(_tokens[current].Type == TokenType.CloseParen) // Check if the next token is a close parenthesis
            {
                if(current < _tokens.Count - 1) // Check if the current index is not the last index
                {
                    current++; // Move to the next token
                }
            }
            else
            {
                throw new Exception("Expected right parenthesis at index " + current); // Error of no right parenthesis
            }
        }
        else
        {
            throw new Exception("Expected left parenthesis at index " + current); // Error of no left parenthesis
        }
        while (_tokens[current].Type != TokenType.EndIf && current < _tokens.Count-1) // Loop through the tokens in the if statement
        {
            ParseStatement(); // Parse the statement

        }
       
        

    }
    
    private void ParseLoop()
    {
        if(_tokens[current].Type == TokenType.OpenParen) // Check if the token is an open parenthesis
        {
            current++; // Move to the next token
            if(_tokens[current].Type == TokenType.Identifier) // Check if the token is an identifier
            {
                ParseIdentifier(); // Parse the identifier
                if(_tokens[current].Type == TokenType.AssignmentOp) // Check if the token is an assignment operator
                {
                    current++; // Move to the next token
                    ParseExpression(); // Parse the expression
                    if(_tokens[current].Type == TokenType.Colon) // Check if the token is a colon
                    {
                        current++; // Move to the next token
                        if(_tokens[current].Type == TokenType.Identifier) // Check if the token is an identifier
                        {
                            ParseIdentifier(); // Parse the identifier
                        }
                        else if(_tokens[current].Type == TokenType.Number) // Check if the token is a number
                        {
                            ParseNumber(); // Parse the number
                        }
                        else
                        {
                            throw new Exception("Expected identifier at index " + current); // Error of no identifier
                        }
                    }
                    else
                    {
                        throw new Exception("Expected Colon at index " + current); // Error of no colon
                    }
                }
                else
                {
                    throw new Exception("Expected assignment operator at index " + current); // Error of no assignment
                }
            }
            else
            {
                throw new Exception("Expected identifier at index " + current); // Error of no identifier
            }
            if(_tokens[current].Type == TokenType.CloseParen) // Check if the next token is a close parenthesis
            {
                if(current < _tokens.Count - 1) // Check if the current index is not the last index
                {
                    current++; // Move to the next token
                }
            }
            else
            {
                throw new Exception("Expected right parenthesis at index " + current); // Error of no right parenthesis
            }
        }
        else
        {
            // Error
            throw new Exception("Expected left parenthesis at index " + current); // Error of no left parenthesis
        }
        while (_tokens[current].Type != TokenType.EndLoop && current < _tokens.Count-1) // Loop through the tokens in the loop statement
        {
            ParseStatement(); // Parse the statement
            
        }
        
    }
    
    private void ParseLogicalExpression() 
    {
        if(_tokens[current].Type == TokenType.Identifier || _tokens[current].Type == TokenType.Number) // Check if the token is an identifier or number
        {
            ParseLogicalTerm(); // Parse the logical term
            if(_tokens[current].Type == TokenType.EqualOp || _tokens[current].Type == TokenType.NotEqualOp || _tokens[current].Type == TokenType.GreaterOp || _tokens[current].Type == TokenType.LessOp || _tokens[current].Type == TokenType.GreaterEqOp || _tokens[current].Type == TokenType.LessEqOp) // Check if the token is an equal, not equal, greater, less, greater or equal, or less or equal operator
            {
                current++; // Move to the next token
                if(_tokens[current].Type == TokenType.Identifier || _tokens[current].Type == TokenType.Number) // Check if the token is an identifier or number
                {
                    ParseLogicalTerm(); // Parse the logical term
                }
                else
                {
                    // Error
                    throw new Exception("Expected identifier or number at index " + current); // Error of no identifier or number
                }
            }
            else
            {
                // Error
                throw new Exception("Expected logical operator at index " + current); // Error of no logical operator
            }
        }
        else
        {
            // Error
            throw new Exception("Expected identifier or number at index " + current); // Error of no identifier or number
        }
        
    }
    
    private void ParseLogicalTerm()
    {
        current++; // Move to the next token
    }
    
}