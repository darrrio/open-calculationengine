
public class ParameterExpression : Expression
{
    private string name;

    public ParameterExpression(string name)
    {
        this.name = name;
    }

    public override double Evaluate(Dictionary<string, double> parameters)
    {
        return parameters[name];
    }
}