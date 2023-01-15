using Code.Data;
using com.enemyhideout.soong;
using UnityEngine;

namespace Code.Views
{
  public class TableObserver : ElementObserver<World>
  {
    protected override void DataUpdated(World instance)
    {
      base.DataUpdated(instance);
      transform.localScale = new Vector3(instance.Bounds.width, 1.0f, instance.Bounds.height);
    }
  }
}