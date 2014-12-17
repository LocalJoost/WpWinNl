using System;

namespace WpWinNl.Utilities
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class SerializerAttribute : Attribute
  {
    internal Type SerializesType;
    public SerializerAttribute(Type serializesType)
    {
      SerializesType = serializesType;
    }
  }
}