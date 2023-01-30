using System.Collections.Generic;
using System.Linq;

namespace com.enemyhideout.noonien
{

  public class CollectionObserver : ElementObserver<CollectionElement>
  {
    private VersionedDataObserver<ICollection<Node>> _collectionObserver;
    private ICollection<Node> _observedCollection; // todo: encapsulate this better in an observer.
    
    protected override void DataAdded(CollectionElement element)
    {
      base.DataAdded(element);
      removeCollectionObserver();
      addObserver();
    }

    protected override void DataUpdated(CollectionElement element)
    {
      base.DataUpdated(element);
      if (element.Collection != _observedCollection)
      {
        removeCollectionObserver();
        addObserver();
      }
    }

    private void addObserver()
    {
      _observedCollection = _element.Collection;
      _collectionObserver = new VersionedDataObserver<ICollection<Node>>(OnCollectionChanged, _observedCollection.Version);
      _observedCollection.AddObserver(_collectionObserver);
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

    private void removeCollectionObserver()
    {
      if (_collectionObserver != null)
      {
        _observedCollection.RemoveObserver(_collectionObserver);
      }

      _observedCollection = null;

    }

    protected void OnCollectionChanged(ICollection<Node> collection)
    {
      var changes  = collection.GetChanges();
      NotifyIfCollectionChanged(changes);
    }

    protected override void DataRemoved(CollectionElement element)
    {
      base.DataRemoved(element);
      _observedCollection.RemoveObserver(_collectionObserver);
      _observedCollection = null;
      _collectionObserver = null;
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

    protected override void OnDestroy()
    {
      base.OnDestroy();
      if (_element != null && _element.Collection != null)
      {
        _element.Collection.RemoveObserver(_collectionObserver);
      }
    }
  }
}