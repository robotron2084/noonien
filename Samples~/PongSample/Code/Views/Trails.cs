using Code.Data;
using com.enemyhideout.soong;
using UnityEngine;

namespace Code.Views
{
  public class Trails : ElementObserver<Unit>
  {
    [SerializeField]
    private TrailRenderer _renderer;
    
    protected override void DataUpdated(Unit instance)
    {
      base.DataUpdated(instance);
      _renderer.enabled = instance.Alive;
      if (!instance.Alive)
      {
        _renderer.Clear();
      }
    }
  }
}