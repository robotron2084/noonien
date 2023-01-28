using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.soong
{
  public class CollectionSource : MonoBehaviour
  {
    
    private ICollection<DataEntity> _collection;

    public ICollection<DataEntity> Collection
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

    private List<IDataObserver<ICollection<DataEntity>>> _observations = new List<IDataObserver<ICollection<DataEntity>>>();

    public void AddObserver(IDataObserver<ICollection<DataEntity>> observer)
    {
      _observations.Add(observer);
      if (_collection != null)
      {
        observer.DataUpdated(_collection);
      }
    }

    public void RemoveObserver(IDataObserver<ICollection<DataEntity>> observer)
    {
      _observations.Remove(observer);
    }
  }
}