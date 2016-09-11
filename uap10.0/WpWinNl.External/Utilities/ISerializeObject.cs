namespace WpWinNl.Utilities
{
  public interface ISerializeObject
  {
    object[] Serialize(object target);
    object Deserialize(object[] data);
  }
}