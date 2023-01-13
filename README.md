# Soong - An Entity Component System for Data

WORK IN PROGRESS

Would you like a better way to work with Data in your games? Well, my friend, you're looking at it.

![Noonien Soong](https://images.squarespace-cdn.com/content/v1/5cc3d1b051f4d40415789cc2/cc796e93-4fa2-4dee-a14d-059cd47d55fc/Noonien-Soong-data-brothers.jpg?format=1000w)

TODO

  * Virtual Entity Collections
  * Docs
  * Helper functions for instantiation and collections.
  * GetElement<T> should handle subtypes/interfaces better?

DONE

  * Data Entities
  * Data Elements
  * Entity Collections
  * Data Hierarchies
  * Observables
  * NotifyManager


## Random Mumblings

So I want two types of observable collections:

The children of an entity.

A 'virtual collection'. Maybe all collections should be virtual, and the children collection should just be a subtype of that collection.

So what do we want to observe, and how do we want to observe it?

Something like:

```csharp

var parent = new DataEntity("EnemyUnits");
// by default all Children implement IEntityCollection.
var element = new CollectionElement(parent.Children, notifyManager, collection);

var enemyUnitManager = new GameObject();
var dataSource = enemyUnitManager.AddComponent<DataSource>();
var enemyUnitCounter = enemyUnitManager.AddComponent<EntityCollectionObserver>();

dataSource.Entity = parent;

//add children to parent, and the element will update and notify.

```
```csharp
ObserveCollection(new VirtualCollection(new List<DataEntity>(){  ... } ));
```
