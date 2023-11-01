using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Open.Billing.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly ILogger<ConfigurationController> _logger;

    public ConfigurationController(ILogger<ConfigurationController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get the billing configuration
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<BillConfiguration> Get()
    {
        string fileName = "configuration.json";
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
        return new List<BillConfiguration>() { JsonSerializer.Deserialize<BillConfiguration>(output)! };
    }

    /// <summary>
    /// Load the billing configuration
    /// </summary>
    /// <param name="configuration"></param>
    [HttpPut]
    public async Task PutAsync([FromBody] BillConfiguration configuration)
    {
        string fileName = "configuration.json";
        string jsonString = JsonSerializer.Serialize(configuration);
        using (StreamWriter outputFile = new StreamWriter(Path.Combine("./", fileName)))
        {
            await outputFile.WriteAsync(jsonString);
        }
    }
}

public class BillConfiguration
{
    public string? formula { get; set; }
}