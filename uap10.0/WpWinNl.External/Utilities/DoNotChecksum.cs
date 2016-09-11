using System;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Used in checksum mode to flag a property as not being part
  /// of the "meaning" of an object - i.e. two objects with the
  /// same checksum "mean" the same thing, even if some of the
  /// properties are different, those properties would not be
  /// relevant to the purpose of the object
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public class DoNotChecksum : Attribute
  {
  }
}