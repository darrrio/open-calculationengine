
using System.Text.Json;
using System.Text.RegularExpressions;
using static Open.Billing.Utility.Extensions;

namespace Open.Billing;
public class BillingExpression
{
    public BillingExpression? leftExpression;
    public BillingExpression? rightExpression;
    private Context _context;
    private string? _value;
    public string? value { get { return _value; } }
    private string? _operator;
    public string? op { get { return _operator; } }
    private decimal _result;
    
    //TODO: Optimize to use simpler regex definitions and dynamic levels
    private static string regex = "(\\$[A-z]+.[A-z]+)";
    private static string regexFirst = "(?:\\(\\$.+;\\$)";
    private static string regexFirst2 = "(?:\\(\\$.+;)";
    private static string regexFirst3 = "(?:\\$PRM.*[^)])";
    private static string regexSecond = "(?:;[^)]+\\$.+)";
    private static string regexSecond2 = "(?:;+\\$.+)";
    private static string regexParam1 = "(?:^\\$PRM1)";
    private static string regexParam2 = "(?:^\\$PRM2)";
    private static string regexParam3 = "(?:^\\$PRM3)";
    public BillingExpression(Context context)
    {
        _context = context;
    }    
    public decimal Evaluate(string? expression)
    {
        scanExpression(expression);
        //TODO: Optimize to use dynamic levels for data input
        if (isFirstLevelParam())
        {
            _operator = _value;
            Dictionary<string,dynamic> dict = HashtableToDictionary<string, dynamic>(_context.data);
            _result = JsonSerializer.Deserialize<decimal>(dict[value!]);
        }
        else if (isSecondLevelParam())
        {
            _operator = _value;
            Dictionary<string,dynamic> dict = HashtableToDictionary<string, dynamic>(_context.data);
            _result = JsonSerializer.Deserialize<decimal>(dict["SERVICES"][0].GetProperty(value!));
        }
        else if (isThirdLevelParam())
        {
            _operator = _value;
            Dictionary<string,dynamic> dict = HashtableToDictionary<string, dynamic>(_context.data);
            _result = JsonSerializer.Deserialize<decimal>(dict["SERVICES"][0].GetProperty("BILLINGUNITS")[0].GetProperty(value!));
        }
        else
        {
            _result = execExpression(expression);
        }

        return _result;
    }
    private bool isThirdLevelParam()
    {
        if (value == null)
            return false;
        return Regex.Matches(value, regexParam3).Count > 0;
    }
    private bool isSecondLevelParam()
    {
        if (value == null)
            return false;
        return Regex.Matches(value, regexParam2).Count > 0;
    }
    private bool isFirstLevelParam()
    {
        if (value == null)
            return false;
        return Regex.Matches(value, regexParam1).Count > 0;
    }
    private void setExpression(string? expression)
    {
        _value = expression!;
    }
    private bool scanExpression(string? expression)
    {
        setExpression(expression);
        //TODO: Optimize to use dynamic levels for data input
        if (!(value == null || isThirdLevelParam() || isSecondLevelParam() || isFirstLevelParam()))
        {
            _operator = Regex.Matches(value!, regex).First().Value;
            leftExpression = new BillingExpression(_context);
            rightExpression = new BillingExpression(_context);

            if (Regex.Matches(value!, regexFirst).Count > 0)
            {
                leftExpression.scanExpression(Regex.Matches(value!, regexFirst).First().Value[1..^2]);
            }
            else if (Regex.Matches(value!, regexFirst2).Count > 0)
            {
                leftExpression.scanExpression(Regex.Matches(value!, regexFirst2).First().Value[1..^0]);
            }
            else if (Regex.Matches(value!, regexFirst3).Count > 0)
            {
                leftExpression.scanExpression(Regex.Matches(value!, regexFirst3).First().Value);
            }
            else
            {
                leftExpression.scanExpression(null);
            }

            if (Regex.Matches(value!, regexSecond).Count > 0)
            {
                rightExpression.scanExpression(Regex.Matches(value!, regexSecond).First().Value[1..^1]);
            }
            else if (Regex.Matches(value!, regexSecond2).Count > 0)
            {
                rightExpression.scanExpression(Regex.Matches(value!, regexSecond2).First().Value[1..^1]);
            }
            else
            {
                rightExpression.scanExpression(null);
            }
        }
        return value == null;
    }
    private decimal execExpression(string? expression)
    {
        //TODO: Optimize to use classes of functions loaded by Dependency Injection
        switch (op)
        {
            case "$OP.SUM":
                {
                    _result = leftExpression!.Evaluate(expression) + rightExpression!.Evaluate(expression);
                    break;
                }
            case "$OP.MULTIPLY":
                {
                    _result = leftExpression!.Evaluate(expression) * rightExpression!.Evaluate(expression);
                    break;
                }
            case "$FN.TARIFF":
                {
                    _result = _context.GetTariff();
                    break;
                }
            default:
                {
                    _result = 0;
                    break;
                }
        }
        return _result;
    }
}