using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace com.enemyhideout.soong
{
  
  // Provides the glue between the data hierarchy and the game object hierarchy.
  public class DataSource : MonoBehaviour
  {

    private DataModel _model;

    public DataModel Model
    {
      get
      {
        return _model;
      }
      set
      {
        if (_model == value)
        {
          return;
        }
        _model = value;
        foreach (var modelObserver in _observations)
        {
          modelObserver.ModelUpdated(_model);
        }
      }
    }

    private List<IModelObserver> _observations = new List<IModelObserver>();

    public void ObserveModel(IModelObserver observer)
    {
      _observations.Add(observer);
      if (_model != null)
      {
        observer.ModelUpdated(_model);
      }
    }

    public void RemoveObserver(IModelObserver observer)
    {
      _observations.Remove(observer);
    }
  }
  
}