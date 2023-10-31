using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace open_billing_api.Controllers;

[ApiController]
[Route("[controller]")]
public class BillController : ControllerBase
{
    private readonly ILogger<BillController> _logger;

    public BillController(ILogger<BillController> logger)
    {
        _logger = logger;
    }

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
        // return new List<Hashtable>() { JsonSerializer.Deserialize<Hashtable>(output)! };
        Hashtable data = JsonSerializer.Deserialize<Hashtable>(output)!;
        Context context = new Context(data);
        BillingNode billingNode = new BillingNode(context);
        billingNode.setFormula(configuration);
        billingNode.scanFormula();
        return billingNode.interpret();
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
