using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Open.CalculationEngine;
public class ExpressionNode(ILogger<ExpressionNode> logger)
{
    private readonly ILogger<ExpressionNode> _logger = logger;
    public List<ExpressionNode> Children { get; set; } = new List<ExpressionNode>();
    private Context? _context = null;
    private decimal _result;
    private string? Value { get; set; }

    public void SetContext(Context context)
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

        var billingExpression = Parse(expression);
        _logger.LogDebug($"Expression: {expression}");  
        return billingExpression._EvaluateComplex();
    }
    private decimal _EvaluateComplex()
    {
        _logger.LogDebug($"Expression: {Value}");  
        _logger.LogDebug($"Children: {Children.ToArray()}");  

        if (Children.Count == 0)
        {
            _result = JsonSerializer.Deserialize<decimal>(_context!.FlatData[Value!]);
        }

        switch (Value)
        {
            case "$OP.SUM":
                {
                    _result = Children.Sum(child => child._EvaluateComplex());
                    _logger.LogDebug($"Value: {Value}, Result: {_result}");  
                    break;
                }
            case "$OP.MULTIPLY":
                {
                    _result = Children.Aggregate(1m, (acc, child) => acc * child._EvaluateComplex());
                    _logger.LogDebug($"Value: {Value}, Result: {_result}");  
                    break;
                }
            case "$FN.TARIFF":
                {
                    _result = _context!.GetTariff();
                    _logger.LogDebug($"Value: {Value}, Result: {_result}");  
                    break;
                }
            default:
                {
                    _result = JsonSerializer.Deserialize<decimal>(_context!.FlatData[Value!]);
                    _logger.LogDebug($"Value: {Value}, Result: {_result}");  
                    break;
                }
        }
        return _result;
    }

    public ExpressionNode Parse(string expression)
    {
        ExpressionNode billingExpression = new ExpressionNode(_logger);
        billingExpression.SetContext(_context!);
        _logger.LogInformation($"Parsing expression: {expression}");

        if (expression == null || expression == "")
        {
            return billingExpression!;
        }

        var match = Regex.Match(expression, @"(\$[A-z]+\.[A-z]+)\((.*)\)");
        if (match.Success)
        {
            billingExpression!.SetValue(match.Groups[1].Value);
            var parameters = SplitParameters(match.Groups[2].Value);
            billingExpression.Children = parameters.Select(Parse).ToList();

            _logger.LogInformation($"Expression parsed: {expression}, Value: {match.Groups[1].Value}, Parameters: {parameters}");
        }
        else
        {
            billingExpression!.SetValue(expression);
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