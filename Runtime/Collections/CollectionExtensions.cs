using System.Collections.Generic;

namespace com.enemyhideout.noonien
{
  public static class CollectionExtensions
  {
    public static V ValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key)
    {
      var retVal = default(V);
      dictionary.TryGetValue(key, out retVal);
      return retVal;
    }
  }
}