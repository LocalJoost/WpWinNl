using System;

namespace WpWinNl.Utilities
{
  /// <summary>
  ///   Indicates that a property or field should not be serialized
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
  public class DoNotSerialize : Attribute
  {

  }
}