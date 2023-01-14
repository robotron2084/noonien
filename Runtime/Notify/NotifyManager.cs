using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.soong
{
  public class NotifyManager : MonoBehaviour, INotifyManager
  {
    private List<Action> _actions = new List<Action>();
    private List<Action> _actionsSwp = new List<Action>();

    public void Update()
    {
      NotifyObservers();
    }
        
    public void NotifyObservers()
    {
      int maxIterations = 10;
      int iterations = 0;
      while (_actions.Count > 0 && iterations < maxIterations)
      {
        DoNotify();
        iterations++;
      }

      if (iterations == maxIterations && _actions.Count > 0)
      {
        throw new Exception("Too many iterations hit while notifying.");
      }
    }

    private void DoNotify()
    {
      var executingList = _actions;
      _actions = _actionsSwp;
      _actionsSwp = executingList;
      foreach (var action in executingList)
      {
        action();
      }
      executingList.Clear();
    }
    

    public void EnqueueNotifier(Action callback)
    {
      _actions.Add(callback);
    }

  }
}