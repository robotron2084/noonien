using Code.Data;
using com.enemyhideout.noonien;
using UnityEngine;

namespace Code.Views
{
  public class Trails : ElementObserver<Unit>
  {
    [SerializeField]
    private TrailRenderer _renderer;
    
    protected override void DataUpdated(Unit element)
    {
      base.DataUpdated(element);
      _renderer.enabled = element.Alive;
      if (element.HasEvent(Unit.KilledEventId))
      {
        _renderer.Clear();
      }
    }
  }
}