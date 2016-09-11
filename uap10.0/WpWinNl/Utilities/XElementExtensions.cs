using System.Xml.Linq;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Extension method for an XElement to easily retrieve attribute values
  /// </summary>
  public static class XElementExtensions
  {
    public static string AttributeValue( this XElement e, string attrName)
    {
      var attr = e.Attribute(attrName);
      return attr != null ? attr.Value : string.Empty;
    }
  }
}
