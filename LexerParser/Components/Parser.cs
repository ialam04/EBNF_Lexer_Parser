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
       ParseProgram();
        
    }
    
    private void ParseProgram()
    {
        if (_tokens[current].Type == TokenType.Program)
        {
            if (current == _tokens.Count - 1)
            {
                throw new Exception("Expected written program");
            }
            current++;
            ParseStatements();
            if(_tokens[current].Type == TokenType.EndProgram)
            {
                // End of program
                current++;
                // Parse successful
            }
            else
            {
                // Error
                throw new Exception("Expected end of program");
            }
        }
        else
        {
            // Error
            throw new Exception("Expected program");
        }
        
    }

    private void ParseStatements()
    {
        while(current < _tokens.Count - 1)
        {
            if(_tokens[current].Type == TokenType.Identifier || _tokens[current].Type == TokenType.If || _tokens[current].Type == TokenType.Loop)
            {
                ParseStatement();
            }
            else
            {
                // Error
                throw new Exception("Expected identifier, if, or loop");
            }
        }
        
    }
    
    private void ParseStatement()
    {
        if (_tokens[current].Type == TokenType.Identifier)
        {
            ParseAssignment();
            if(_tokens[current].Type == TokenType.Semicolon)
            {
                if (current < _tokens.Count - 1)
                {
                    current++;
                }
            }
            else
            {
                // Error
                throw new Exception("Expected semicolon");
            }
        }
        
        else if (_tokens[current].Type == TokenType.If)
        {
            current++;
            ParseIf();
            if(_tokens[current].Type == TokenType.EndIf)
            {
                if (current < _tokens.Count - 1)
                {
                    current++;
                }
            }
            else
            {
                // Error
                throw new Exception("Expected end if");
            }
        }
        
        else if (_tokens[current].Type == TokenType.Loop)
        {
            current++;
            ParseLoop();
            if(_tokens[current].Type == TokenType.EndLoop)
            {
                if (current < _tokens.Count - 1)
                {
                    current++;
                }
            }
            else
            {
                // Error
                throw new Exception("Expected end loop");
            }
        }
        
    }
    
    private void ParseAssignment()
    {
        
        ParseIdentifier();
        if (_tokens[current].Type == TokenType.AssignmentOp)
        {
            current++;
            ParseExpression();
        }
        else
        {
            // Error
            throw new Exception("Expected assignment");
        }
        
    }
    
    private void ParseExpression()
    {
        ParseTerm();
        while (_tokens[current].Type == TokenType.PlusOp || _tokens[current].Type == TokenType.MinusOp)
        {
            current++;
            ParseTerm();
        }
        
    }
    
    private void ParseTerm()
    {
        ParseFactor();
        while (_tokens[current].Type == TokenType.MultiplyOp || _tokens[current].Type == TokenType.DivideOp || _tokens[current].Type == TokenType.ModuloOp)
        {
            current++;
            ParseFactor();
        }
    }
    
    private void ParseFactor()
    {
        if(_tokens[current].Type == TokenType.Number)
        {
            ParseNumber();
        }
        else if(_tokens[current].Type == TokenType.Identifier)
        {
            ParseIdentifier();
        }
        else if(_tokens[current].Type == TokenType.OpenParen)
        {
            current++;
            ParseExpression();
            if(_tokens[current].Type == TokenType.CloseParen)
            {
                current++;
            }
            else
            {
                // Error
                throw new Exception("Expected right parenthesis");
            }
        }
        else
        {
            // Error
            throw new Exception("Expected number, identifier, or left parenthesis");
        }
    }
    
    private void ParseNumber()
    {
        current++;
    }
    
    private void ParseIdentifier()
    {
        current++;
    }
    
    private void ParseIf()
    {
        if(_tokens[current].Type == TokenType.OpenParen)
        {
            current++;
            ParseLogicalExpression();
            if(_tokens[current].Type == TokenType.CloseParen)
            {
                current++;
            }
            else
            {
                // Error
                throw new Exception("Expected right parenthesis");
            }
        }
        else
        {
            // Error
            throw new Exception("Expected left parenthesis");
        }
        while (_tokens[current].Type != TokenType.EndIf && current < _tokens.Count-1)
        {
            ParseStatement();

        }
       
        

    }
    
    private void ParseLoop()
    {
        if(_tokens[current].Type == TokenType.OpenParen)
        {
            current++;
            if(_tokens[current].Type == TokenType.Identifier)
            {
                ParseIdentifier();
                if(_tokens[current].Type == TokenType.AssignmentOp)
                {
                    current++;
                    ParseExpression();
                    if(_tokens[current].Type == TokenType.Colon)
                    {
                        current++;
                        if(_tokens[current].Type == TokenType.Identifier)
                        {
                            ParseIdentifier();
                        }
                        else
                        {
                            // Error
                            throw new Exception("Expected identifier");
                        }
                    }
                    else
                    {
                        // Error
                        throw new Exception("Expected Colon");
                    }
                }
                else
                {
                    // Error
                    throw new Exception("Expected assignment");
                }
            }
            else
            {
                // Error
                throw new Exception("Expected identifier");
            }
            if(_tokens[current].Type == TokenType.CloseParen)
            {
                current++;
            }
            else
            {
                // Error
                throw new Exception("Expected right parenthesis");
            }
        }
        else
        {
            // Error
            throw new Exception("Expected left parenthesis");
        }
        while (_tokens[current].Type != TokenType.EndLoop && current < _tokens.Count-1)
        {
            ParseStatement();
            
        }
        
    }
    
    private void ParseLogicalExpression()
    {
        if(_tokens[current].Type == TokenType.Identifier || _tokens[current].Type == TokenType.Number)
        {
            ParseLogicalTerm();
            if(_tokens[current].Type == TokenType.EqualOp || _tokens[current].Type == TokenType.NotEqualOp || _tokens[current].Type == TokenType.GreaterOp || _tokens[current].Type == TokenType.LessOp || _tokens[current].Type == TokenType.GreaterEqOp || _tokens[current].Type == TokenType.LessEqOp)
            {
                current++;
                if(_tokens[current].Type == TokenType.Identifier || _tokens[current].Type == TokenType.Number)
                {
                    ParseLogicalTerm();
                }
                else
                {
                    // Error
                    throw new Exception("Expected identifier or number");
                }
            }
            else
            {
                // Error
                throw new Exception("Expected logical operator");
            }
        }
        else
        {
            // Error
            throw new Exception("Expected identifier or number");
        }
        
    }
    
    private void ParseLogicalTerm()
    {
        current++;
    }
    
}