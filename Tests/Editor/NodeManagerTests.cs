using System.Collections.Generic;
using com.enemyhideout.noonien;
using NUnit.Framework;
using Tests.Editor;
using Tests.Runtime;

namespace com.enemyhideout.noonien.tests
{
  [TestFixture]
  public class NodeManagerTests
  {
    public class FindTestCase : TestBase
    {
      public string Query;
      public Node Root;
      public Node ExpectedValue;
    }

    [Test]
    public void TestFind([ValueSource(nameof(FindTestCases))] FindTestCase testCase)
    {
      var output = NodeManager.FindInternal(testCase.Query, testCase.Root);
      Assert.That(output, Is.EqualTo(testCase.ExpectedValue));
    }
    
    private static Node singleChildRoot = new Node(null,"Root");

    private static Node rootFirstChild = singleChildRoot.AddNewChild("FirstChild");
    private static Node rootSecondChild = singleChildRoot.AddNewChild("SecondChild");
    private static Node rootThirdChild = singleChildRoot.AddNewChild("ThirdChild");
    private static Node secondGrandChild = rootSecondChild.AddNewChild("GrandChild");
    private static Node greatGrandChild = secondGrandChild.AddNewChild("GreatChild");

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
      var root = new Node(null);
      NodeManager manager = new NodeManager(root);
      var node = manager.Add("Mynode");
      Assert.That(node, Is.Not.Null);
      Assert.That(node.Parent, Is.EqualTo(root));
      Assert.That(node.Name, Is.EqualTo("Mynode"));

      var samenode = manager.Get("Mynode");
      Assert.That(node, Is.EqualTo(samenode));
    }

    [Test]
    public void TestRegisterAndGetnode()
    {
      var root = new Node(null);
      NodeManager manager = new NodeManager(root);
      var child = root.AddNewChild("Child");
      manager.Register("AnotherName", child);
      var samenode = manager.Get("AnotherName");
      Assert.That(samenode, Is.EqualTo(child));
    }

    [Test]
    public void TestRegisterAndGetElement()
    {
      var root = new Node(null);
      NodeManager manager = new NodeManager(root);
      var child = root.AddNewChild("Child");
      var cashElement = child.AddElement<CashElement>();
      manager.Register("AnotherName", cashElement);
      var samenode = manager.Get<CashElement>("AnotherName");
      Assert.That(samenode, Is.EqualTo(cashElement));
    }

    [Test]
    public void TestRegisterAndGetCollection()
    {
      var root = new Node(null);
      NodeManager manager = new NodeManager(root);
      var child = root.AddNewChild("Child");
      var cashElement = child.AddElement<CashElement>();
      manager.Register("AnotherName", root.Children);
      var samenode = manager.GetCollection("AnotherName");
      Assert.That(samenode, Is.EqualTo(root.Children));
      
      var samenodeGeneric = manager.GetCollection<Node>("AnotherName");
      Assert.That(samenodeGeneric, Is.EqualTo(root.Children));

    }

  }
  
  

  public static class nodeExtensions
  {
    public static Node AddChildren(this Node node, params string[] names )
    {
      foreach (var name in names)
      {
        node.AddNewChild(name);
      }

      return node;
    }
    
  }
}