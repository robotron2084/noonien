using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.soong
{
  public class NotifyManager : MonoBehaviour, INotifyManager
  {

    public static int LateUpdate = 100;

    private SortedDictionary<int, NotifyQueue> _queues = new SortedDictionary<int, NotifyQueue>();
    
    public class NotifyQueue
    {
      private List<Action> _actions = new List<Action>();
      private List<Action> _actionsSwp = new List<Action>();
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


      public void Add(Action callback)
      {
        _actions.Add(callback);
      }
    }


    public void Update()
    {
      NotifyObservers();
    }


    public void NotifyObservers()
    {
      foreach (var queuesValue in _queues.Values)
      {
        queuesValue.NotifyObservers();
      }
    }

    public void EnqueueNotifier(Action callback, int queuePriority=0)
    {
      NotifyQueue queue = null;
      if (!_queues.TryGetValue(queuePriority, out queue))
      {
        queue = new NotifyQueue();
        _queues[queuePriority] = queue;
      }
      queue.Add(callback);
    }

  }
}