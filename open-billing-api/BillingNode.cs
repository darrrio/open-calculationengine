
using System.Text.RegularExpressions;

public class BillingNode
{
    public BillingNode leftNode;
    public BillingNode rightNode;
    private Context _context;
    private string _value;
    public string value { get { return _value; } }
    private string _operator;
    public string op { get { return _operator; } }
    private decimal _result;
    private static string regex = "(\\$[A-z]+.[A-z]+)";
    private static string regexFirst = "(?:\\(\\$.+;\\$)";
    private static string regexFirst2 = "(?:\\(\\$.+;)";
    private static string regexFirst3 = "(?:\\$PRM.*[^)])";
    private static string regexSecond = "(?:;[^)]+\\$.+)";
    private static string regexSecond2 = "(?:;+\\$.+)";
    private static string regexParam1 = "(?:^\\$PRM1)";
    private static string regexParam2 = "(?:^\\$PRM2)";
    private static string regexParam3 = "(?:^\\$PRM3)";
    public BillingNode(Context context)
    {
        _context = context;
        leftNode = new BillingNode(context);
        rightNode = new BillingNode(context);
        _value = string.Empty;
        _operator = string.Empty;
    }
    public bool isThirdLevelParam()
    {
        return Regex.Matches(value,regexParam3).Count > 0;
    }
    public bool isSecondLevelParam()
    {
        return Regex.Matches(value,regexParam2).Count > 0;
    }
    public bool isFirstLevelParam()
    {
        return Regex.Matches(value,regexParam1).Count > 0;
    }
    public bool isLeaf()
    {
        return _value == null;
    }
    public void setFormula(string formula)
    {
        _value = formula;
    }
    public bool scanFormula()
    {
        if (!(isLeaf() || isThirdLevelParam() || isSecondLevelParam() || isFirstLevelParam()))
        {
            _operator = Regex.Matches(value,regex).First().Value;
            leftNode = new BillingNode(_context);
            rightNode = new BillingNode(_context);
            leftNode.setFormula(Regex.Matches(value,regexFirst).First().Value[1..^2] ?? Regex.Matches(_value,regexFirst2).First().Value[1..0] ?? Regex.Matches(_value,regexFirst3).First().Value);
            rightNode.setFormula(Regex.Matches(value,regexSecond).First().Value[1..^1] ?? Regex.Matches(_value,regexSecond2).First().Value[1..^1]);
            leftNode.scanFormula();
            rightNode.scanFormula();
        }
        return isLeaf();
    }
    public decimal execFormula()
    {
        switch (op)
        {
            case "$OP.SUM":
                {
                    _result = leftNode.interpret() + rightNode.interpret();
                    break;
                }
            case "$OP.MULTIPLY":
                {
                    _result = leftNode.interpret() * rightNode.interpret();
                    break;
                }
            case "$FN.TARIFF":
                {
                    _result = _context.GetTariff();
                    break;
                }
            // case "$FN.SEASONCOUNTDAYS":
            //     {
            //         this._result = this._context.countSeasonDays(this.leftNode.interpret());
            //         break;
            //     }
            default:
                {
                    _result = 0;
                    break;
                }
        }
        return _result;
    }
    public decimal interpret()
    {

        if (isFirstLevelParam())
        {
            _operator = _value;
            _result = (decimal)_context.data[_value]!;
        }
        else if (isSecondLevelParam())
        {
            _operator = _value;
            _result = (decimal)_context.data[_value]!;
        }
        else if (isThirdLevelParam())
        {
            _operator = _value;
            _result = (decimal)_context.data[_value]!;
        }
        else
        {
            _result = execFormula();
        }

        return _result;
    }
}