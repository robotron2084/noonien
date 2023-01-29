using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.enemyhideout.soong;
using NUnit.Framework;
using Tests.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

public class SoongRuntimeTests
{
    private INotifyManager _notifyManager;
    [SetUp]
    public void OneTimeSetup()
    {
        GameObject goManager = new GameObject();
        _notifyManager = goManager.AddComponent<NotifyManager>();
    }
    
    // A Test behaves as an ordinary method
    [Test]
    public void SoongRuntimeTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator SoongRuntimeTestsWithEnumeratorPasses()
    {
        DataEntity entity = new DataEntity(null);
        HealthElement health = new HealthElement(entity);

        health.Health = 10;

        GameObject go = new GameObject();
        var dataSource = go.AddComponent<EntitySource>();
        var healthObserver = go.AddComponent<HealthObserver>();

        GameObject go2 = new GameObject();
        var dataSource2 = go2.AddComponent<EntitySource>();
        var healthObserver2 = go2.AddComponent<HealthObserver>();

        dataSource.Entity = entity;
        // healthObserver won't initialize until start, so let's wait.
        yield return null;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));

        health.Health = 9;
        
        health.NotifyUpdated();
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));

        dataSource.Entity = null;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(-1));

        // Ensure that the observer isn't receiving notifications.
        health.Health = 8;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(-1));
        
        // glue to data source2.
        dataSource2.Entity = entity;
        Assert.That(healthObserver2.ObservedHealth, Is.EqualTo(health.Health));
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(-1));

        dataSource.Entity = entity;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));
        Assert.That(healthObserver2.ObservedHealth, Is.EqualTo(health.Health));
        
    }

    [UnityTest]
    public IEnumerator TestNotify()
    {
        DataEntity entity = new DataEntity(_notifyManager);
        HealthElement health = new HealthElement(entity);

        health.Health = 10;

        GameObject go = new GameObject();
        var dataSource = go.AddComponent<EntitySource>();
        var healthObserver = go.AddComponent<HealthObserver>();

        dataSource.Entity = entity;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));

        health.Health = 9;
        Assert.That(healthObserver.ObservedHealth, Is.Not.EqualTo(health.Health));
        yield return null;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));

        health.Health = 8;
        yield return null;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));
    }
    
    [UnityTest]
    public IEnumerator TestOneIteration()
    {
        GameObject goManager = new GameObject();
        NotifyManager notifyManager = goManager.AddComponent<NotifyManager>();

        GameObject go = new GameObject();
        TestUpdateBehavior updateBehavior = go.AddComponent<TestUpdateBehavior>();
        updateBehavior._NotifyManager = notifyManager;
        updateBehavior.TriggerUpdate();
        yield return null;
        Assert.That(updateBehavior.TestValue, Is.EqualTo(42));
    }
    
    [UnityTest]
    public IEnumerator TestTwoIterations()
    {
        GameObject go = new GameObject();
        DependentUpdateBehaviour updateBehavior = go.AddComponent<DependentUpdateBehaviour>();
        DependentUpdateBehaviour updateBehavior2 = go.AddComponent<DependentUpdateBehaviour>();
        updateBehavior.NotifyManager = _notifyManager;
        updateBehavior2.NotifyManager = _notifyManager;
        updateBehavior.ItemToTrigger = updateBehavior2;
        updateBehavior.TriggerUpdate();
        yield return null;
        Assert.That(updateBehavior.TestValue, Is.EqualTo(42));
        Assert.That(updateBehavior2.TestValue, Is.EqualTo(42));
    }
    
    [UnityTest]
    public IEnumerator TestUnsafeIterations()
    {
        GameObject go = new GameObject();
        DependentUpdateBehaviour updateBehavior = go.AddComponent<DependentUpdateBehaviour>();
        DependentUpdateBehaviour updateBehavior2 = go.AddComponent<DependentUpdateBehaviour>();
        updateBehavior.NotifyManager = _notifyManager;
        updateBehavior2.NotifyManager = _notifyManager;
        //circular loop
        updateBehavior.ItemToTrigger = updateBehavior2;
        updateBehavior2.ItemToTrigger = updateBehavior;
        updateBehavior.TriggerUpdate();
        
        Assert.Throws<Exception>(_notifyManager.NotifyObservers);
        //note: leaving this test in this state will cause exceptions to be thrown
        // and other tests to break seemingly randomly!
        
        // Ensure that the notifyManager clears out and can recover.
        updateBehavior.TestValue = 0;
        updateBehavior.TestValue = 0;
        updateBehavior2.ItemToTrigger = null;
        updateBehavior.TriggerUpdate();
        yield return null;
        Assert.That(updateBehavior.TestValue, Is.EqualTo(42));
        Assert.That(updateBehavior2.TestValue, Is.EqualTo(42));

        
        
        
    }

    [UnityTest]
    public IEnumerator TestCollectionObserving()
    {
        DataEntity parent = new DataEntity(_notifyManager, "Parent");
        new CollectionElement(parent);
        for (int i = 0; i < 5; i++)
        {
            var child = new DataEntity(_notifyManager);
            parent.AddChild(child);
        }

        GameObject go = new GameObject();
        var dataSource = go.AddComponent<EntitySource>();
        var collectionCounter = go.AddComponent<CollectionCounter>();
        dataSource.Entity = parent;
        
        yield return null;
        Assert.That(collectionCounter.Items.Count, Is.EqualTo(parent.ChildrenCount));
        var newChild = new DataEntity(_notifyManager);
        
        parent.AddChild(newChild);
        Assert.That(collectionCounter.Items, Is.Not.EqualTo(parent.Children));
        yield return null;
        Assert.That(collectionCounter.LatestChanges.Count, Is.EqualTo(1));
        Assert.That(collectionCounter.Items, Is.EqualTo(parent.Children));

        newChild.RemoveParent();
        Assert.That(collectionCounter.Items, Is.Not.EqualTo(parent.Children));
        yield return null;
        Assert.That(collectionCounter.Items, Is.EqualTo(parent.Children));

        var lastChild = parent.GetChildAt(parent.ChildrenCount - 1);
        parent.InsertChild(0, lastChild);
        yield return null;
        Assert.That(collectionCounter.LatestChanges.Count, Is.EqualTo(2));
        
        Assert.That(collectionCounter.Items, Is.EqualTo(parent.Children));

    }
    
    
    [UnityTest]
    public IEnumerator TestCollectionClear()
    {
        DataEntity parent = new DataEntity(_notifyManager, "Parent");
        new CollectionElement(parent);
        for (int i = 0; i < 5; i++)
        {
            var child = new DataEntity(_notifyManager);
            parent.AddChild(child);
        }

        GameObject go = new GameObject();
        var dataSource = go.AddComponent<EntitySource>();
        var collectionCounter = go.AddComponent<CollectionCounter>();
        dataSource.Entity = parent;
        
        yield return null;
        Assert.That(collectionCounter.Items.Count, Is.EqualTo(parent.ChildrenCount));
        var newChild = new DataEntity(_notifyManager);
        
        parent.Children.Clear();
        Assert.That(parent.Children.Count, Is.EqualTo(0));
        Assert.That(collectionCounter.Items, Is.Not.EqualTo(parent.Children));
        yield return null;
        Assert.That(collectionCounter.LatestChanges.Count, Is.EqualTo(5));
        Assert.That(collectionCounter.Items, Is.EqualTo(parent.Children));
        Assert.That(collectionCounter.Items.Count, Is.EqualTo(0));
    }
    
    [UnityTest]
    public IEnumerator TestCollectionObservingAfterChangeEvents()
    {
        DataEntity parent = new DataEntity(_notifyManager, "Parent");
        new CollectionElement(parent);
        for (int i = 0; i < 5; i++)
        {
            var child = new DataEntity(_notifyManager);
            parent.AddChild(child);
        }

        // this is the key change from the above: the 'collection changes' have 
        // occurred, but we still want some kind of CollectionUpdated to occur
        // so that the CollectionCounter correctly gets initialized.
        yield return null;

        GameObject go = new GameObject();
        var dataSource = go.AddComponent<EntitySource>();
        var collectionCounter = go.AddComponent<CollectionCounter>();
        dataSource.Entity = parent;
        
        yield return null;
        Assert.That(collectionCounter.Items.Count, Is.EqualTo(parent.ChildrenCount));
        var newChild = new DataEntity(_notifyManager);
        
        parent.AddChild(newChild);
        Assert.That(collectionCounter.Items, Is.Not.EqualTo(parent.Children));
        yield return null;
        Assert.That(collectionCounter.LatestChanges.Count, Is.EqualTo(1));
        Assert.That(collectionCounter.Items, Is.EqualTo(parent.Children));

        newChild.RemoveParent();
        Assert.That(collectionCounter.Items, Is.Not.EqualTo(parent.Children));
        yield return null;
        Assert.That(collectionCounter.Items, Is.EqualTo(parent.Children));

        var lastChild = parent.GetChildAt(parent.ChildrenCount - 1);
        parent.InsertChild(0, lastChild);
        yield return null;
        Assert.That(collectionCounter.LatestChanges.Count, Is.EqualTo(2));
        
        Assert.That(collectionCounter.Items, Is.EqualTo(parent.Children));

    }
    
    
    [UnityTest]
    public IEnumerator TestCollectionIncludingMultiples()
    {
        DataEntity parent = new DataEntity(_notifyManager, "Parent");
        var collectionElement = new CollectionElement(parent);
        var collection = new EntityCollection(_notifyManager);
        for (int i = 0; i < 5; i++)
        {
            var child = new DataEntity(_notifyManager);
            parent.AddChild(child);
            // add the item to the collection twice.
            collection.AddChild(child);
            collection.AddChild(child);
        }

        collectionElement.Collection = collection;

        // this is the key change from the above: the 'collection changes' have 
        // occurred, but we still want some kind of CollectionUpdated to occur
        // so that the CollectionCounter correctly gets initialized.
        yield return null;

        GameObject go = new GameObject();
        var dataSource = go.AddComponent<EntitySource>();
        var collectionCounter = go.AddComponent<CollectionCounter>();
        dataSource.Entity = parent;
        
        yield return null;
        Assert.That(collectionCounter.Items, Is.EqualTo(collection));

    }


    [UnityTest]
    public IEnumerator TestCompositeCollection()
    {
        var parentA = new DataEntity( _notifyManager, "ParentA");
        AddChildren(parentA, _notifyManager, 5);
        var parentB = new DataEntity(_notifyManager, "ParentB");
        AddChildren(parentB, _notifyManager, 5);
        var composite = new CompositeEntityCollection(_notifyManager, parentA.Children, parentB.Children);

        var collection = parentA.Children.Concat(parentB.Children);
        Assert.That(composite, Is.EqualTo(collection));

        parentA.RemoveChild(parentA.Children[0]);

        collection = parentA.Children.Concat(parentB.Children);
        Assert.That(composite, Is.Not.EqualTo(collection));

        yield return null;
        Assert.That(composite, Is.EqualTo(collection));


    }

    
    [Test]
    public void TestFilterCollection()
    {
        var parentA = new DataEntity( _notifyManager, "ParentA");
        AddChildren(parentA, _notifyManager, 5);
        var filtered = new FilteredEntityCollection(x => x.Where(y => y.Name.Contains("2")), parentA.Children, _notifyManager);
        List<DataEntity> items = new List<DataEntity>() { parentA.Children[2] };
        Assert.That(filtered, Is.EqualTo(items));

        var anotherChild = new DataEntity(_notifyManager, "2 Fast 2 Furious");
        parentA.AddChild(anotherChild);
        items = new List<DataEntity>() { parentA.Children[2], anotherChild };

        Assert.That(filtered, Is.Not.EqualTo(items));
        _notifyManager.NotifyObservers();
        Assert.That(filtered, Is.EqualTo(items));

    }

    
    [Test]
    public void TestFilteredAndCompositedCollection()
    {
        var parentA = new DataEntity( _notifyManager, "ParentA");
        AddChildren(parentA, _notifyManager, 5);
        var parentB = new DataEntity( _notifyManager, "ParentB");
        AddChildren(parentB, _notifyManager, 5);
        var filtered = new FilteredEntityCollection(x => x.Where(y => y.Name.Contains("2")), parentA.Children, _notifyManager);
        var composite = new CompositeEntityCollection(_notifyManager, filtered, parentB.Children);

        
        List<DataEntity> items = new List<DataEntity>() { parentA.Children[2] };
        items.AddRange(parentB.Children);
        Assert.That(composite, Is.EqualTo(items));
        var anotherChild = new DataEntity(_notifyManager, "2 Fast 2 Furious");
        parentA.AddChild(anotherChild);
        _notifyManager.NotifyObservers();
        items = new List<DataEntity>() { parentA.Children[2] , anotherChild };
        items.AddRange(parentB.Children);
        Assert.That(composite, Is.EqualTo(items));

    }

    // [Test]
    // public void TestPrefabInstantiation()
    // {
    //     
    // }

    [UnityTest]
    public IEnumerator TestObservingMultipleElements()
    {
        DataEntity entity = new DataEntity(_notifyManager);
        HealthElement health = new HealthElement(entity);
        CashElement cash = new CashElement(entity);

        cash.Cash = 123;
        health.Health = 456;
        GameObject go = new GameObject();
        var dataSource = go.AddComponent<EntitySource>();
        var multipleObserver = go.AddComponent<MultipleObserver>();
        dataSource.Entity = entity;
        yield return null;
        Assert.That(multipleObserver.Cash, Is.EqualTo(cash.Cash));
        Assert.That(multipleObserver.Health, Is.EqualTo(health.Health));

    }

    private static void AddChildren(DataEntity parent, INotifyManager notifyManager, int numChildren)
    {
        for (int i = 0; i < numChildren; i++)
        {
            parent.AddChild(new DataEntity(notifyManager, $"Child {i}"));
        }
    }

}
