using com.enemyhideout.noonien;
using UnityEngine;

namespace PongSample.Code
{
  public class PongNodeProvider : NamedNodeProviderBase
  {
    private NodeManager _nodeManager;

    public override NodeManager NodeManager
    {
      get
      {
        return GameController.NodeManager;
      }
    }
  }
}