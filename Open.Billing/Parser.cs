public class Parser
{
    private string[] tokens;
    private int index;

    public Expression Parse(string expression)
    {
        tokens = expression.Split(' ');
        index = 0;
        return ParseExpression();
    }

    private Expression ParseExpression()
    {
        if (tokens[index].StartsWith("$OP."))
        {
            return ParseOperator();
        }
        else if (tokens[index].StartsWith("$FN."))
        {
            return ParseFunction();
        }
        else if (tokens[index].StartsWith("$PRM"))
        {
            return ParseParameter();
        }
        else
        {
            throw new Exception("Unknown token: " + tokens[index]);
        }
    }

    private Expression ParseOperator()
    {
        string op = tokens[index++];
        Expression left = ParseExpression();
        Expression right = ParseExpression();
        return new OperatorExpression(op, left, right);
    }

    private Expression ParseFunction()
    {
        string func = tokens[index++];
        Expression argument = ParseExpression();
        return new FunctionExpression(func, argument);
    }

    private Expression ParseParameter()
    {
        string param = tokens[index++];
        return new ParameterExpression(param);
    }
}