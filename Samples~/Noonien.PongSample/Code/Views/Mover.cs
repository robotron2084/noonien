using Code.Data;
using com.enemyhideout.noonien;
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

    protected override void DataUpdated(Unit element)
    {
      base.DataUpdated(element);
      var pos = element.Position;
      _t.position = new Vector3(pos.x, 0, pos.y);
      _cube.localScale = new Vector3(element.Bounds.width, _cube.localScale.y, element.Bounds.height);
    }
  }
}