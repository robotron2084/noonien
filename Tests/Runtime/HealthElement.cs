using com.enemyhideout.soong;
using DefaultNamespace;

namespace Tests.Runtime
{
  public class HealthElement : DataElement
  {

    public HealthElement(DataEntity parent, INotifyManager notifyManager) : base(parent, notifyManager) { }
    
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