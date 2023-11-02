
public class OperatorExpression : Expression
{
    private string op;
    private Expression left;
    private Expression right;

    public OperatorExpression(string op, Expression left, Expression right)
    {
        this.op = op;
        this.left = left;
        this.right = right;
    }

    public override double Evaluate(Dictionary<string, double> parameters)
    {
        double leftVal = left.Evaluate(parameters);
        double rightVal = right.Evaluate(parameters);

        switch (op)
        {
            case "$OP.SUM":
                return leftVal + rightVal;
            case "$OP.MULTIPLY":
                return leftVal * rightVal;
            default:
                throw new Exception("Unknown operator");
        }
    }
}