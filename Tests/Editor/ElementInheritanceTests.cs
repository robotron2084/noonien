using com.enemyhideout.soong;

namespace Tests.Runtime
{
  public class SuperClassElement : DataElement
  {
    public SuperClassElement(DataEntity parent) : base(parent)
    {
    }
  }

  public class SubClassElement : SuperClassElement
  {
    public SubClassElement(DataEntity parent) : base(parent)
    {
    }
  }


  public class InterfaceElement : DataElement, IElement
  {
    public InterfaceElement(DataEntity parent) : base(parent)
    {
    }
  }
  
  public class SubInterfaceElement : SubClassElement, IElement
  {
    public SubInterfaceElement(DataEntity parent) : base(parent)
    {
    }
  }
  
  public interface IElement
  {
  }

  public interface ISubElement : IElement
  {
  }

  public class MultiInterfaceElement : DataElement, ISubElement
  {
    public MultiInterfaceElement(DataEntity parent) : base(parent)
    {
    }
  }

  public class NotADataElement
  {
  }
}