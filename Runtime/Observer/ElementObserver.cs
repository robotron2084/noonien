using System;
using System.Collections.Generic;
using com.enemyhideout.noonien;
using UnityEngine;

namespace com.enemyhideout.noonien
{
  public class ElementObserver : MonoBehaviour
  {
  }

  public class ElementObserver<T> : ElementObserver where T : Element
  {
    // The element we're interested in. It is always assumed there is at least one element being looked at.
    protected T _element;
    // The element's data model.
    protected Node Node;
    // The data source we use to query for the model.
    protected NodeProvider _source;
    // A list of observers that listen to various elements. By default we just listen to one.
    protected List<DataElementObserver> _observers = new List<DataElementObserver>();
    protected INodeObserver _nodeObserver;

    protected virtual void Awake()
    {
      _nodeObserver = new NodeObserver(NodeUpdated);
      // initialize our observer to listen to the element type.
      Observe<T>(DataUpdated, DataAdded, DataRemoved);
      InitSource();
    }

    public void InitSource()
    {
      if (_source != null)
      {
        return;
      }
      _source = GetComponentInParent<NodeProvider>();
      if (_source != null)
      {
        _source.ObserveModel(_nodeObserver);
      }
    }

    protected virtual void Start()
    {
      InitSource();
    }

    public void NodeUpdated(Node node)
    {
      if (node == Node)
      {
        return;
      }
      Node = node;
      foreach (var dataObserver in _observers)
      {
        dataObserver.NodeUpdated(Node);
      }
    }

    protected virtual void DataAdded(T element)
    {
      _element = element;
    }
    
    protected virtual void DataUpdated(T element)
    {
      // do stuff.
    }
    
    protected virtual void DataRemoved(T element)
    {
      _element = null;
    }


    protected void Observe<TDataElement>(Action<TDataElement> updated, Action<TDataElement> added=null, Action<TDataElement> removed=null) where TDataElement : Element
    {
      var observer = new DataElementObserver<TDataElement>(updated, added, removed) ;
      _observers.Add(observer);
    }

    //maaaaybe?
    protected void ListenToCollection<TCollection>(Action<List<CollectionChange<T>>> callback)
    {
      
    }
    
    protected void OnDestroy()
    {
      foreach (var dataObserver in _observers)
      {
        dataObserver.Destroy();
      }

      _observers.Clear();

      Node = null;
      if (_source != null)
      {
        _source.RemoveObserver(_nodeObserver);
      }
    }
  }
}