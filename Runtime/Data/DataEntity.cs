using System;
using System.Collections.Generic;
using System.Linq;
using com.enemyhideout.soong.Reflection;
using UnityEngine;

namespace com.enemyhideout.soong
{
  public class DataEntity
  {
    private static TypeCache<DataElement> _typeCache = new TypeCache<DataElement>();
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
    }

    private Dictionary<Type, DataElement> _elementsMap = new Dictionary<Type, DataElement>();

    public override string ToString()
    {
      return $"[DataEntity {Name}]";
    }


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
    
    public DataEntity(INotifyManager notifyManager, DataEntity parent) : this(notifyManager)
    {
      parent.AddChild(this);
    }

    public DataEntity(INotifyManager notifyManager, string name)
    {
      _notifyManager = notifyManager;
      _name = name;
    }

    public DataEntity(INotifyManager notifyManager, string name, DataEntity parent) : this(notifyManager, name)
    {
      parent.AddChild(this);
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

    public void InsertChild(int index, DataEntity child)
    {
      if (child._parent != null)
      {
        child.RemoveParent();
      }
      child._parent = this;
      InsertChildInternal(index, child);
    }
    
    public void RemoveChild(DataEntity child)
    {
      if (child._parent == this)
      {
        DetachChildInternal(child);
        child._parent = null;
      }
    }

    public void AddChild(DataEntity child)
    {
      if (child == this)
      {
        throw new ArgumentException("Cannot add child to self.");
      }
      if (child._parent != null)
      {
        child.RemoveParent();
      }
      child._parent = this;
      AttachChildInternal(child);
    }
    
    private void InsertChildInternal(int index, DataEntity child)
    {
      if (child.Parent != this)
      {
        throw new Exception("Attempting to attach child to an entity who is not the parent. This method should only be called internally by children.");
      }
      LazyInitChildren();
      _children.InsertChild(index, child);
    }

    private void AttachChildInternal(DataEntity child)
    {
      if (child.Parent != this)
      {
        throw new Exception("Attempting to attach child to an entity who is not the parent. This method should only be called internally by children.");
      }
      LazyInitChildren();
      _children.AddChild(child);
    }
    
    private void DetachChildInternal(DataEntity dataEntity)
    {
      if (dataEntity.Parent != this)
      {
        throw new Exception("Attempting to detach child from an entity who is not the parent.");
      }
      LazyInitChildren();
      _children.RemoveChild(dataEntity);
    }

    public T GetElement<T>() where T : class
    {
      DataElement dataElement = null;
      _elementsMap.TryGetValue(typeof(T), out dataElement);
      return dataElement as T;
    }
    
    public void AddElement(DataElement element)
    {
      AddElementInternal(_elementsMap, element, _typeCache);
    }

    public static string CreateName()
    {
      return $"Model {__index++}";
    }

    private static void AddElementsInternal(
      Dictionary<Type, DataElement> map, 
      IEnumerable<DataElement> newElements,
      TypeCache<DataElement> typeCache)
    {
      foreach (var dataElement in newElements)
      {
        AddElementInternal(map, dataElement, typeCache);
      }
    }

    private static void AddElementInternal(Dictionary<Type, DataElement> map,
      DataElement element, 
      TypeCache<DataElement> typeCache)
    {
      IEnumerable<Type> types = typeCache.GetTypes(element.GetType());
      foreach (var type in types)
      {
        map[type] = element;
      }
    }

    public void RemoveParent()
    {
      if (_parent != null)
      {
        _parent.RemoveChild(this);
      }
    }
  }
}