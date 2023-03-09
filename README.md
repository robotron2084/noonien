# Noonien - An Entity Component System for Data

WORK IN PROGRESS

Would you like a better way to work with Data in your games? Well, my friend, you're looking at it.

![Noonien Soong](https://images.squarespace-cdn.com/content/v1/5cc3d1b051f4d40415789cc2/cc796e93-4fa2-4dee-a14d-059cd47d55fc/Noonien-Soong-data-brothers.jpg?format=1000w)

TODO
  * Docs
  * Systems Design - Create a controller subsystem for executing ticks.
    * Problem - remove items automatically from system.
  * Serialization/Initialization - Can we define and load a data hierarchy without needing code?
  * Improve the observing of collections - ElementObserver should allow for the observing of Collections just like Elements.
  * Improve the observing of 'sub-items' - What if we want our observer to watch multiple nodes? What if we want our observer to be notified when, say, an element in an element is updated? Do I even want that?
  * Helper functions for instantiation and collections - Spawners, Prefab/Addressables providers.
  * Node Editor - Allow for display of collection items in graph, and also to open sub nodes.
  * More Samples
    * Rigid Body Based game.
    * Shop

DONE
  * Node Editor - Add scrolling to imgui container.
  * Renamed from Soong to Noonien. Highlights
    * DataEntity -> Node
    * DataElement -> Element
    * EntitySource -> NodeProvider
    * EntityManager -> NodeManager
  * Add: Make EntityManager have a concept of a Cached Item.
    * You can now Add/Register/Get nodes, elements, and collections.
  * Fix: issue with instantiation race condition for EntitySource.
  * Fix: NamedEntitySource isn't correctly updating.
  * Fix: Make sure Listen<T> works for multiple entity types.
  * Replace the inefficient Collection Delta class with events.
  * Added queuePriority to NotifyManager so we can have phases of updates.
  * Events Bus? Something that would queue events on the element, that other elements could then listen for that are transient?
  * Added HelloEntity Sample.
  * Editor UI to view Entity Hierarchies.
  * Inspector UI for EntitySource.
  * EntityManager that can query stuff.
  * NamedEntitySource
  * GetElementAt<> needs to return null if item not found.
  * Created a small game sample. 
  * GetElement<T> should handle subtypes/interfaces better.
  * Virtual Entity Collections
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

// Let's say that we want our observer to update when a node inside another observer is updated. We have a couple options:

// 1. Bubble up a data updated event to the 'root' element. This allows for
// simple bindings of data. For example:

public class MyElement : Element
{
  private SubElement _subData;
  public SubElement SubData
  {
    get{
      return _subData;
    }
    set{
      if(_subData != null)
      {
        RemoveObserver(_subData);
      }
      _subData = value;
      if(_subData != null)
      {
        AddObserver(_subData);
      }
    }
  }
}

// 2. Require explicit listening to the element. Possibly more useful for collections
// or if we want to do something in particular if this value were to change.

public class MyObserver : ElementObserver<MyElement>
{

    void Awake()
    {
    }


}


```