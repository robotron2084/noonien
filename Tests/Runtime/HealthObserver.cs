using com.enemyhideout.noonien;
using UnityEngine;

namespace Tests.Runtime
{
  public class HealthObserver : ElementObserver<HealthElement>
  {

    public int ObservedHealth = 0;
    protected override void DataUpdated(HealthElement element)
    {
      base.DataUpdated(element);
      ObservedHealth = element.Health;
    }

    protected override void DataRemoved(HealthElement element)
    {
      base.DataRemoved(element);
      ObservedHealth = -1;
    }
  }
}