using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;

namespace com.enemyhideout.soong
{
  public class DataElement
  {
    public DataEntity _parent;

    private INotifyManager _notifyManager;

    public DataElement(DataEntity parent, INotifyManager notifyManager)
    {
      _parent = parent;
      parent.AddElement(this);
      _notifyManager = notifyManager;
    }

    public DataEntity Parent
    {
      get => _parent;
    }

    public string Name
    {
      get
      {
        return $"{_parent.Name}.{GetType()}";
      }
    }

    private List<IDataObserver> _observers = new List<IDataObserver>();

    public void AddObserver(IDataObserver observer)
    {
      _observers.Add(observer);
    }

    public void RemoveObserver(IDataObserver observer)
    {
      _observers.Remove(observer);
    }

    public void MarkDirty()
    {
      _notifyManager?.EnqueueNotifier(NotifyUpdated);
    }

    public void NotifyUpdated()
    {
      foreach (var observation in _observers)
      {
        observation.ElementUpdated(this);
      }
    }

    public void SetProperty<T>(T newVal, ref T val)
    {
      PropertyCheck(newVal, ref val, this);
    }

    public static void PropertyCheck<T>(T newVal, ref T val, DataElement element)
    {
      if (newVal.Equals(val))
      {
        return;
      }

      val = newVal;
      element.MarkDirty();

    }
  }
}