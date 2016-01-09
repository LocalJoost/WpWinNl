using System;
using System.Reflection;

namespace WpWinNl.Utilities
{
  public class GetSetGeneric<T, TR> : GetSet
  {
    public delegate TR GetValue(T obj);
    public delegate void SetValue(T obj, TR value);
    private readonly GetValue _get;
    private readonly SetValue _set;

    public GetSetGeneric(PropertyInfo info)
    {
      MethodInfo getMethod;
      MethodInfo setMethod = null;
      Name = info.Name;
      Info = info;
      CollectionType = Info.PropertyType.GetInterface("IEnumerable", true) != null;

      getMethod = info.GetMethod;
      setMethod = info.SetMethod;

      _get = (GetValue)getMethod.CreateDelegate(typeof(GetValue));
      if (setMethod != null) _set = (SetValue)setMethod.CreateDelegate(typeof(SetValue));
    }

    public GetSetGeneric(FieldInfo info)
    {
      Name = info.Name;
      FieldInfo = info;
      _get = new GetValue(GetFieldValue);
      _set = new SetValue(SetFieldValue);
      CollectionType = FieldInfo.FieldType.GetInterface("IEnumerable", true) != null;

      return;
    }

    public GetSetGeneric(string name)
    {
      Name = name;
      MethodInfo getMethod;
      MethodInfo setMethod = null;
      var t = typeof(T);
      //var p = t.GetProperty(name); JSC
      var p = t.GetRuntimeProperty(name);
      if (p == null)
      {
        // FieldInfo = typeof(T).GetField(Name); JSC
        FieldInfo = typeof(T).GetRuntimeField(Name);
        _get = new GetValue(GetFieldValue);
        _set = new SetValue(SetFieldValue);
        CollectionType = FieldInfo.FieldType.GetInterface("IEnumerable", true) != null;
        return;
      }
      Info = p;
      CollectionType = Info.PropertyType.GetInterface("IEnumerable", true) != null;
      getMethod = p.GetMethod;
      setMethod = p.SetMethod;
      _get = (GetValue)getMethod.CreateDelegate(typeof(GetValue));
      if (setMethod != null) _set = (SetValue)setMethod.CreateDelegate(typeof(SetValue));
    }

    private TR GetFieldValue(T obj)
    {
      var result = (TR)FieldInfo.GetValue(obj);
      return result;
    }

    private void SetFieldValue(T obj, TR value)
    {
      FieldInfo.SetValue(obj, value);
    }

    public override object Get(object item)
    {
      return _get((T)item);
    }

    public override void Set(object item, object value)
    {
      _set((T)item, (TR)value);
    }
  }
}