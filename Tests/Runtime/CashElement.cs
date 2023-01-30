using com.enemyhideout.noonien;

namespace Tests.Runtime
{
  public class CashElement : Element
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