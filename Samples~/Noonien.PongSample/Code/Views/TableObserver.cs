using Code.Data;
using com.enemyhideout.noonien;
using UnityEngine;

namespace Code.Views
{
  public class TableObserver : ElementObserver<World>
  {
    protected override void DataUpdated(World element)
    {
      base.DataUpdated(element);
      transform.localScale = new Vector3(element.Bounds.width, 1.0f, element.Bounds.height);
    }
  }
}