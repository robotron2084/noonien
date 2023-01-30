using System.Collections.Generic;

namespace com.enemyhideout.noonien
{
  public class CompositeNodeCollection : NodeCollection
  {
    private IEnumerable<ICollection<Node>> _collections;
    private DataObserver<ICollection<Node>> _observer;

    public CompositeNodeCollection(INotifyManager notifyManager, params ICollection<Node>[] collections) : base(notifyManager)
    {
      _collections = collections;
      _observer = new DataObserver<ICollection<Node>>(OnCollectionChanged);
      foreach (var entityCollection in _collections)
      {
        entityCollection.AddObserver(_observer);
      }
      OnCollectionChanged(null);
    }

    private void OnCollectionChanged(ICollection<Node> obj)
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