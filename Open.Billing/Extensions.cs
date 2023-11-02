using System.Collections;
using System.Text.Json.Nodes;

namespace Open.Billing.Utility
{
    public static class Extensions
    {
        public static Dictionary<K, V> JsonObjectToDictionary<K, V>(JsonObject table)
            where K : notnull
        {
            return table
              .Cast<DictionaryEntry>()
              .ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value!);
        }

        public static Dictionary<string, dynamic> FlatDictionary(JsonObject dict, string prefix = "")
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();

            foreach (var pair in dict)
            {
                if (pair.Value is JsonObject nestedDict)
                {
                    foreach (var nestedPair in FlatDictionary(nestedDict, $"{prefix}{pair.Key}."))
                    {
                        result.Add(nestedPair.Key, nestedPair.Value);
                    }
                }
                else if (pair.Value is JsonArray nestedList)
                {
                    for(int i=0; i <nestedList.Count; i++)
                    {
                        if (nestedList[i] is JsonObject nestedDict2)
                        {
                            foreach (var nestedPair in FlatDictionary(nestedDict2, $"{prefix}{pair.Key}[{i}]."))
                            {
                                result.Add(nestedPair.Key, nestedPair.Value);
                            }
                        }
                        else
                        {
                            result.Add(pair.Key, nestedList[i]);
                        }
                    }
                }
                else
                {
                    result.Add(pair.Key, pair.Value);
                }
            }
            return result;
        }
    }
}