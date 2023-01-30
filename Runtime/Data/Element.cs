using System;
using System.Collections.Generic;
using System.Linq;

namespace com.enemyhideout.noonien
{
  public abstract class Element : IObservable<Element>
  {
    protected Node _parent;
    private EventBuffer _eventBuffer;

    public int Version => _observable.Version;

    public virtual Node Parent
    {
      get => _parent;
      set
      {
        InitializeParent(value);
      }
    }

    protected void InitializeParent(Node parent)
    {
      if (_parent != null)
      {
        throw new Exception("Cannot reparent a data element!");
      }
      _parent = parent;
      _notifyManager = parent.NotifyManager;
      _parent.AddElement(this);
      _observable = new Observable<Element>(this, _notifyManager);
    }

    public string Name
    {
      get
      {
        return $"{_parent.Name}.{GetType()}";
      }
    }
    protected Observable<Element> _observable;
    protected Observable<Element> _eventBufferObservable;
    protected INotifyManager _notifyManager;

    public Element()
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

    public void RemoveObserver(IDataObserver<Element> element)
    {
      _observable.RemoveObserver(element);
    }

    public void AddObserver(IDataObserver<Element> element)
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