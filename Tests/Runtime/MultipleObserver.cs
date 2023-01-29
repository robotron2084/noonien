using com.enemyhideout.soong;
using UnityEngine;

namespace Tests.Runtime
{
  public class MultipleObserver : ElementObserver<HealthElement>
  {
    public int Health;
    public int Cash;
    
    protected override void Awake()
    {
      base.Awake();
      Observe<CashElement>(CashUpdated);
    }

    private void CashUpdated(CashElement instance)
    {
      Cash = instance.Cash;
    }

    protected override void DataUpdated(HealthElement instance)
    {
      base.DataUpdated(instance);
      Health = instance.Health;
    }
  }
}