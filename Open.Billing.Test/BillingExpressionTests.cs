using System.Text.Json;
using System.Text.Json.Nodes;

namespace Open.Billing.Test;

public class BillingExpressionTests
{
    [Theory(Timeout = 1000)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_ParameterExpression_InputExpression, TestHelper.Evaluate_ParameterExpression_Result)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_SimpleExpression_InputExpression, TestHelper.Evaluate_SimpleExpression_Result)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_ComplexExpression_InputExpression, TestHelper.Evaluate_ComplexExpression_Result)]
    public void EvaluateComplex(string inputData, string inputExpression, double expected)
    {
        //Arrange
        JsonObject data = JsonSerializer.Deserialize<JsonObject>(inputData)!;
        Context context = new Context(data);
        BillingExpression billingNode = new BillingExpression(context);

        //Act
        double result = (double)billingNode.EvaluateComplex(inputExpression);
        
        //Assert
        Assert.Equal(expected, result);
    }

    [Fact(Timeout = 1000)]
    public void EvaluateComplex_WithNullExpression_ReturnsZero()
    {
        //Arrange
        Context context = new Context(new JsonObject());
        BillingExpression billingNode = new BillingExpression(context);

        //Act
        double result = (double)billingNode.EvaluateComplex(null);

        //Assert
        Assert.Equal(0, result);
    }
}