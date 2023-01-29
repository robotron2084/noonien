using com.enemyhideout.soong;

namespace Tests.Runtime
{
  public class CashElement : DataElement
  {
    public CashElement(DataEntity parent) : base(parent)
    {
      
    }
    
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