using System.Collections.Generic;
using System.Linq;

namespace com.enemyhideout.soong
{

  public class CollectionObserver : ElementObserver<CollectionElement>
  {
    private CollectionDelta<DataEntity> _delta;
    private Observable<IEntityCollection> _collectionObserver;

    protected override void DataAdded(CollectionElement instance)
    {
      base.DataAdded(instance);
      _delta = new CollectionDelta<DataEntity>();
      instance.Collection.AddObserver(new DataObserver<IEntityCollection>(OnCollectionChanged));
    }

    protected void OnCollectionChanged(IEntityCollection collection)
    {
      var changes = _delta.ComputeChanges(_element.Collection.ToList());
      NotifyIfCollectionChanged(changes);
    }

    protected override void DataUpdated(CollectionElement instance)
    {
      base.DataUpdated(instance);
      var changes = _delta.ComputeChanges(instance.Collection.ToList());
      NotifyIfCollectionChanged(changes);
    }

    protected override void DataRemoved(CollectionElement instance)
    {
      base.DataRemoved(instance);
      _delta = null;
    }

    private void NotifyIfCollectionChanged(List<CollectionChange<DataEntity>> collectionChanges)
    {
      if (collectionChanges.Count > 0)
      {
        CollectionUpdated(collectionChanges);
      }
    }

    protected virtual void CollectionUpdated(List<CollectionChange<DataEntity>> collectionChanges)
    {
      
    }
  }
}