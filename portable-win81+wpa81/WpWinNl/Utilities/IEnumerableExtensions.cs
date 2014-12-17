using System;
using System.Collections.Generic;
using System.Linq;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Helper methods to support ForEach and ConvertAll to all IEnumerable collections
  /// </summary>
  public static class IEnumerableExtensions
  {
    public static void ForEach<T>(this IEnumerable<T> t, Action<T> action)
    {
      foreach (var item in t)
      {
        action(item);
      }
    }

#if WINDOWS_PHONE
    public static IEnumerable<TC> ConvertAll<T, TC>(
                                  this IEnumerable<T> inputList,
                                  Converter<T, TC> convert)
    {
      return inputList.Select(t => convert(t));
    }
#endif
  }
}
