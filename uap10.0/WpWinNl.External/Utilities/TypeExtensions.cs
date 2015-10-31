using System;
using System.Linq;
using System.Reflection;

namespace WpWinNl.Utilities
{
  public static class TypeExtensions
  {
    public static Type GetInterface(this Type type, string name, bool ignoreCase)
    {
      return type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(
        p => string.Compare(p.Name, name, ignoreCase? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)==0) ;
    }
  }
}
