using System;
using System.Collections.Generic;

namespace com.enemyhideout.soong
{
  public class EntityManager
  {

    public DataEntity Root;

    private Dictionary<string, DataEntity> _cachedEntityLookup = new Dictionary<string, DataEntity>();
    private Dictionary<string, ICollection> _cachedCollectionLookup = new Dictionary<string, ICollection>();
    private Dictionary<string, DataElement> _cachedElementLookup = new Dictionary<string, DataElement>();

    public DataEntity Add(string lookup, DataEntity parent=null)
    {
      if (parent == null)
      {
        parent = Root;
      }
      var entity = parent.AddNewChild(lookup);
      Register(lookup, entity);
      return entity;
    }
    
    public DataEntity Get(string lookup)
    {
      return _cachedEntityLookup.ValueOrDefault(lookup);
    }
    
    public T Get<T>(string lookup) where T : DataElement
    {
      return _cachedElementLookup.ValueOrDefault(lookup) as T;
    }

    public ICollection<DataEntity> GetCollection(string lookup)
    {
      return _cachedCollectionLookup.ValueOrDefault(lookup) as ICollection<DataEntity>;
    }

    public ICollection<T> GetCollection<T>(string lookup)
    {
      return _cachedCollectionLookup.ValueOrDefault(lookup) as ICollection<T>;
    }

    public void Register(string lookup, DataEntity entity)
    {
      RegisterInternal(_cachedEntityLookup, lookup, entity);
    }
    
    public void Register(string lookup, ICollection collection)
    {
      RegisterInternal(_cachedCollectionLookup, lookup, collection);
    }

    public void Register(string lookup, DataElement element)
    {
      RegisterInternal(_cachedElementLookup, lookup, element);
    }

    private static void RegisterInternal<K, V>(Dictionary<K, V> dictionary, K key, V value)
    {
      if (dictionary.ContainsKey(key))
      {
        throw new ArgumentException($"Value already registered for {key}");
      }
      dictionary[key] = value;
      
    }
    
    public EntityManager(DataEntity root)
    {
      Root = root;
    }

    public DataEntity Find(string query)
    {
      return FindInternal(query, Root);
    }

    public static DataEntity FindInternal(string query, DataEntity root)
    {
      var entityTokens = query.Split(".");
      var searchItem = root;
      if (searchItem.Name != entityTokens[0])
      {
        return null;
      }
      if (entityTokens.Length == 1)
      {
        return searchItem;
      }
      var tokenIndex = 1;
      int iteration = 0;
      int maxIterations = 10;
      while (true && iteration < maxIterations)
      {
        var searchString = entityTokens[tokenIndex];
        bool itemFound = false;
        foreach (var child in searchItem.Children)
        {
          if (child.Name == searchString)
          {
            searchItem = child;
            itemFound = true;
            tokenIndex++;
            if (tokenIndex == entityTokens.Length)
            {
              return searchItem;
            }
            break;
          }
        }

        if (!itemFound)
        {
          return null;
        }
        iteration++;
      }
      return null;
    }
  }
}