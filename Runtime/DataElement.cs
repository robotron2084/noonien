using System;
using System.Collections.Generic;
using System.Linq;

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

    private List<Observation> _observers;

    public void AddObserver(IDataObserver observer, Action<DataElement> callback)
    {
      _observers.Add(new Observation
      {
        Observer = observer,
        Callback = callback
      });
    }

    public void RemoveObserver(IDataObserver observer)
    {
      var obs = _observers.RemoveAll(x => x.Observer == observer);
    }

    public void MarkDirty()
    {
      
    }

    public void NotifyObservers()
    {
      foreach (var observation in _observers)
      {
        observation.Callback(this);
      }
    }
  }


  public class Observation
  {
    public IDataObserver Observer;
    public Action<DataElement> Callback;
  }
  
  public interface IDataObserver
  {
  }
}