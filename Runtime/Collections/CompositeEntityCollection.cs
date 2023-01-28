using System.Collections.Generic;

namespace com.enemyhideout.soong
{
  public class CompositeEntityCollection : EntityCollection
  {
    private IEnumerable<ICollection<DataEntity>> _collections;
    private DataObserver<ICollection<DataEntity>> _observer;

    public CompositeEntityCollection(INotifyManager notifyManager, params ICollection<DataEntity>[] collections) : base(notifyManager)
    {
      _collections = collections;
      _observer = new DataObserver<ICollection<DataEntity>>(OnCollectionChanged);
      foreach (var entityCollection in _collections)
      {
        entityCollection.AddObserver(_observer);
      }
      OnCollectionChanged(null);
    }

    private void OnCollectionChanged(ICollection<DataEntity> obj)
    {
      _children.Clear();
      foreach (var entityCollection in _collections)
      {
        _children.AddRange(entityCollection);
      }
      MarkDirty();
    }

    public void Dispose()
    {
      foreach (var entityCollection in _collections)
      {
        entityCollection.RemoveObserver(_observer);
      }
    }
  }
}