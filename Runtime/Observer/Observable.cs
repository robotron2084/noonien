using System.Collections.Generic;

namespace com.enemyhideout.soong
{

  public abstract class Observable
  {
    protected bool _dirty = false;
    protected INotifyManager _notifyManager;
    public abstract void MarkDirty();
  }
  
  public class Observable<TObserved> : Observable
  {
    private TObserved _observered;

    public Observable(TObserved observed, INotifyManager notifyManager)
    {
      _observered = observed;
      _notifyManager = notifyManager;
    }

    private List<IDataObserver<TObserved>> _observers = new List<IDataObserver<TObserved>>();

    public void AddObserver(IDataObserver<TObserved> observer)
    {
      _observers.Add(observer);
    }

    public void RemoveObserver(IDataObserver<TObserved> observer)
    {
      _observers.Remove(observer);
    }

    public override void MarkDirty()
    {
      if (!_dirty)
      {
        _dirty = true; // do not allow enqueuing.
        _notifyManager?.EnqueueNotifier(NotifyUpdated);
      }
    }

    public void NotifyUpdated()
    {
      _dirty = false;
      foreach (var observation in _observers)
      {
        observation.DataUpdated(_observered);
      }
    }

  }
}