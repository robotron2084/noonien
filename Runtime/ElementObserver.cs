using System;
using System.Collections.Generic;
using com.enemyhideout.soong;
using PlasticGui.Diff.Annotate;
using UnityEngine;

namespace DefaultNamespace
{
  public class ElementObserver : MonoBehaviour
  {
  }

  public class ElementObserver<T> : ElementObserver, IEntityObserver where T : DataElement
  {
    // The element we're interested in. It is always assumed there is at least one element being looked at.
    protected T _element;
    // The element's data model.
    protected DataEntity Entity;
    // The data source we use to query for the model.
    protected DataSource _source;
    // A list of observers that listen to various elements. By default we just listen to one.
    protected List<DataObserver<T>> _observers = new List<DataObserver<T>>();

    protected virtual void Awake()
    {
      Listen<T>(DataUpdated);
    }

    protected virtual void Start()
    {
      _source = GetComponentInParent<DataSource>();
      if (_source != null)
      {
        _source.ObserveModel(this);
      }
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
          dataObserver.RemoveObserver(dataObserver);
        }
        
        if (entity != null)
        {
          var element = entity.GetElement<T>();
          if (element != null)
          {
            DataAdded(element);
            dataObserver.AddObserver(element);
            dataObserver.ElementUpdated(element);
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


    private void Listen<TDataElement>(Action<T> callback) where TDataElement : DataElement
    {
      var observer = new DataObserver<T>(callback);
      _observers.Add(observer);
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
        _source.RemoveObserver(this);
      }
    }
  }
}