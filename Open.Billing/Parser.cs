using System.Text;
using System.Text.RegularExpressions;
using Open.Billing;

public class Parser
{
    private Context _context;
    public Parser(Context context)
    {
        _context = context;
    }
    public BillingExpression Parse(string expression)
    {
        BillingExpression billingExpression = new BillingExpression(_context);

        if (expression == null || expression == "")
        {
            return billingExpression;
        }

        var match = Regex.Match(expression, @"(\$[A-z]+\.[A-z]+)\((.*)\)");
        if (match.Success)
        {
            billingExpression.SetValue(match.Groups[1].Value);
            var parameters = SplitParameters(match.Groups[2].Value);
            billingExpression.Childrens = parameters.Select(Parse).ToList();
        }
        else
        {
            billingExpression.SetValue(expression);
        }
        return billingExpression;
    }
    private List<string> SplitParameters(string parameters)
    {
        var splitParameters = new List<string>();
        var currentParameter = new StringBuilder();
        int openParentheses = 0;

        foreach (char c in parameters)
        {
            if (c == '(')
            {
                openParentheses++;
            }
            else if (c == ')')
            {
                openParentheses--;
            }

            currentParameter.Append(c);

            if (c == ';' && openParentheses == 0)
            {
                splitParameters.Add(currentParameter.ToString().TrimEnd(';'));
                currentParameter.Clear();
            }
        }

        if (currentParameter.Length > 0)
        {
            splitParameters.Add(currentParameter.ToString());
        }

        return splitParameters;
    }
}