using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace TestSilverlightSerializer
{
  public class TestDepObject : DependencyObject
  {
    #region MyProperty

    /// <summary>
    /// MyProperty Property name
    /// </summary>
    public const string MyPropertyPropertyName = "MyProperty";

    public string MyProperty
    {
      get { return (string)GetValue(MyPropertyProperty); }
      set { SetValue(MyPropertyProperty, value); }
    }

    /// <summary>
    /// MyProperty Property definition
    /// </summary>
    public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(
        MyPropertyPropertyName,
        typeof(string),
        typeof(TestDepObject),
        new PropertyMetadata(default(string), MyPropertyChanged));

    /// <summary>
    /// MyProperty property changed callback.
    /// </summary>
    /// <param name="d">The depency object (i.e. the behavior).</param>
    /// <param name="e">The property event args <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
    public static void MyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var thisobj = d as TestDepObject;
      var newValue = (string)e.NewValue;
      if (thisobj != null)
      {
      }
    }

    #endregion
  }
}
