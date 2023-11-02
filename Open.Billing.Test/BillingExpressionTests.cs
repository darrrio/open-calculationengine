using System.Text.Json;
using System.Text.Json.Nodes;

namespace Open.Billing.Test;

public class BillingExpressionTests
{
    [Theory(Timeout = 1000)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_ParameterExpression_InputExpression, TestHelper.Evaluate_ParameterExpression_Result)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_SimpleExpression_InputExpression, TestHelper.Evaluate_SimpleExpression_Result)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_ComplexExpression_InputExpression, TestHelper.Evaluate_ComplexExpression_Result)]
    public void Evaluate(string inputData, string inputExpression, double expected)
    {
        //Arrange
        JsonObject data = JsonSerializer.Deserialize<JsonObject>(inputData)!;
        Context context = new Context(data);
        BillingExpression billingNode = new BillingExpression(context);

        //Act
        double result = (double)billingNode.Evaluate(inputExpression);
        
        //Assert
        Assert.Equal(expected, result);
    }
    
    [Theory(Timeout = 1000)]
    [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_ParameterExpression_InputExpression, TestHelper.Evaluate_ParameterExpression_Result)]
    // [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_SimpleExpression_InputExpression, TestHelper.Evaluate_SimpleExpression_Result)]
    // [InlineData(TestHelper.Evaluate_InputData, TestHelper.Evaluate_ComplexExpression_InputExpression, TestHelper.Evaluate_ComplexExpression_Result)]
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
    public void Evaluate_WithNullExpression_ReturnsZero()
    {
        //Arrange
        Context context = new Context(new JsonObject());
        BillingExpression billingNode = new BillingExpression(context);

        //Act
        double result = (double)billingNode.Evaluate(null);

        //Assert
        Assert.Equal(0, result);
    }
    // [Fact(Timeout = 1000)]
    // public void Evaluate_WithEmptyExpression_ReturnsZero()
    // {
    //     //Arrange
    //     Context context = new Context(new JsonObject());
    //     BillingExpression billingNode = new BillingExpression(context);

    //     //Act
    //     double result = (double)billingNode.Evaluate("");

    //     //Assert
    //     Assert.Equal(0, result);
    // }
    // [Fact(Timeout = 1000)]
    // public void Evaluate_WithInvalidExpression_ThrowsException()
    // {
    //     //Arrange
    //     Context context = new Context(new JsonObject());
    //     BillingExpression billingNode = new BillingExpression(context);

    //     //Act
    //     Action act = () => billingNode.Evaluate("Invalid Expression");

    //     //Assert
    //     Assert.Throws<Exception>(act);
    // }
}