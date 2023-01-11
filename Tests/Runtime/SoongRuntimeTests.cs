using System.Collections;
using System.Collections.Generic;
using com.enemyhideout.soong;
using NUnit.Framework;
using Tests.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

public class SoongRuntimeTests
{
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
        HealthElement health = new HealthElement();
        DataModel model = new DataModel(health);

        health.Health = 10;

        GameObject go = new GameObject();
        var dataSource = go.AddComponent<DataSource>();
        var healthObserver = go.AddComponent<HealthObserver>();

        GameObject go2 = new GameObject();
        var dataSource2 = go2.AddComponent<DataSource>();
        var healthObserver2 = go2.AddComponent<HealthObserver>();

        dataSource.Model = model;
        // healthObserver won't initialize until start, so let's wait.
        yield return null;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));

        health.Health = 9;
        
        health.NotifyUpdated();
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));

        dataSource.Model = null;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(-1));

        // Ensure that the observer isn't receiving notifications.
        health.Health = 8;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(-1));
        
        // glue to data source2.
        dataSource2.Model = model;
        Assert.That(healthObserver2.ObservedHealth, Is.EqualTo(health.Health));
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(-1));

        dataSource.Model = model;
        Assert.That(healthObserver.ObservedHealth, Is.EqualTo(health.Health));
        Assert.That(healthObserver2.ObservedHealth, Is.EqualTo(health.Health));
        
    }
}
