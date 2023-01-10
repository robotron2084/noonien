using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.enemyhideout.soong
{
  public class DataModel
  {
    public static int __index = 0;
    public static string LogTag = "Soong";
    
    public DataModel Parent = null;
    public List<DataModel> Children = new List<DataModel>();
    public List<DataElement> Elements = new List<DataElement>();

    private Dictionary<Type, DataElement> _elementsMap = new Dictionary<Type, DataElement>();

    private static ILogger _logger = new Logger(Debug.unityLogger.logHandler);

    public DataModel()
    {
      _name = CreateName();
    }

    public DataModel(string name)
    {
      _name = name;
    }

    public DataModel(string name, params DataElement[] elements)
    {
      AddElementsInternal(_elementsMap, Elements, elements);
    }

    private string _name;
    public string Name
    {
      get => _name;
      set => _name = value;
    }

    public DataModel GetChildAt(int index)
    {
      return Children[index];
    }

    public T GetElement<T>() where T : DataElement
    {
      return (T)_elementsMap[typeof(T)];
    }

    public void AddElement(DataElement element)
    {
      AddElementInternal(_elementsMap, Elements, element);
    }

    public static string CreateName()
    {
      return $"Model {__index++}";
    }

    private static void AddElementsInternal(
      Dictionary<Type, DataElement> map, 
      List<DataElement> elementsList,
      IEnumerable<DataElement> newElements)
    {
      foreach (var dataElement in newElements)
      {
        AddElementInternal(map, elementsList, dataElement);
      }
    }

    private static void AddElementInternal(
      Dictionary<Type, DataElement> map,
      List<DataElement> elementsList,
      DataElement element)
    {
      var t = element.GetType();
      if (map.ContainsKey(t))
      {
        _logger.LogError(LogTag, $"{t.ToString()} is already registered on model {element.Name}");
        return;
      }
      map[t] = element;
      elementsList.Add(element);
    }
    
  }
}