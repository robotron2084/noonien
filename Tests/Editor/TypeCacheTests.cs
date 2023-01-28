using System;
using System.Collections.Generic;
using com.enemyhideout.soong;
using IObservable = com.enemyhideout.soong.IObservable<com.enemyhideout.soong.DataElement>;
using com.enemyhideout.soong.Reflection;
using NUnit.Framework;
using Tests.Runtime;

namespace Tests.Editor
{
  [TestFixture]
  public class TypeCacheTests
  {

    [Test]
    public void TestBuildTypes([ValueSource(nameof(BuildTypesTestCases))] BuildTypesTestCase testCase)
    {
      var output = TypeCache<DataElement>.BuildTypes(testCase.Input);
      Assert.That(output, Is.EqualTo(testCase.Expected));
    }

    [Test]
    public void TestBuildTypesThrowsOnBadType()
    {
      Assert.Throws<ArgumentException>(() => TypeCache<DataElement>.BuildTypes(typeof(NotADataElement)));
    }

    
    public class BuildTypesTestCase
    {

      public string Description;
      public Type Input;
      public List<Type> Expected;

      public override string ToString()
      {
        return Description;
      }
    }
    

    public static List<BuildTypesTestCase> BuildTypesTestCases = new List<BuildTypesTestCase>()
    {
      new BuildTypesTestCase
      {
        Description = "Base DataElement Should Return nothing.",
        Input = typeof(DataElement),
        Expected = new List<Type>()
      },
      new BuildTypesTestCase
      {
        Description = "Super DataElement Should Return itself.",
        Input = typeof(SuperClassElement),
        Expected = new List<Type>() { typeof(SuperClassElement) }
      },
      new BuildTypesTestCase
      {
        Description = "Sub DataElement Should Return itself and superclass.",
        Input = typeof(SubClassElement),
        Expected = new List<Type>() { typeof(SubClassElement), typeof(SuperClassElement) }
      },
      new BuildTypesTestCase
      {
        Description = "InterfaceElement implements IElement and itself",
        Input = typeof(InterfaceElement),
        Expected = new List<Type>() { typeof(InterfaceElement), typeof(IElement) }
      },
      new BuildTypesTestCase
      {
        Description = "SubInterfaceElement implements IElement, itself, and subclasses",
        Input = typeof(SubInterfaceElement),
        Expected = new List<Type>() { typeof(SubInterfaceElement),typeof(SubClassElement), typeof(SuperClassElement), typeof(IElement) }
      },
      new BuildTypesTestCase
      {
        Description = "Multi interface element",
        Input = typeof(MultiInterfaceElement),
        Expected = new List<Type>() { typeof(MultiInterfaceElement), typeof(IElement), typeof(ISubElement) }
      },
    };
  }
}