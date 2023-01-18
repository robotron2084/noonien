using System.Collections.Generic;

namespace com.enemyhideout.soong
{
  public class EventBuffer
  {
    private bool _dirty = false;
    private INotifyManager _notifyManager;
    private Dictionary<string, DataEvent> _events;

    public EventBuffer(INotifyManager notifyManager)
    {
      _notifyManager = notifyManager;
    }
    
    public void EnqueueEvent(DataEvent evt)
    {
      if (_events == null)
      {
        _events = new Dictionary<string, DataEvent>();
      }
      _events[evt.Id] = evt;
      MarkDirty();
    }

    private void MarkDirty()
    {
      if (!_dirty)
      {
        _dirty = true;
        _notifyManager?.EnqueueNotifier(Clear, NotifyManager.LateUpdate);
      }
    }

    public T EventForId<T>(string id) where T : DataEvent
    {
      if (_events == null)
      {
        return null;
      }
      DataEvent retVal = null;
      _events.TryGetValue(id, out retVal);
      return retVal as T;
    }

    public void Clear()
    {
      if (_events == null)
      {
        return;
      }

      _dirty = false;
      _events.Clear();
    }

    public bool HasEvent(string id)
    {
      if (_events == null)
      {
        return false;
      }

      return _events.ContainsKey(id);
    }
    public static void ClearBuffer(EventBuffer buffer)
    {
      buffer.Clear();
    }

  }
}