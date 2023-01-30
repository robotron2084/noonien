using System;
using System.Collections.Generic;

namespace com.enemyhideout.noonien
{
  public class NodeManager
  {

    public Node Root;

    private Dictionary<string, Node> _cachedEntityLookup = new Dictionary<string, Node>();
    private Dictionary<string, ICollection> _cachedCollectionLookup = new Dictionary<string, ICollection>();
    private Dictionary<string, Element> _cachedElementLookup = new Dictionary<string, Element>();

    public Node Add(string lookup, Node parent=null)
    {
      if (parent == null)
      {
        parent = Root;
      }
      var entity = parent.AddNewChild(lookup);
      Register(lookup, entity);
      return entity;
    }
    
    public Node Get(string lookup)
    {
      return _cachedEntityLookup.ValueOrDefault(lookup);
    }
    
    public T Get<T>(string lookup) where T : Element
    {
      return _cachedElementLookup.ValueOrDefault(lookup) as T;
    }

    public ICollection<Node> GetCollection(string lookup)
    {
      return _cachedCollectionLookup.ValueOrDefault(lookup) as ICollection<Node>;
    }

    public ICollection<T> GetCollection<T>(string lookup)
    {
      return _cachedCollectionLookup.ValueOrDefault(lookup) as ICollection<T>;
    }

    public void Register(string lookup, Node entity)
    {
      RegisterInternal(_cachedEntityLookup, lookup, entity);
    }
    
    public void Register(string lookup, ICollection collection)
    {
      RegisterInternal(_cachedCollectionLookup, lookup, collection);
    }

    public void Register(string lookup, Element element)
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
    
    public NodeManager(Node root)
    {
      Root = root;
    }

    public Node Find(string query)
    {
      return FindInternal(query, Root);
    }

    public static Node FindInternal(string query, Node root)
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