using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSilverlightSerializer
{
  public class ReflectionTestSubject
  {

    public ReflectionTestSubject()
    {
      MyList = new List<string>();
    }

    public ReflectionTestSubject(string s) :this()
    {
      
    }
    public List<string> MyList { get; set; }

    public string MyString { get; set; }
  }
}
