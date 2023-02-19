using System;
using System.Collections.Generic;
using System.Linq;
using com.enemyhideout.noonien.Reflection;
using UnityEngine;

namespace com.enemyhideout.noonien
{
  /// <summary>
  /// A object of data, that contains other nodes and elements.
  /// </summary>
  public class Node
  {
    private static TypeCache<Element> _typeCache = new TypeCache<Element>();
    private readonly INotifyManager _notifyManager;

    public INotifyManager NotifyManager => _notifyManager;

    public static int __index = 0;
    public static string LogTag = "Soong";

    private NodeCollection _children;
    public ICollection<Node> Children
    {
      get
      {
        LazyInitChildren();
        return _children;
      }
    }

    private Node _parent;
    public Node Parent
    {
      get
      {
        return _parent;
      }
      set
      {
        // this is necessary for deserialization. Setting this outside of serialization won't do what you want.
        _parent = value;
      }
    }

    private Dictionary<Type, Element> _elementsMap = new Dictionary<Type, Element>();
    private List<Element> _elements = new List<Element>();

    public IReadOnlyCollection<Element> Elements
    {
      get => _elements;
    }

    public override string ToString()
    {
      return $"[Node {Name}]";
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
        return _elements.Count;
      }
    }

    private static ILogger _logger = new Logger(Debug.unityLogger.logHandler);
    
    public Node(INotifyManager notifyManager)
    {
      _notifyManager = notifyManager;
      _name = CreateName();
    }
    
    public Node(INotifyManager notifyManager, Node parent) : this(notifyManager)
    {
      parent.AddChild(this);
    }

    public Node(INotifyManager notifyManager, string name)
    {
      _notifyManager = notifyManager;
      _name = name;
    }


    public Node(INotifyManager notifyManager, string name, Node parent) : this(notifyManager, name)
    {
      parent.AddChild(this);
    }
    
    private string _name;
    public string Name
    {
      get => _name;
      set => _name = value;
    }

    public Node GetChildAt(int index)
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
        _children = new NodeCollection(_notifyManager);
      }
    }

    public void InsertChild(int index, Node child)
    {
      if (child._parent != null)
      {
        child.RemoveParent();
      }
      child._parent = this;
      InsertChildInternal(index, child);
    }
    
    public void RemoveChild(Node child)
    {
      if (child._parent == this)
      {
        DetachChildInternal(child);
        child._parent = null;
      }
    }

    public void AddChild(Node child)
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
    
    private void InsertChildInternal(int index, Node child)
    {
      if (child.Parent != this)
      {
        throw new Exception("Attempting to attach child to an entity who is not the parent. This method should only be called internally by children.");
      }
      LazyInitChildren();
      _children.InsertChild(index, child);
    }

    private void AttachChildInternal(Node child)
    {
      if (child.Parent != this)
      {
        throw new Exception("Attempting to attach child to an entity who is not the parent. This method should only be called internally by children.");
      }
      LazyInitChildren();
      _children.AddChild(child);
    }
    
    private void DetachChildInternal(Node node)
    {
      if (node.Parent != this)
      {
        throw new Exception("Attempting to detach child from an entity who is not the parent.");
      }
      LazyInitChildren();
      _children.RemoveChild(node);
    }

    public T AddElement<T>() where T : Element, new() 
    {
      var element = new T();
      element.Parent = this;
      return element;
    }

    public T Get<T>() where T : class
    {
      return GetElement<T>();
    }

    public T GetElement<T>() where T : class
    {
      Element element = null;
      _elementsMap.TryGetValue(typeof(T), out element);
      return element as T;
    }
    
    public void AddElement(Element element)
    {
      AddElementInternal(_elementsMap, element, _typeCache, _elements);
    }

    public static string CreateName()
    {
      return $"Model {__index++}";
    }

    private static void AddElementsInternal(
      Dictionary<Type, Element> map, 
      IEnumerable<Element> newElements,
      TypeCache<Element> typeCache,
      List<Element> elements)
    {
      foreach (var dataElement in newElements)
      {
        AddElementInternal(map, dataElement, typeCache, dataElements: elements);
      }
    }

    private static void AddElementInternal(Dictionary<Type, Element> map,
      Element element,
      TypeCache<Element> typeCache, List<Element> dataElements)
    {
      IEnumerable<Type> types = typeCache.GetTypes(element.GetType());
      foreach (var type in types)
      {
        map[type] = element;
      }

      if (!dataElements.Contains(element))
      {
        dataElements.Add(element);
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