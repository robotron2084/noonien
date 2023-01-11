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

    private DataModel _parent;
    public DataModel Parent
    {
      get
      {
        return _parent;
      }
      set
      {
        if (_parent == value)
        {
          return;
        }
        if (_parent != null)
        {
          _parent.RemoveChild(this);
        }
        _parent = value;
      }
    }

    private void RemoveChild(DataModel dataModel)
    {
      _children.Remove(dataModel);
    }

    private List<DataModel> _children = new List<DataModel>();

    public int ChildrenCount
    {
      get
      {
        return _children.Count;
      }
    }

    private Dictionary<Type, DataElement> _elementsMap = new Dictionary<Type, DataElement>();

    public int ElementsCount
    {
      get
      {
        return _elementsMap.Count;
      }
    }


    private static ILogger _logger = new Logger(Debug.unityLogger.logHandler);

    public DataModel()
    {
      _name = CreateName();
    }

    public DataModel(string name)
    {
      _name = name;
    }

    public DataModel(params DataElement[] elements)
    {
      _name = CreateName();
      AddElementsInternal(_elementsMap, elements);
    }

    public DataModel(string name, params DataElement[] elements)
    {
      AddElementsInternal(_elementsMap, elements);
    }

    private string _name;
    public string Name
    {
      get => _name;
      set => _name = value;
    }

    public DataModel GetChildAt(int index)
    {
      return _children[index];
    }

    public void AddChild(DataModel model)
    {
      _children.Add(model);
      model.Parent = this;
    }

    public T GetElement<T>() where T : DataElement
    {
      return (T)_elementsMap[typeof(T)];
    }

    public void AddElement(DataElement element)
    {
      AddElementInternal(_elementsMap, element);
    }

    public static string CreateName()
    {
      return $"Model {__index++}";
    }

    private static void AddElementsInternal(
      Dictionary<Type, DataElement> map, 
      IEnumerable<DataElement> newElements)
    {
      foreach (var dataElement in newElements)
      {
        AddElementInternal(map, dataElement);
      }
    }

    private static void AddElementInternal(
      Dictionary<Type, DataElement> map,
      DataElement element)
    {
      var t = element.GetType();
      if (map.ContainsKey(t))
      {
        _logger.LogError(LogTag, $"{t.ToString()} is already registered on model {element.Name}");
        return;
      }
      map[t] = element;
    }

  }
}