using com.enemyhideout.soong;

namespace Tests.Runtime
{
  public class CashElement : DataElement
  {
    private int _cash;
    public int Cash
    {
      get
      {
        return _cash;
      }
      set
      {
        SetProperty(value, ref _cash);
      }
    }
    
  }
}