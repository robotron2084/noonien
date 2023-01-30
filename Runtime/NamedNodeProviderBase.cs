using UnityEngine;

namespace com.enemyhideout.noonien
{
  public abstract class NamedNodeProviderBase : NodeProvider
  {
    public string Query;
    
    public abstract NodeManager NodeManager { get;}

    public virtual void Start()
    {
      Node = NodeManager.Find(Query);
      if (Node == null)
      {
        Debug.Log($"Node '{Query}' was not found.");
      }
    }
  }
}