using Code.Data;
using com.enemyhideout.soong;
using UnityEngine;

namespace Code.Views
{
  public class Mover : ElementObserver<Unit>
  {
    private Transform _t;
    [SerializeField]
    private Transform _cube;
    protected override void Awake()
    {
      base.Awake();
      _t = transform;
    }

    protected override void DataUpdated(Unit instance)
    {
      base.DataUpdated(instance);
      var pos = instance.Position;
      _t.position = new Vector3(pos.x, 0, pos.y);
      _cube.localScale = new Vector3(instance.Bounds.width, _cube.localScale.y, instance.Bounds.height);
    }
  }
}