using System;
using System.Collections.Generic;
using com.enemyhideout.noonien;

namespace com.enemyhideout.noonien
{
  public interface INodeObserver : IDataObserver<Node>
  {
  }

  internal class NodeObserver : DataObserver<Node>, INodeObserver
  {
    public NodeObserver(Action<Node> callback) : base(callback)
    {
    }
  }

}