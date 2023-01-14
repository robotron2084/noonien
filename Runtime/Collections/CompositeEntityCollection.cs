using System.Collections.Generic;

namespace com.enemyhideout.soong
{
  public class CompositeEntityCollection : EntityCollection
  {
    private IEnumerable<IEntityCollection> _collections;
    private DataObserver<IEntityCollection> _observer;

    public CompositeEntityCollection(INotifyManager notifyManager, params IEntityCollection[] collections) : base(notifyManager)
    {
      _collections = collections;
      _observer = new DataObserver<IEntityCollection>(OnCollectionChanged);
      foreach (var entityCollection in _collections)
      {
        entityCollection.AddObserver(_observer);
      }
      OnCollectionChanged(null);
    }

    private void OnCollectionChanged(IEntityCollection obj)
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