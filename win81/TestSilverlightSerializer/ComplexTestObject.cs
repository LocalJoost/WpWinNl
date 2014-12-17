using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestSilverlightSerializer
{
  public class ComplexTestObject
  {

    public ComplexTestObject()
    {
      Alist = new List<DateTimeOffset>();
    }

    public List<DateTimeOffset> Alist;

    public string Whatever { get; set; }

    public DateTimeOffset DateTime1 { get; set; }
    public DateTimeOffset DateTime2 { get; set; }

  }
}
