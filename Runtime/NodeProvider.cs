using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.noonien
{
  
  // Provides the glue between the data hierarchy and the game object hierarchy.
  public class NodeProvider : MonoBehaviour
  {

    private Node _node;

    public Node Node
    {
      get
      {
        return _node;
      }
      set
      {
        if (_node == value)
        {
          return;
        }
        _node = value;
        foreach (var modelObserver in _observations)
        {
          modelObserver.DataUpdated(_node);
        }
      }
    }

    private List<INodeObserver> _observations = new List<INodeObserver>();

    public void ObserveModel(INodeObserver observer)
    {
      _observations.Add(observer);
      if (_node != null)
      {
        observer.DataUpdated(_node);
      }
    }

    public void RemoveObserver(INodeObserver observer)
    {
      _observations.Remove(observer);
    }
  }
  
}