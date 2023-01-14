using System;
using System.Collections;
using System.Collections.Generic;
using com.enemyhideout.soong;
using NUnit.Framework;
using Tests.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

namespace com.enemyhideout.soong.tests
{
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
            var element = new SuperClassElement(entity);
            Assert.That(entity.ElementsCount, Is.EqualTo(1));
            Assert.That(element.Parent, Is.EqualTo(entity));
        }

        [Test]
        public void TestAddChild()
        {
            DataEntity parent = new DataEntity(null);
            DataEntity parent2 = new DataEntity(null);
            DataEntity child = new DataEntity(null);
            parent.AddChild(child);

            Assert.That(parent.ChildrenCount, Is.EqualTo(1));

            DataEntity childRetVal = parent.GetChildAt(0);
            Assert.That(childRetVal, Is.EqualTo(child));
            Assert.That(child.Parent, Is.EqualTo(parent));
            Assert.That(parent.Parent, Is.Null);

            parent2.AddChild(child);

            Assert.That(parent.ChildrenCount, Is.EqualTo(0));
            Assert.That(child.Parent, Is.EqualTo(parent2));
            Assert.That(parent2.GetChildAt(0), Is.EqualTo(child));

            child.RemoveParent();
            Assert.That(child.Parent, Is.Null);
            Assert.That(parent2.ChildrenCount, Is.EqualTo(0));
            Assert.That(parent2.Children.Count, Is.EqualTo(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => parent2.GetChildAt(0));
            
            parent.AddChild(child);
            parent.InsertChild(0, child);
            Assert.That(child.Parent, Is.EqualTo(parent));
            Assert.That(parent.ChildrenCount, Is.EqualTo(1));
            Assert.That(parent.Children[0], Is.EqualTo(child));

            DataEntity child2 = new DataEntity(null);
            parent.InsertChild(0,child2);
            Assert.That(child2.Parent, Is.EqualTo(parent));
            Assert.That(parent.ChildrenCount, Is.EqualTo(2));
            Assert.That(parent.Children[0], Is.EqualTo(child2));

        }

        [Test]
        public void TestGetElementSuperClass()
        {
            var entity = new DataEntity(null);
            var subElement = new SubClassElement(entity);

            var retVal = entity.GetElement<SuperClassElement>();
            Assert.That(retVal, Is.EqualTo(subElement));
        }

        [Test]
        public void TestGetElementSubClass()
        {
            var entity = new DataEntity(null);
            var subElement = new SubClassElement(entity);

            var retVal = entity.GetElement<SubClassElement>();
            Assert.That(retVal, Is.EqualTo(subElement));
        }
        
        [Test]
        public void TestGetElementSubClassMixed()
        {
            var entity = new DataEntity(null);
            var subElement = new SubClassElement(entity);
            var superElement = new SuperClassElement(entity);
            
            var retVal = entity.GetElement<SuperClassElement>();
            Assert.That(retVal, Is.EqualTo(superElement));
            
        }

        [Test]
        public void TestGetElementSubClassMixedOrder2()
        {
            var entity = new DataEntity(null);
            var superElement = new SuperClassElement(entity);
            var subElement = new SubClassElement(entity);

            // An odd and perhaps benign issue: it is not possible to gain access to SuperClass due to the order of 
            // addition of elements! Not sure if I care?
            var retVal = entity.GetElement<SuperClassElement>();
            Assert.That(retVal, Is.EqualTo(subElement));
            
        }

    }
}