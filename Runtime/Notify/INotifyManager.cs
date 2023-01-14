using System;

namespace com.enemyhideout.soong
{
  public interface INotifyManager
  {
    void NotifyObservers();
    void EnqueueNotifier(Action callback);
  }
  
}