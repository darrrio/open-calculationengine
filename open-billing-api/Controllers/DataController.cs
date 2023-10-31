using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace open_billing_api.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;

    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get the billing data
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<Hashtable> Get()
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
        return new List<Hashtable>() { JsonSerializer.Deserialize<Hashtable>(output)! };
    }

    /// <summary>
    /// Load the billing data
    /// </summary>
    /// <param name="configuration"></param>
    [HttpPut]
    public async Task PutAsync([FromBody] Hashtable configuration)
    {
        string fileName = "data.json";
        string jsonString = JsonSerializer.Serialize(configuration);
        using (StreamWriter outputFile = new StreamWriter(Path.Combine("./", fileName)))
        {
            await outputFile.WriteAsync(jsonString);
        }
    }
}


