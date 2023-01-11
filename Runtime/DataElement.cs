using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;

namespace com.enemyhideout.soong
{
  public class DataElement
  {
    public DataModel _parent;

    public DataModel Parent
    {
      get => _parent;
      set => _parent = value;
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
      // todo, mark this up!
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