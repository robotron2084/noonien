using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Tests.Runtime
{
  public class TestNotifyManager : INotifyManager
  {
    private List<Action> _actions = new List<Action>();
    private List<Action> _actions2 = new List<Action>();
    
    public void NotifyObservers()
    {
      var executingList = _actions;
      _actions = _actions2;
      _actions2 = executingList;
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