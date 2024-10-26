
using static Open.CalculationEngine.Utility.Extensions;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Open.CalculationEngine;
public class Context
{
    private JsonObject? Data { get; set; } = null;
    public Dictionary<string, dynamic> FlatData
    {
        get
        {
            if(Data == null)
            {
                throw new Exception("Data is not set");
            }
            return FlatDictionary(Data);
        }
    }
    public Context()
    {
    }
    public void SetData(JsonObject baseData)
    {
        Data = baseData;
    }
    public decimal GetTariff()
    {
        return JsonSerializer.Deserialize<decimal>(FlatData["$PRM3.TARIFF"]);
    }
    

}