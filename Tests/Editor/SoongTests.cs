using System.Collections;
using System.Collections.Generic;
using com.enemyhideout.soong;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class SoongTests
{
    [Test]
    public void SoongTestsSimplePasses()
    {
        DataModel model = new DataModel();
        Assert.That(model.Name, Is.EqualTo("Model 0"));
    }

    [Test]
    public void TestAddElement()
    {
        DataModel model = new DataModel();
        model.AddElement(new DataElement());
        Assert.That(model.Elements.Count, Is.EqualTo(1));
    }
    
}
