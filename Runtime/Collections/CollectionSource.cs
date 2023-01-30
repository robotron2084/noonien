using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.noonien
{
  public class CollectionSource : MonoBehaviour
  {
    
    private ICollection<Node> _collection;

    public ICollection<Node> Collection
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

    private List<IDataObserver<ICollection<Node>>> _observations = new List<IDataObserver<ICollection<Node>>>();

    public void AddObserver(IDataObserver<ICollection<Node>> observer)
    {
      _observations.Add(observer);
      if (_collection != null)
      {
        observer.DataUpdated(_collection);
      }
    }

    public void RemoveObserver(IDataObserver<ICollection<Node>> observer)
    {
      _observations.Remove(observer);
    }
  }
}