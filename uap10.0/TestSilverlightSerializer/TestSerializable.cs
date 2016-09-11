using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpWinNl.Utilities;

namespace TestSilverlightSerializer
{
  public class TestSerializable
  {
    public string ToSerialize { get; set; }

    [DoNotSerialize]
    public string ToSkip { get; set; }
  }
}
