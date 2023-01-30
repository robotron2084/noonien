using System;
using System.Collections;
using System.Collections.Generic;
using com.enemyhideout.noonien;
using NUnit.Framework;
using Tests.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

namespace com.enemyhideout.noonien.tests
{
    public class NoonienTests
    {
        [Test]
        public void SoongTestsNaming()
        {
            Node.__index = 0;
            Node node = new Node(null);
            Assert.That(node.Name, Is.EqualTo("Model 0"));

            Node model2 = new Node(null, "My Name");
            Assert.That(model2.Name, Is.EqualTo("My Name"));

        }

        [Test]
        public void TestAddElement()
        {
            Node node = new Node(null);
            var element = node.AddElement<SuperClassElement>();
            Assert.That(node.ElementsCount, Is.EqualTo(1));
            Assert.That(element.Parent, Is.EqualTo(node));
        }

        [Test]
        public void TestAddChild()
        {
            Node parent = new Node(null);
            Node parent2 = new Node(null);
            Node child = new Node(null);
            parent.AddChild(child);

            Assert.That(parent.ChildrenCount, Is.EqualTo(1));

            Node childRetVal = parent.GetChildAt(0);
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

            Node child2 = new Node(null);
            parent.InsertChild(0,child2);
            Assert.That(child2.Parent, Is.EqualTo(parent));
            Assert.That(parent.ChildrenCount, Is.EqualTo(2));
            Assert.That(parent.Children[0], Is.EqualTo(child2));

        }

        [Test]
        public void TestGetElementSuperClass()
        {
            var node = new Node(null);
            var subElement = node.AddElement<SubClassElement>();

            var retVal = node.GetElement<SuperClassElement>();
            Assert.That(retVal, Is.EqualTo(subElement));
        }

        [Test]
        public void TestGetElementSubClass()
        {
            var node = new Node(null);
            var subElement = node.AddElement<SubClassElement>();

            var retVal = node.GetElement<SubClassElement>();
            Assert.That(retVal, Is.EqualTo(subElement));
        }
        
        [Test]
        public void TestGetElementSubClassMixed()
        {
            var node = new Node(null);
            var subElement = node.AddElement<SubClassElement>();
            var superElement = node.AddElement<SuperClassElement>();
            
            var retVal = node.GetElement<SuperClassElement>();
            Assert.That(retVal, Is.EqualTo(superElement));
            
        }

        [Test]
        public void TestGetElementSubClassMixedOrder2()
        {
            var node = new Node(null);
            var superElement = node.AddElement<SuperClassElement>();
            var subElement = node.AddElement<SubClassElement>();

            // An odd and perhaps benign issue: it is not possible to gain access to SuperClass due to the order of 
            // addition of elements! Not sure if I care?
            var retVal = node.GetElement<SuperClassElement>();
            Assert.That(retVal, Is.EqualTo(subElement));
        }
        
        [Test]
        public void TestGetElementInterface()
        {
            var node = new Node(null);
            var element = node.AddElement<InterfaceElement>();
            var retVal = node.GetElement<IElement>();
            Assert.That(retVal, Is.EqualTo(element));
        }
        
        [Test]
        public void TestGetElementThatDoesntExist()
        {
            var node = new Node(null);
            var retVal = node.GetElement<IElement>();
            Assert.That(retVal, Is.Null);
        }



    }
}