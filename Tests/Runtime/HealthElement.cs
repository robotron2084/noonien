using com.enemyhideout.soong;

namespace Tests.Runtime
{
  public class HealthElement : DataElement
  {

    public HealthElement(DataEntity parent) : base(parent) { }
    
    private int _health;
    public int Health
    {
      get
      {
        return _health;
      }
      set
      {
        SetProperty(value, ref _health);
      }
    }

  }
}