using com.enemyhideout.noonien;
using UnityEngine;

namespace Tests.Runtime
{
  public class TestUpdateBehavior : MonoBehaviour
  {
    public INotifyManager _NotifyManager;
    public int TestValue;

    public void TriggerUpdate()
    {
      _NotifyManager.EnqueueNotifier(Callback);
    }

    public void Callback()
    {
      TestValue = 42;
    }
  }
}