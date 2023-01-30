using System;

namespace com.enemyhideout.noonien
{
  public interface INotifyManager
  {
    void NotifyObservers();
    void EnqueueNotifier(Action callback, int queuePriority=0);
  }
  
}