using System.Collections.Generic;
using com.enemyhideout.soong;
using NUnit.Framework;
using UnityEngine;

namespace com.enemyhideout.soong.tests
{
  [TestFixture]
  public class CollectionDeltaTesting
  {
    
    [Test]
    public void TestComputeDelta([ValueSource(nameof(ComputeDeltaTestCases))] ComputeDeltaTestCase testCase)
    {
      var output = CollectionDelta<object>.ComputeDelta(testCase.Original, testCase.NewList);
      Assert.That( output, Is.EqualTo(testCase.Changes));
    }

    public class ComputeDeltaTestCase
    {
      public string Description;
      public List<object> Original;
      public List<object> NewList;
      public List<CollectionChange<object>> Changes;
 
      public override string ToString()
      {
        return Description;
      }
    }

    public class TestObject
    {
      private string _name;

      public TestObject(string name)
      {
        _name = name;
      }

      public override string ToString()
      {
        return $"[TestObject {_name}]";
      }
    }

    private static object ObjectA = new TestObject("A");
    private static object ObjectB = new TestObject("B");
    private static object ObjectC = new TestObject("C");
    private static object ObjectD = new TestObject("D");

    
    
    public static IEnumerable<ComputeDeltaTestCase> ComputeDeltaTestCases = new List<ComputeDeltaTestCase>()
    { 
      new ComputeDeltaTestCase
      {
        Description = "Empty List Equality Test",
        Original = new List<object>(),
        NewList = new List<object>(),
        Changes = new List<CollectionChange<object>>()
      },
      new ComputeDeltaTestCase
      {
        Description = "One Item List Equality Test",
        Original = new List<object>(){ 1 },
        NewList = new List<object>() { 1 },
        Changes = new List<CollectionChange<object>>()
      },
      new ComputeDeltaTestCase
      {
        Description = "One Item List, item removed",
        Original = new List<object>(){ 1 },
        NewList = new List<object>(),
        Changes = new List<CollectionChange<object>>()
        {
          new CollectionChange<object>
          {
            Item = 1,
            OldIndex = 0,
            NewIndex = -1,
            Action = CollectionChangeAction.Removed
          }
        }
      },
      new ComputeDeltaTestCase
      {
        Description = "One Item List, item added",
        Original = new List<object>(),
        NewList = new List<object>(){1},
        Changes = new List<CollectionChange<object>>()
        {
          new CollectionChange<object>
          {
            Item = 1,
            OldIndex = -1,
            NewIndex = 0,
            Action = CollectionChangeAction.Added
          }
        }
      },
      new ComputeDeltaTestCase
      {
        Description = "Add/Move/Remove Ints",
        Original = new List<object>(){5,6,7},
        NewList = new List<object>(){7,8,9},
        Changes = new List<CollectionChange<object>>()
        {
          new CollectionChange<object>
          {
            Item = 7,
            OldIndex = 2,
            NewIndex = 0,
            Action = CollectionChangeAction.Moved
          },
          new CollectionChange<object>
          {
            Item = 8,
            OldIndex = -1,
            NewIndex = 1,
            Action = CollectionChangeAction.Added
          },
          new CollectionChange<object>
          {
            Item = 9,
            OldIndex = -1,
            NewIndex = 2,
            Action = CollectionChangeAction.Added
          },
          new CollectionChange<object>
          {
            Item = 5,
            OldIndex = 0,
            NewIndex = -1,
            Action = CollectionChangeAction.Removed
          },
          new CollectionChange<object>
          {
            Item = 6,
            OldIndex = 1,
            NewIndex = -1,
            Action = CollectionChangeAction.Removed
          }
        }
      },
      new ComputeDeltaTestCase
      {
        Description = "Move Object From Beginning to end",
        Original = new List<object>(){ObjectA, ObjectB, ObjectC, ObjectD},
        NewList = new List<object>(){ObjectD, ObjectA, ObjectB, ObjectC},
        Changes = new List<CollectionChange<object>>()
        {
          new CollectionChange<object>
          {
            Item = ObjectD,
            OldIndex = 3,
            NewIndex = 0,
            Action = CollectionChangeAction.Moved
          },
          new CollectionChange<object>
          {
            Item = ObjectA,
            OldIndex = 0,
            NewIndex = 1,
            Action = CollectionChangeAction.Moved
          },
          new CollectionChange<object>
          {
            Item = ObjectB,
            OldIndex = 1,
            NewIndex = 2,
            Action = CollectionChangeAction.Moved
          },
          new CollectionChange<object>
          {
            Item = ObjectC,
            OldIndex = 2,
            NewIndex = 3,
            Action = CollectionChangeAction.Moved
          },
          }
        },
      new ComputeDeltaTestCase
      {
        Description = "Add Object To Beginning",
        Original = new List<object>(){ObjectA, ObjectB, ObjectC},
        NewList = new List<object>(){ObjectD, ObjectA, ObjectB, ObjectC},
        Changes = new List<CollectionChange<object>>()
        {
          new CollectionChange<object>
          {
            Item = ObjectD,
            OldIndex = -1,
            NewIndex = 0,
            Action = CollectionChangeAction.Added
          },
          new CollectionChange<object>
          {
            Item = ObjectA,
            OldIndex = 0,
            NewIndex = 1,
            Action = CollectionChangeAction.Moved
          },
          new CollectionChange<object>
          {
            Item = ObjectB,
            OldIndex = 1,
            NewIndex = 2,
            Action = CollectionChangeAction.Moved
          },
          new CollectionChange<object>
          {
            Item = ObjectC,
            OldIndex = 2,
            NewIndex = 3,
            Action = CollectionChangeAction.Moved
          },
        }
      },
      new ComputeDeltaTestCase
      {
        Description = "Add Object To End",
        Original = new List<object>(){ObjectA, ObjectB, ObjectC},
        NewList = new List<object>(){ObjectA, ObjectB, ObjectC, ObjectD},
        Changes = new List<CollectionChange<object>>()
        {
          new CollectionChange<object>
          {
            Item = ObjectD,
            OldIndex = -1,
            NewIndex = 3,
            Action = CollectionChangeAction.Added
          },
        }
      }
    };
  }
}