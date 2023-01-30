using System.Collections.Generic;
using com.enemyhideout.soong;
using NUnit.Framework;
using Tests.Editor;
using Tests.Runtime;

namespace com.enemyhideout.soong.tests
{
  [TestFixture]
  public class EntityManagerTests
  {
    public class FindTestCase : TestBase
    {
      public string Query;
      public DataEntity Root;
      public DataEntity ExpectedValue;
    }

    [Test]
    public void TestFind([ValueSource(nameof(FindTestCases))] FindTestCase testCase)
    {
      var output = EntityManager.FindInternal(testCase.Query, testCase.Root);
      Assert.That(output, Is.EqualTo(testCase.ExpectedValue));
    }
    
    private static DataEntity singleChildRoot = new DataEntity(null,"Root");

    private static DataEntity rootFirstChild = singleChildRoot.AddNewChild("FirstChild");
    private static DataEntity rootSecondChild = singleChildRoot.AddNewChild("SecondChild");
    private static DataEntity rootThirdChild = singleChildRoot.AddNewChild("ThirdChild");
    private static DataEntity secondGrandChild = rootSecondChild.AddNewChild("GrandChild");
    private static DataEntity greatGrandChild = secondGrandChild.AddNewChild("GreatChild");

    public static List<FindTestCase> FindTestCases = new List<FindTestCase>()
    {
      new FindTestCase
      {
        Description = "Only Child",
        Query = "Root.FirstChild",
        Root = singleChildRoot,
        ExpectedValue = singleChildRoot.Children[0]
      },
      new FindTestCase
      {
        Description = "Invalid Query",
        Query = "asdfds",
        Root = singleChildRoot,
        ExpectedValue = null
      },
      new FindTestCase
      {
        Description = "GrandChild",
        Query = "Root.SecondChild.GrandChild",
        Root = singleChildRoot,
        ExpectedValue = secondGrandChild
      },
      new FindTestCase
      {
        Description = "GreatChild",
        Query = "Root.SecondChild.GrandChild.GreatChild",
        Root = singleChildRoot,
        ExpectedValue = greatGrandChild
      },
      new FindTestCase
      {
        Description = "Partially Invalid Query",
        Query = "Root.FirstChild.SecondChild",
        Root = singleChildRoot,
        ExpectedValue = null
      }
    };
    
    [Test]
    public void TestAdd()
    {
      var root = new DataEntity(null);
      EntityManager manager = new EntityManager(root);
      var entity = manager.Add("MyEntity");
      Assert.That(entity, Is.Not.Null);
      Assert.That(entity.Parent, Is.EqualTo(root));
      Assert.That(entity.Name, Is.EqualTo("MyEntity"));

      var sameEntity = manager.Get("MyEntity");
      Assert.That(entity, Is.EqualTo(sameEntity));
    }

    [Test]
    public void TestRegisterAndGetEntity()
    {
      var root = new DataEntity(null);
      EntityManager manager = new EntityManager(root);
      var child = root.AddNewChild("Child");
      manager.Register("AnotherName", child);
      var sameEntity = manager.Get("AnotherName");
      Assert.That(sameEntity, Is.EqualTo(child));
    }

    [Test]
    public void TestRegisterAndGetElement()
    {
      var root = new DataEntity(null);
      EntityManager manager = new EntityManager(root);
      var child = root.AddNewChild("Child");
      var cashElement = child.AddElement<CashElement>();
      manager.Register("AnotherName", cashElement);
      var sameEntity = manager.Get<CashElement>("AnotherName");
      Assert.That(sameEntity, Is.EqualTo(cashElement));
    }

    [Test]
    public void TestRegisterAndGetCollection()
    {
      var root = new DataEntity(null);
      EntityManager manager = new EntityManager(root);
      var child = root.AddNewChild("Child");
      var cashElement = child.AddElement<CashElement>();
      manager.Register("AnotherName", root.Children);
      var sameEntity = manager.GetCollection("AnotherName");
      Assert.That(sameEntity, Is.EqualTo(root.Children));
      
      var sameEntityGeneric = manager.GetCollection<DataEntity>("AnotherName");
      Assert.That(sameEntityGeneric, Is.EqualTo(root.Children));

    }

  }
  
  

  public static class EntityExtensions
  {
    public static DataEntity AddChildren(this DataEntity entity, params string[] names )
    {
      foreach (var name in names)
      {
        entity.AddNewChild(name);
      }

      return entity;
    }
    
  }
}