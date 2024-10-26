
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Open.Billing;
public class BillingExpression
{
    public BillingExpression? LeftExpression;
    public BillingExpression? RightExpression;
    public List<BillingExpression> Childrens { get; set; } = new List<BillingExpression>();
    private readonly Context _context;
    private decimal _result;
    private string? Value { get; set; }

    public BillingExpression(Context context)
    {
        _context = context;
    }
    public void SetValue(string value)
    {
        Value = value;
    }
    public decimal EvaluateComplex(string? expression = null)
    {
        if (expression == null)
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
}