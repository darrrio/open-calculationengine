using System.Collections;

namespace Open.Billing.Utility
{
    public static class Extensions
    {
        public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
            where K : notnull
        {
            return table
              .Cast<DictionaryEntry>()
              .ToDictionary(kvp => (K)kvp.Key, kvp => (V)kvp.Value!);
        }
    }
}