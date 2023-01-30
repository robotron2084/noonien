using System;
using System.Collections.Generic;
using System.Linq;

namespace com.enemyhideout.soong
{
  public abstract class DataElement : IObservable<DataElement>
  {
    protected DataEntity _parent;
    private EventBuffer _eventBuffer;

    public int Version => _observable.Version;

    public virtual DataEntity Parent
    {
      get => _parent;
      set
      {
        InitializeParent(value);
      }
    }

    protected void InitializeParent(DataEntity parent)
    {
      if (_parent != null)
      {
        throw new Exception("Cannot reparent a data element!");
      }
      _parent = parent;
      _notifyManager = parent.NotifyManager;
      _parent.AddElement(this);
      _observable = new Observable<DataElement>(this, _notifyManager);
    }

    public string Name
    {
      get
      {
        return $"{_parent.Name}.{GetType()}";
      }
    }
    protected Observable<DataElement> _observable;
    protected Observable<DataElement> _eventBufferObservable;
    protected INotifyManager _notifyManager;

    public DataElement()
    {
    }
    
    public void EnqueueEvent(DataEvent evt)
    {
      if (_eventBuffer == null)
      {
        _eventBuffer = new EventBuffer(_notifyManager);
      }
      _eventBuffer.EnqueueEvent(evt);
    }
    
    public bool HasEvent(string id)
    {
      if (_eventBuffer == null)
      {
        return false;
      }

      return _eventBuffer.HasEvent(id);
    }


    public T EventForId<T>(string id) where T : DataEvent
    {
      if (_eventBuffer == null)
      {
        return null;
      }
      return _eventBuffer.EventForId<T>(id);
    }
    
    public void NotifyUpdated()
    {
      _observable.NotifyUpdated();
      _eventBufferObservable?.NotifyUpdated();
    }

    public void RemoveObserver(IDataObserver<DataElement> element)
    {
      _observable.RemoveObserver(element);
    }

    public void AddObserver(IDataObserver<DataElement> element)
    {
      _observable.AddObserver(element);
    }

    public void SetProperty<T>(T newVal, ref T val)
    {
      PropertyCheck(newVal, ref val, _observable);
    }

    public static void PropertyCheck<T>(T newVal, ref T val, Observable observable)
    {
      if (newVal.Equals(val))
      {
        return;
      }

      val = newVal;
      observable.MarkDirty();

    }
  }
}