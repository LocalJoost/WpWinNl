using System;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Always use an event to create instances of this type
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class CreateUsingEvent : Attribute
  {
  }
}