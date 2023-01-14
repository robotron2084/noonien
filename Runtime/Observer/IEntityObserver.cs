using System;
using System.Collections.Generic;
using com.enemyhideout.soong;

namespace com.enemyhideout.soong
{
  public interface IEntityObserver : IDataObserver<DataEntity>
  {
  }

  public class EntityObserver : DataObserver<DataEntity>, IEntityObserver
  {
    public EntityObserver(Action<DataEntity> callback) : base(callback)
    {
    }
  }

}