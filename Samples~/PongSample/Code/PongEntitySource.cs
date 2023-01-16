using com.enemyhideout.soong;
using UnityEngine;

namespace PongSample.Code
{
  public class PongEntitySource : NamedEntitySource
  {
    private EntityManager _entityManager;

    public override EntityManager EntityManager
    {
      get
      {
        return GameController.EntityManager;
      }
    }
  }
}