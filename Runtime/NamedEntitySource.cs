using UnityEngine;

namespace com.enemyhideout.soong
{
  public abstract class NamedEntitySource : EntitySource
  {
    public string Query;
    
    public abstract EntityManager EntityManager { get;}

    public virtual void Start()
    {
      Entity = EntityManager.Find(Query);
    }
  }
}