
using System.Text.Json;
using System.Text.RegularExpressions;
using static Open.Billing.Utility.Extensions;

namespace Open.Billing;
public class BillingExpression
{
    public BillingExpression? leftExpression;
    public BillingExpression? rightExpression;
    public List<BillingExpression> Childrens { get; set; } = new List<BillingExpression>();
    private Context _context;
    private string? _value;
    private string? _operator;
    private decimal _result;
    public string? Value { get { return _value; } }
    public string? Op { get { return _operator; } }

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
    public void SetValue(string value)
    {
        _value = value;
    }
    public decimal Evaluate(string expression)
    {
        _ParseExpression(expression);
        return _Evaluate(expression);
    }
    public decimal EvaluateComplex(string expression)
    {
        if(expression == null)
        {
            return 0;
        }
        Parser parser = new Parser(_context);
        var billingExpression = parser.Parse(expression);
        Console.WriteLine($"Expression: {expression}");  // Debug statement
        return billingExpression._EvaluateComplex();
    }
    private decimal _EvaluateComplex()
    {
        Console.WriteLine($"Expression: {Value}");  // Debug statement
        Console.WriteLine($"Childrens: {Childrens.ToArray()}");  // Debug statement
        if (Childrens.Count == 0)
        {
            _result = JsonSerializer.Deserialize<decimal>(_context.FlatData[Value!]);
        }
        switch (Value)
        {
            case "$OP.SUM":
                {
                    _result = Childrens.Sum(child => child._EvaluateComplex());
                    Console.WriteLine($"Value: {Value}, Result: {_result}");  // Debug statement
                    break;
                }
            case "$OP.MULTIPLY":
                {
                    _result = Childrens.Aggregate(1m, (acc, child) => acc * child._EvaluateComplex());
                    Console.WriteLine($"Value: {Value}, Result: {_result}");  // Debug statement
                    break;
                }
            case "$FN.TARIFF":
                {
                    _result = _context.GetTariff();
                    Console.WriteLine($"Value: {Value}, Result: {_result}");  // Debug statement
                    break;
                }
            default:
                {
                    _result = JsonSerializer.Deserialize<decimal>(_context.FlatData[Value!]);
                    Console.WriteLine($"Value: {Value}, Result: {_result}");  // Debug statement
                    break;
                }
        }
        return _result;
    }
    private void _ParseExpression(string? expression)
    {
        if (expression == null)
        {
            return;
        }

        _value = expression;

        //TODO: Optimize to use dynamic levels for data input
        if (Value == null || _IsThirdLevelParam() || _IsSecondLevelParam() || _IsFirstLevelParam())
        {
            return;
        }

        _operator = Regex.Matches(Value!, regex).First().Value;
        leftExpression = new BillingExpression(_context);
        rightExpression = new BillingExpression(_context);

        var firstMatch1 = Regex.Matches(Value!, regexFirst).FirstOrDefault();
        var firstMatch2 = Regex.Matches(Value!, regexFirst2).FirstOrDefault();
        var firstMatch3 = Regex.Matches(Value!, regexFirst3).FirstOrDefault();

        var secondMatch1 = Regex.Matches(Value!, regexSecond).FirstOrDefault();
        var secondMatch2 = Regex.Matches(Value!, regexSecond2).FirstOrDefault();

        var firstMatch = firstMatch1 != null ? firstMatch1.Value[1..^2] : (firstMatch2 != null ? firstMatch2.Value[1..^0] : (firstMatch3 != null ? firstMatch3.Value : null));
        var secondMatch = secondMatch1 != null ? secondMatch1.Value[1..^1] : (secondMatch2 != null ? secondMatch2.Value[1..^1] : null);

        leftExpression._ParseExpression(firstMatch);
        rightExpression._ParseExpression(secondMatch);
    }
    private decimal _Evaluate(string? expression)
    {
        //TODO: Optimize to use dynamic levels for data input
        if (_IsFirstLevelParam())
        {
            _operator = _value;
            _result = JsonSerializer.Deserialize<decimal>(_context.FlatData[Value!]);
        }
        else if (_IsSecondLevelParam())
        {
            _operator = _value;
            _result = JsonSerializer.Deserialize<decimal>(_context.FlatData[Value!]);
        }
        else if (_IsThirdLevelParam())
        {
            _operator = _value;
            _result = JsonSerializer.Deserialize<decimal>(_context.FlatData[Value!]);
        }
        else
        {
            //TODO: Optimize to use classes of functions loaded by Dependency Injection
            switch (Op)
            {
                case "$OP.SUM":
                    {
                        _result = leftExpression!._Evaluate(expression) + rightExpression!._Evaluate(expression);
                        break;
                    }
                case "$OP.MULTIPLY":
                    {
                        _result = leftExpression!._Evaluate(expression) * rightExpression!._Evaluate(expression);
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
        }

        return _result;
    }
    private bool _IsThirdLevelParam()
    {
        if (Value == null)
            return false;
        return Regex.Matches(Value, regexParam3).Count > 0;
    }
    private bool _IsSecondLevelParam()
    {
        if (Value == null)
            return false;
        return Regex.Matches(Value, regexParam2).Count > 0;
    }
    private bool _IsFirstLevelParam()
    {
        if (Value == null)
            return false;
        return Regex.Matches(Value, regexParam1).Count > 0;
    }

}