using System.Collections.Generic;
using System.Linq;

namespace com.enemyhideout.noonien
{

  public class CollectionObserver : ElementObserver<CollectionElement>
  {
    private VersionedDataObserver<ICollection<Node>> _collectionObserver;

    protected override void DataAdded(CollectionElement element)
    {
      base.DataAdded(element);
      _collectionObserver = new VersionedDataObserver<ICollection<Node>>(OnCollectionChanged, element.Collection.Version);
      element.Collection.AddObserver(_collectionObserver);
      List<CollectionChange<Node>> changes = new List<CollectionChange<Node>>();
      for (var i = 0; i < _element.Collection.Count; i++)
      {
        var dataEntity = _element.Collection[i];
        changes.Add(new CollectionChange<Node>
        {
          Item = dataEntity,
          OldIndex = -1,
          NewIndex = i,
          Action = CollectionChangeAction.Added
        });
      }
      NotifyIfCollectionChanged(changes);

    }

    protected void OnCollectionChanged(ICollection<Node> collection)
    {
      var changes  = collection.GetChanges();
      NotifyIfCollectionChanged(changes);
    }

    protected override void DataRemoved(CollectionElement element)
    {
      base.DataRemoved(element);
      element.Collection.RemoveObserver(_collectionObserver);
    }

    private void NotifyIfCollectionChanged(IReadOnlyCollection<CollectionChange<Node>> collectionChanges)
    {
      if (collectionChanges.Count > 0)
      {
        CollectionUpdated(collectionChanges);
      }
    }

    protected virtual void CollectionUpdated(IReadOnlyCollection<CollectionChange<Node>> collectionChanges)
    {
      
    }
  }
}