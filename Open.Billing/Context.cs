
using static Open.Billing.Utility.Extensions;
using System.Collections;
using System.Text.Json;

namespace Open.Billing;
public class Context
{
    public Hashtable data { get; set; }
    public Context(Hashtable baseData)
    {
        data = baseData;
    }
    public decimal GetTariff()
    {
        var dict = HashtableToDictionary<string, dynamic>(data);
        return JsonSerializer.Deserialize<decimal>(dict["SERVICES"][0].GetProperty("BILLINGUNITS")[0].GetProperty("$PRM3.TARIFF"));
    }
    

}