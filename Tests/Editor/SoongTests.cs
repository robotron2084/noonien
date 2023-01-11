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
        DataEntity entity = new DataEntity();
        DataEntity.__index = 0;
        Assert.That(entity.Name, Is.EqualTo("Model 0"));

        DataEntity model2 = new DataEntity("My Name");
        Assert.That(model2.Name, Is.EqualTo("My Name"));
        
    }

    [Test]
    public void TestAddElement()
    {
        DataEntity entity = new DataEntity();
        new DataElement(entity, null);
        Assert.That(entity.ElementsCount, Is.EqualTo(1));
    }

    [Test]
    public void TestAddChild()
    {
        DataEntity parent = new DataEntity();
        DataEntity child = new DataEntity();
        parent.AddChild(child);
        
        Assert.That(parent.ChildrenCount, Is.EqualTo(1));

        DataEntity childRetVal = parent.GetChildAt(0);
        Assert.That(childRetVal, Is.EqualTo(child));
        Assert.That(child.Parent, Is.EqualTo(parent));
        Assert.That(parent.Parent, Is.Null);

        child.Parent = null;
        
        Assert.That(parent.ChildrenCount, Is.EqualTo(0));
        Assert.That(child.Parent, Is.Null);
        
    }



    
}
