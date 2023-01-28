using System.Collections.Generic;
using System.Linq;
using com.enemyhideout.soong;

namespace Tests.Runtime
{
  public class CollectionCounter : CollectionObserver
  {
    public IReadOnlyCollection<CollectionChange<DataEntity>> LatestChanges;
    public List<DataEntity> Items = new List<DataEntity>();

    protected override void DataAdded(CollectionElement instance)
    {
      Items.Clear();
      base.DataAdded(instance);
    }

    protected override void CollectionUpdated(IReadOnlyCollection<CollectionChange<DataEntity>> collectionChanges)
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

    protected override void DataRemoved(CollectionElement instance)
    {
      base.DataRemoved(instance);
      Items.Clear();
    }
  }
}