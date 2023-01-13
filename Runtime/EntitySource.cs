using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.soong
{
  
  // Provides the glue between the data hierarchy and the game object hierarchy.
  public class EntitySource : MonoBehaviour
  {

    private DataEntity _entity;

    public DataEntity Entity
    {
      get
      {
        return _entity;
      }
      set
      {
        if (_entity == value)
        {
          return;
        }
        _entity = value;
        foreach (var modelObserver in _observations)
        {
          modelObserver.DataUpdated(_entity);
        }
      }
    }

    private List<IEntityObserver> _observations = new List<IEntityObserver>();

    public void ObserveModel(IEntityObserver observer)
    {
      _observations.Add(observer);
      if (_entity != null)
      {
        observer.DataUpdated(_entity);
      }
    }

    public void RemoveObserver(IEntityObserver observer)
    {
      _observations.Remove(observer);
    }
  }
  
}