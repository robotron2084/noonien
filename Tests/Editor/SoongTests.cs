using System;
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
        DataEntity.__index = 0;
        DataEntity entity = new DataEntity(null);
        Assert.That(entity.Name, Is.EqualTo("Model 0"));

        DataEntity model2 = new DataEntity(null, "My Name");
        Assert.That(model2.Name, Is.EqualTo("My Name"));
        
    }

    [Test]
    public void TestAddElement()
    {
        DataEntity entity = new DataEntity(null);
        new DataElement(entity);
        Assert.That(entity.ElementsCount, Is.EqualTo(1));
    }

    [Test]
    public void TestAddChild()
    {
        DataEntity parent = new DataEntity(null);
        DataEntity parent2 = new DataEntity(null);
        DataEntity child = new DataEntity(null);
        child.Parent = parent;
        
        Assert.That(parent.ChildrenCount, Is.EqualTo(1));

        DataEntity childRetVal = parent.GetChildAt(0);
        Assert.That(childRetVal, Is.EqualTo(child));
        Assert.That(child.Parent, Is.EqualTo(parent));
        Assert.That(parent.Parent, Is.Null);

        child.Parent = parent2;
        
        Assert.That(parent.ChildrenCount, Is.EqualTo(0));
        Assert.That(child.Parent, Is.EqualTo(parent2));
        Assert.That(parent2.GetChildAt(0), Is.EqualTo(child));

        child.Parent = null;
        Assert.That(child.Parent, Is.Null);
        Assert.That(parent2.ChildrenCount, Is.EqualTo(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => parent2.GetChildAt(0));
    }



    
}
