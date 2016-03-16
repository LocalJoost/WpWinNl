using System.Collections.Generic;

namespace TestSilverlightSerializer
{
  public class TestComplexSerializable
  {
    public TestComplexSerializable()
    {
      GenericPropery = new List<GenericThingy<int>>();
    }
    public string String1 { get; set; }
    public string String2 { get; set; }

    public List<GenericThingy<int>> GenericPropery { get; set; }
  }

  public class GenericThingy<T>
  {
    public T Whatever { get; set; }
    public double SomethingElse { get; set; }
  }
}
