using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.soong
{
  public class CollectionSource : MonoBehaviour
  {
    
    private IEntityCollection _collection;

    public IEntityCollection Collection
    {
      get
      {
        return _collection;
      }
      set
      {
        if (_collection == value)
        {
          return;
        }
        _collection = value;
        foreach (var modelObserver in _observations)
        {
          modelObserver.DataUpdated(_collection);
        }
      }
    }

    private List<IDataObserver<IEntityCollection>> _observations = new List<IDataObserver<IEntityCollection>>();

    public void AddObserver(IDataObserver<IEntityCollection> observer)
    {
      _observations.Add(observer);
      if (_collection != null)
      {
        observer.DataUpdated(_collection);
      }
    }

    public void RemoveObserver(IDataObserver<IEntityCollection> observer)
    {
      _observations.Remove(observer);
    }
  }
}