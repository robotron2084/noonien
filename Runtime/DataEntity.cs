using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.enemyhideout.soong
{
  public class DataEntity
  {
    private readonly INotifyManager _notifyManager;

    public INotifyManager NotifyManager => _notifyManager;

    public static int __index = 0;
    public static string LogTag = "Soong";

    private EntityCollection _children;

    public IEntityCollection Children
    {
      get
      {
        LazyInitChildren();
        return _children;
      }
    }

    private DataEntity _parent;
    public DataEntity Parent
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
          _parent.DetachChildInternal(this);
        }
        _parent = value;
        if (_parent != null)
        {
          _parent.AttachChildInternal(this);
        }

      }
    }

    private Dictionary<Type, DataElement> _elementsMap = new Dictionary<Type, DataElement>();

    
    /// <summary>
    /// Better than calling Children.Count because it won't alloc children.
    /// </summary>
    public int ChildrenCount
    {
      get
      {
        if (_children == null)
        {
          return 0;
        }
        return _children.Count;
      }
    }

    public int ElementsCount
    {
      get
      {
        return _elementsMap.Count;
      }
    }

    private static ILogger _logger = new Logger(Debug.unityLogger.logHandler);

    public DataEntity(INotifyManager notifyManager)
    {
      _notifyManager = notifyManager;
      _name = CreateName();
    }

    public DataEntity(INotifyManager notifyManager, string name)
    {
      _notifyManager = notifyManager;
      _name = name;
    }

    public DataEntity(INotifyManager notifyManager, params DataElement[] elements)
    {
      _notifyManager = notifyManager;
      _name = CreateName();
      AddElementsInternal(_elementsMap, elements);
    }

    public DataEntity(INotifyManager notifyManager, string name, params DataElement[] elements)
    {
      _notifyManager = notifyManager;
      AddElementsInternal(_elementsMap, elements);
    }

    private string _name;
    public string Name
    {
      get => _name;
      set => _name = value;
    }

    public DataEntity GetChildAt(int index)
    {
      if (_children == null)
      {
        throw new ArgumentOutOfRangeException($"{Name} has no children.");
      }
      return Children[index];
    }

    private void LazyInitChildren()
    {
      if (_children == null)
      {
        _children = new EntityCollection(_notifyManager);
      }
    }

    public void AttachChildInternal(DataEntity entity)
    {
      if (entity.Parent != this)
      {
        throw new Exception("Attempting to attach child to an entity who is not the parent. This method should only be called internally by children.");
      }
      LazyInitChildren();
      _children.AddChild(entity);
    }
    
    public void DetachChildInternal(DataEntity dataEntity)
    {
      if (dataEntity.Parent != this)
      {
        throw new Exception("Attempting to destach child from an entity who is not the parent.");
      }
      LazyInitChildren();
      _children.RemoveChild(dataEntity);
    }

    public T GetElement<T>() where T : DataElement
    {
      return (T)_elementsMap[typeof(T)]; // todo: handle subtypes, maybe we need to iterate over this as a list.
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