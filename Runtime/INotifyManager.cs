using System;

namespace DefaultNamespace
{
  public interface INotifyManager
  {
    void NotifyObservers();
    void EnqueueNotifier(Action callback);
  }
  
}