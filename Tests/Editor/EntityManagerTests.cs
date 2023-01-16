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
    
    private static DataEntity singleChildRoot = new DataEntity(null,"root");

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
        Query = "FirstChild",
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
        Query = "SecondChild.GrandChild",
        Root = singleChildRoot,
        ExpectedValue = secondGrandChild
      },
      new FindTestCase
      {
        Description = "GreatChild",
        Query = "SecondChild.GrandChild.GreatChild",
        Root = singleChildRoot,
        ExpectedValue = greatGrandChild
      },
      new FindTestCase
      {
        Description = "Partially Invalid Query",
        Query = "FirstChild.SecondChild",
        Root = singleChildRoot,
        ExpectedValue = null
      }

    };
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

    public static DataEntity AddNewChild(this DataEntity entity, string name)
    {
      var child = new DataEntity(entity.NotifyManager, name, entity);
      entity.AddChild(child);
      return child;
    }

  }
}