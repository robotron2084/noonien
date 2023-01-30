using System.Collections.Generic;

namespace com.enemyhideout.noonien
{

  public interface IObservable<TObserved>
  {
    void AddObserver(IDataObserver<TObserved> observer);
    void RemoveObserver(IDataObserver<TObserved> observer);
    
    int Version { get; }
  }
  
  public abstract class Observable
  {
    protected bool _dirty = false;
    protected INotifyManager _notifyManager;

    protected int _version;
    public int Version => _version;
    protected int _queuePriority = 0;
    public abstract void MarkDirty();
  }
  
  public class Observable<TObserved> : Observable, IObservable<TObserved>
  {
    private TObserved _observered;

    public Observable(TObserved observed, INotifyManager notifyManager)
    {
      _observered = observed;
      _notifyManager = notifyManager;
    }
    
    public Observable(TObserved observed, INotifyManager notifyManager, int queuePriority)
    {
      _observered = observed;
      _notifyManager = notifyManager;
      _queuePriority = queuePriority;
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
        _notifyManager?.EnqueueNotifier(NotifyUpdated, _queuePriority);
      }
      _version++;
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