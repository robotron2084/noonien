using System;
using System.Collections.Generic;
using com.enemyhideout.soong;
using UnityEngine;

namespace com.enemyhideout.soong
{
  public class ElementObserver : MonoBehaviour
  {
  }

  public class ElementObserver<T> : ElementObserver where T : DataElement
  {
    // The element we're interested in. It is always assumed there is at least one element being looked at.
    protected T _element;
    // The element's data model.
    protected DataEntity Entity;
    // The data source we use to query for the model.
    protected EntitySource _source;
    // A list of observers that listen to various elements. By default we just listen to one.
    protected List<DataElementObserver<T>> _observers = new List<DataElementObserver<T>>();
    protected IEntityObserver _entityObserver;

    protected virtual void Awake()
    {
      _entityObserver = new EntityObserver(EntityUpdated);
      // initialize our observer to listen to the element type.
      Listen<T>(DataUpdated);
      InitSource();
    }

    public void InitSource()
    {
      if (_source != null)
      {
        return;
      }
      _source = GetComponentInParent<EntitySource>();
      if (_source != null)
      {
        _source.ObserveModel(_entityObserver);
      }
    }

    protected virtual void Start()
    {
      InitSource();
    }

    public void EntityUpdated(DataEntity entity)
    {
      if (entity == Entity)
      {
        return;
      }
      Entity = entity;
      foreach (var dataObserver in _observers)
      {
        if (dataObserver.Element != null)
        {
          DataRemoved(dataObserver.Element);
          dataObserver.RemoveObserver();
        }
        
        if (entity != null)
        {
          var element = entity.GetElement<T>();
          if (element != null)
          {
            DataAdded(element);
            dataObserver.AddObserver(element);
            dataObserver.DataUpdated(element);
          }
        }
      }
    }

    protected virtual void DataAdded(T instance)
    {
      _element = instance;
    }
    
    protected virtual void DataUpdated(T instance)
    {
      // do stuff.
    }
    
    protected virtual void DataRemoved(T instance)
    {
      _element = null;
    }


    protected void Listen<TDataElement>(Action<T> callback) where TDataElement : DataElement
    {
      var observer = new DataElementObserver<T>(callback);
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

      Entity = null;
      if (_source != null)
      {
        _source.RemoveObserver(_entityObserver);
      }
    }
  }
}