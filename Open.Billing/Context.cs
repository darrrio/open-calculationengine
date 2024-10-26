
using static Open.Billing.Utility.Extensions;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Open.Billing;
public class Context
{
    private JsonObject Data { get; set; }
    public Dictionary<string, dynamic> FlatData
    {
        get
        {
            return FlatDictionary(Data);
        }
    }
    public Context(JsonObject baseData)
    {
        Data = baseData;
    }
    public decimal GetTariff()
    {
        return JsonSerializer.Deserialize<decimal>(FlatData["$PRM3.TARIFF"]);
    }
    

}