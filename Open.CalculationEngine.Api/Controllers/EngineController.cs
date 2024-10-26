using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace Open.CalculationEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EngineController(ILogger<EngineController> logger, Context context, ExpressionNode billingExpression) : ControllerBase
{
    /// <summary>
    /// Calculate expression from input file
    /// </summary>
    /// <param name="expression">Expression formula to calculate</param>
    /// <param name="inputFile">Base data used for calculation</param>
    /// <response code="200">Returns result of formula expression</response>
    /// <response code="400">Returns if expression or formula are not valid</response>
    [HttpPost("CalculateExpression")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public decimal CalculateExpression(string expression, [FromBody] Hashtable inputFile)
    {
        logger.LogDebug($"Expression : {expression}");
        string jsonString = JsonSerializer.Serialize(inputFile);
        JsonObject data = JsonSerializer.Deserialize<JsonObject>(jsonString)!;
        context.SetData(data);
        billingExpression.SetContext(context);
        ExpressionNode billingNode = billingExpression;
        return billingNode.Evaluate(expression);
    }
}
