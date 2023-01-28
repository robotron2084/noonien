using System.Collections.Generic;
using System.Linq;

namespace com.enemyhideout.soong
{

  public class CollectionObserver : ElementObserver<CollectionElement>
  {
    private VersionedDataObserver<ICollection<DataEntity>> _collectionObserver;

    protected override void DataAdded(CollectionElement instance)
    {
      base.DataAdded(instance);
      _collectionObserver = new VersionedDataObserver<ICollection<DataEntity>>(OnCollectionChanged, instance.Collection.Version);
      instance.Collection.AddObserver(_collectionObserver);
      List<CollectionChange<DataEntity>> changes = new List<CollectionChange<DataEntity>>();
      for (var i = 0; i < _element.Collection.Count; i++)
      {
        var dataEntity = _element.Collection[i];
        changes.Add(new CollectionChange<DataEntity>
        {
          Item = dataEntity,
          OldIndex = -1,
          NewIndex = i,
          Action = CollectionChangeAction.Added
        });
      }
      NotifyIfCollectionChanged(changes);

    }

    protected void OnCollectionChanged(ICollection<DataEntity> collection)
    {
      var changes  = collection.GetChanges();
      NotifyIfCollectionChanged(changes);
    }

    protected override void DataRemoved(CollectionElement instance)
    {
      base.DataRemoved(instance);
      instance.Collection.RemoveObserver(_collectionObserver);
    }

    private void NotifyIfCollectionChanged(IReadOnlyCollection<CollectionChange<DataEntity>> collectionChanges)
    {
      if (collectionChanges.Count > 0)
      {
        CollectionUpdated(collectionChanges);
      }
    }

    protected virtual void CollectionUpdated(IReadOnlyCollection<CollectionChange<DataEntity>> collectionChanges)
    {
      
    }
  }
}