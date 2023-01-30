using com.enemyhideout.noonien;

namespace Tests.Runtime
{
  public class HealthElement : Element
  {

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