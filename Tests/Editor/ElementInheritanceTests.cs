using com.enemyhideout.soong;

namespace Tests.Runtime
{
  public class SuperClassElement : DataElement
  {
  }

  public class SubClassElement : SuperClassElement
  {
  }

  public class InterfaceElement : DataElement, IElement
  {
  }
  
  public class SubInterfaceElement : SubClassElement, IElement
  {
  }
  
  public interface IElement
  {
  }

  public interface ISubElement : IElement
  {
  }

  public class MultiInterfaceElement : DataElement, ISubElement
  {
  }

  public class NotADataElement
  {
  }
}