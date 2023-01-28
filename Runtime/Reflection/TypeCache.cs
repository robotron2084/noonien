using System;
using System.Collections.Generic;
using System.Linq;

namespace com.enemyhideout.soong.Reflection
{
  public class TypeCache<TBase>
  {

    private Dictionary<Type, IEnumerable<Type>> _typeMap = new Dictionary<Type, IEnumerable<Type>>();

    public IEnumerable<Type> GetTypes(Type t)
    {
      if(_typeMap.TryGetValue(t, out IEnumerable<Type> IEnumerable))
      {
        return IEnumerable;
      }else
      {
        IEnumerable<Type> types = BuildTypes(t);
        _typeMap[t] = types;
        return types;
      }
    }

    public static IEnumerable<Type> BuildTypes(Type t)
    {
      if (!(typeof(TBase).IsAssignableFrom(t)))
      {
        throw new ArgumentException($"Type {t} is not of type {typeof(TBase)}");
      }
      List<Type> retVal = new List<Type>();
      Type baseClass = t;
      Type superType = typeof(TBase);
      while (baseClass != superType)
      {
        retVal.Add(baseClass);
        baseClass = baseClass.BaseType;
      }

      var baseInterfaces = superType.GetInterfaces();
      var typeInterfaces = t.GetInterfaces();
      var interfacesExceptBase = typeInterfaces.Except(baseInterfaces);
      retVal.AddRange(interfacesExceptBase);

      return retVal;
    }
    
  }
}