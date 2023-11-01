
public class FunctionExpression : Expression
{
    private string func;
    private Expression argument;

    public FunctionExpression(string func, Expression argument)
    {
        this.func = func;
        this.argument = argument;
    }

    public override double Evaluate(Dictionary<string, double> parameters)
    {
        double argVal = argument.Evaluate(parameters);

        switch (func)
        {
            case "$FN.TARIFF":
                return argVal * 2; // Simplified example
            default:
                throw new Exception("Unknown function");
        }
    }
}