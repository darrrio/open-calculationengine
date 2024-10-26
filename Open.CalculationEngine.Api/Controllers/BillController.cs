using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace Open.CalculationEngine.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BillController(ILogger<BillController> logger, Context context, ExpressionNode billingExpression) : ControllerBase
{
    private readonly ILogger<BillController> _logger = logger;
    private readonly Context _context = context;
    private readonly ExpressionNode _billingExpression = billingExpression;

    /// <summary>
    /// Get a bill
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IEnumerable<object> Get(int id)
    {
        return new List<object>();
    }
    /// <summary>
    /// Generate a bill
    /// </summary>
    /// <param name="configuration"></param>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public decimal Post(string configuration)
    {
        string fileName = "data.json";
        string output = "";
        try
        {
            // Open the text file using a stream reader.
            using (var sr = new StreamReader(fileName))
            {
                output = sr.ReadToEnd();
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        JsonObject data = JsonSerializer.Deserialize<JsonObject>(output)!;
        _context.SetData(data);
        _billingExpression.SetContext(_context);
        ExpressionNode billingNode = _billingExpression;
        return billingNode.EvaluateComplex(configuration);
    }
    /// <summary>
    /// Delete a generated bill
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete]
    public void Delete(int id)
    {

    }
}
