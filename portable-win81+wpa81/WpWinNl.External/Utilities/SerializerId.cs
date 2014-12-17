using System;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Attribute used to flag IDs this can be useful for check object
  /// consistence when the serializer is in a mode that does not 
  /// serialize identifiers
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public class SerializerId : Attribute
  {
  }
}