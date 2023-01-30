using com.enemyhideout.noonien;

namespace Tests.Runtime
{
  public class SuperClassElement : Element
  {
  }

  public class SubClassElement : SuperClassElement
  {
  }

  public class InterfaceElement : Element, IElement
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

  public class MultiInterfaceElement : Element, ISubElement
  {
  }

  public class NotADataElement
  {
  }
}