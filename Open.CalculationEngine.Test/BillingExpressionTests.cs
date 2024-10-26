using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;

namespace Open.CalculationEngine.Test;

public class BillingExpressionTests
{
    private ServiceProvider services;
    private Context context;
    private ExpressionNode billingNode;
    public BillingExpressionTests()
    {
        services = new ServiceCollection()
            .AddLogging()
            .AddSingleton<Context>()
            .AddSingleton<ExpressionNode>()
            .BuildServiceProvider();
        context = services.GetRequiredService<Context>();
        billingNode = services.GetRequiredService<ExpressionNode>();
    }

    [Theory(Timeout = 1000)]
    [InlineData(TestHelper.EvaluateInputData, TestHelper.EvaluateParameterExpressionInputExpression, TestHelper.EvaluateParameterExpressionResult)]
    [InlineData(TestHelper.EvaluateInputData, TestHelper.EvaluateSimpleExpressionInputExpression, TestHelper.EvaluateSimpleExpressionResult)]
    [InlineData(TestHelper.EvaluateInputData, TestHelper.EvaluateComplexExpressionInputExpression, TestHelper.EvaluateComplexExpressionResult)]
    public void EvaluateComplex(string inputData, string inputExpression, double expected)
    {
        //Arrange
        JsonObject data = JsonSerializer.Deserialize<JsonObject>(inputData)!;
        context.SetData(data);
        billingNode.SetContext(context);

        //Act
        double result = (double)billingNode.Evaluate(inputExpression);
        
        //Assert
        Assert.Equal(expected, result);
    }

    [Fact(Timeout = 1000)]
    public void EvaluateComplex_WithNullExpression_ReturnsZero()
    {
        //Arrange
        context.SetData(new JsonObject());
        billingNode.SetContext(context);

        //Act
        double result = (double)billingNode.Evaluate(null);

        //Assert
        Assert.Equal(0, result);
    }
}