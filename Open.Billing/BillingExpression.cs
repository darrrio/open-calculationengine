
using System.Text.Json;
using System.Text.RegularExpressions;

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
    public BillingExpression(Context context)
    {
        _context = context;
    }
    public void SetValue(string value)
    {
        _value = value;
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
}