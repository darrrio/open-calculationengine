
using static Open.Billing.Utility.Extensions;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Open.Billing;
public class Context
{
    private JsonObject data { get; set; }
    public Dictionary<string, dynamic> Data
    {
        get
        {
            return data.ToDictionary(kvp => (string)kvp.Key, kvp => (dynamic)kvp.Value!);
        }
    }
    public Dictionary<string, dynamic> FlatData
    {
        get
        {
            return FlatDictionary(data);
        }
    }
    public Context(JsonObject baseData)
    {
        data = baseData;
    }
    public decimal GetTariff()
    {
        return JsonSerializer.Deserialize<decimal>(FlatData["$PRM3.TARIFF"]);
    }
    

}