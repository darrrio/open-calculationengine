using System.Collections;
using System.Text;
using System.Text.Json;

namespace Open.Billing.Test;

public class BillingExpressionTests
{
    [Theory]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_ParameterExpression_InputExpression,TestHelper.Evaluate_ParameterExpression_Result)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_SimpleExpression_InputExpression,TestHelper.Evaluate_SimpleExpression_Result)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_ComplexExpression_InputExpression,TestHelper.Evaluate_ComplexExpression_Result)]
    public void Evaluate(string inputData, string inputExpression, string expected)
    {
        //Arrange
        Hashtable data = JsonSerializer.Deserialize<Hashtable>(inputData)!;
        Context context = new Context(data);
        BillingExpression billingNode = new BillingExpression(context);

        //Act
        decimal result = billingNode.Evaluate(inputExpression);

        //Assert
        Assert.Equal(expected, result.ToString());
    }
}