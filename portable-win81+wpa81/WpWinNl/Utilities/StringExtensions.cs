using System;

namespace WpWinNl.Utilities
{
  public static class StringExtensions
  {
    /// <summary>
    /// method to strip HTML tags from a string
    /// </summary>
    /// <param name="str">the string to strip</param>
    /// <param name="decodeFirst">if set to <c>true</c> [decode first].</param>
    /// <returns></returns>
    public static string StripHtml(this string str, bool decodeFirst = true)
    {
      try
      {

        while (((str.IndexOf("<") > -1) && (str.IndexOf(">") > -1) && (str.IndexOf("<") < str.IndexOf(">"))))
        {
          var start = str.IndexOf("<");
          var end = str.IndexOf(">");
          var count = end - start + 1;

          str = str.Remove(start, count);
        }

        str = str.Replace(" ", " ");
        str = str.Replace(">", "");
        str = str.Replace("\r\n", "");

        return str.Trim();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }
}
