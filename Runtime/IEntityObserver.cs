using System.Collections.Generic;
using com.enemyhideout.soong;

namespace com.enemyhideout.soong
{
  public interface IEntityObserver
  {
    void EntityUpdated(DataEntity entity);
  }
}