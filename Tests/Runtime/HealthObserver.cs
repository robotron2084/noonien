using com.enemyhideout.soong;
using UnityEngine;

namespace Tests.Runtime
{
  public class HealthObserver : ElementObserver<HealthElement>
  {

    public int ObservedHealth = 0;
    protected override void DataUpdated(HealthElement instance)
    {
      base.DataUpdated(instance);
      ObservedHealth = instance.Health;
    }

    protected override void DataRemoved(HealthElement instance)
    {
      base.DataRemoved(instance);
      ObservedHealth = -1;
    }
  }
}