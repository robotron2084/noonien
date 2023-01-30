using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace com.enemyhideout.noonien
{
  public class Collection<T> : ICollection<T>
  {

    protected List<T> _children = new List<T>();
    protected INotifyManager _notifyManager;
    protected Observable<ICollection<T>> _observable;
    private List<CollectionChange<T>> _changes = new List<CollectionChange<T>>();
    public Collection(INotifyManager notifyManager)
    {
      _notifyManager = notifyManager;
      _observable = new Observable<ICollection<T>>(this, _notifyManager);
    }


    public void InsertChild(int index, T entity)
    {
      _children.Insert(index, entity);
      _changes.Add(new CollectionChange<T>
      {
        Item = entity,
        OldIndex = -1,
        NewIndex = index,
        Action = CollectionChangeAction.Added
      });
      MarkDirty();
    }

    public void AddChild(T entity)
    {
      _children.Add(entity);
      _changes.Add(new CollectionChange<T>
      {
        Item = entity,
        OldIndex = -1,
        NewIndex = _children.Count - 1,
        Action = CollectionChangeAction.Added
      });
      MarkDirty();
    }

    public void RemoveChild(T dataEntity)
    {
      int index = _children.IndexOf(dataEntity);
      _children.Remove(dataEntity);
      _changes.Add(new CollectionChange<T>
      {
        Item = dataEntity,
        OldIndex = index,
        NewIndex = -1,
        Action = CollectionChangeAction.Removed
      });
      MarkDirty();
    }
    
    public void RemoveObserver(IDataObserver<ICollection<T>> element)
    {
      _observable.RemoveObserver(element);
    }

    public int Version => _observable.Version;

    public void AddObserver(IDataObserver<ICollection<T>> element)
    {
      _observable.AddObserver(element);
    }

    public bool HasObservers()
    {
      return _observable.HasObservers();
    }

    public IReadOnlyCollection<CollectionChange<T>> GetChanges()
    {
      return _changes as IReadOnlyCollection<CollectionChange<T>>;
    }

    public void Clear()
    {
      foreach (var child in _children.ToList())
      {
        RemoveChild(child);
      }
    }

    /// <summary>
    /// Clear our events out in LateUpdate.
    /// </summary>
    private void Reset()
    {
      _dirty = false;
      _changes.Clear();
    }

    public IEnumerator<T> GetEnumerator()
    {
      return _children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public T this[int index] => _children[index];
    public int Count
    {
      get => _children.Count;
    }

    private bool _dirty = false;
    public void MarkDirty()
    {
      _observable.MarkDirty();
      if (!_dirty)
      {
        _dirty = true;
        _notifyManager?.EnqueueNotifier(Reset, NotifyManager.LateUpdate);
      }
    }
  }
}