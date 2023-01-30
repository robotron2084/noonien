using System.Collections.Generic;
using System.Linq;
using com.enemyhideout.noonien;

namespace Tests.Runtime
{
  public class CollectionCounter : CollectionObserver
  {
    public IReadOnlyCollection<CollectionChange<Node>> LatestChanges;
    public List<Node> Items = new List<Node>();

    protected override void DataAdded(CollectionElement element)
    {
      Items.Clear();
      base.DataAdded(element);
    }

    protected override void CollectionUpdated(IReadOnlyCollection<CollectionChange<Node>> collectionChanges)
    {
      base.CollectionUpdated(collectionChanges);
      LatestChanges = collectionChanges.ToList();
      foreach (var collectionChange in collectionChanges)
      {
        switch (collectionChange.Action)
        {
          case CollectionChangeAction.Added:
            Items.Insert(collectionChange.NewIndex, collectionChange.Item);
            break;
          case CollectionChangeAction.Removed:
            Items.RemoveAt(collectionChange.OldIndex);
            break;
        }
      }
    }

    protected override void DataRemoved(CollectionElement element)
    {
      base.DataRemoved(element);
      Items.Clear();
    }
  }
}