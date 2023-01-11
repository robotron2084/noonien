using System.Collections;
using System.Collections.Generic;
using com.enemyhideout.soong;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class SoongTests
{
    [Test]
    public void SoongTestsNaming()
    {
        DataModel model = new DataModel();
        DataModel.__index = 0;
        Assert.That(model.Name, Is.EqualTo("Model 0"));

        DataModel model2 = new DataModel("My Name");
        Assert.That(model2.Name, Is.EqualTo("My Name"));
        
    }

    [Test]
    public void TestAddElement()
    {
        DataModel model = new DataModel();
        model.AddElement(new DataElement());
        Assert.That(model.ElementsCount, Is.EqualTo(1));
    }

    [Test]
    public void TestAddChild()
    {
        DataModel parent = new DataModel();
        DataModel child = new DataModel();
        parent.AddChild(child);
        
        Assert.That(parent.ChildrenCount, Is.EqualTo(1));

        DataModel childRetVal = parent.GetChildAt(0);
        Assert.That(childRetVal, Is.EqualTo(child));
        Assert.That(child.Parent, Is.EqualTo(parent));
        Assert.That(parent.Parent, Is.Null);

        child.Parent = null;
        
        Assert.That(parent.ChildrenCount, Is.EqualTo(0));
        Assert.That(child.Parent, Is.Null);

        
        
    }



    
}
