using com.enemyhideout.soong;
using UnityEngine;

namespace Tests.Runtime
{
  public class DependentUpdateBehaviour : MonoBehaviour
  {
    public DependentUpdateBehaviour ItemToTrigger;
    public INotifyManager NotifyManager;
    public int TestValue = 0;

    public void TriggerUpdate()
    {
      NotifyManager.EnqueueNotifier(Callback);
    }

    private void Callback()
    {
      TestValue = 42;
      if (ItemToTrigger != null)
      {
        ItemToTrigger.TriggerUpdate();
      }
    }
  }
}