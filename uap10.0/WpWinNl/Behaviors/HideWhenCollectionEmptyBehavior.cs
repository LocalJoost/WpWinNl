using System.Collections;
using System.Collections.Specialized;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace WpWinNl.Behaviors
{
  public class HideWhenCollectionEmptyBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      SetVisibility();
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      if (Collection != null)
      {
        Collection.CollectionChanged -= OnCollectionChanged;
      }
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      SetVisibility();
    }

    private void SetVisibility()
    {
      var collection = Collection as ICollection;
      AssociatedObject.Visibility = collection != null && collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    #region Collection

    public const string CollectionPropertyName = "Collection";

    public INotifyCollectionChanged Collection
    {
      get { return (INotifyCollectionChanged)GetValue(CollectionProperty); }
      set { SetValue(CollectionProperty, value); }
    }

    public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register(
        CollectionPropertyName,
        typeof(INotifyCollectionChanged),
        typeof(HideWhenCollectionEmptyBehavior),
        new PropertyMetadata(default(INotifyCollectionChanged), OnCollectionChanged));

    public static void OnCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var behavior = d as HideWhenCollectionEmptyBehavior;
      var newValue = (INotifyCollectionChanged)e.NewValue;
      var oldValue = (INotifyCollectionChanged)e.OldValue;
      if (behavior != null)
      {
        if (oldValue != null)
        {
          oldValue.CollectionChanged -= behavior.OnCollectionChanged;
        }
        if (newValue != null)
        {
          newValue.CollectionChanged += behavior.OnCollectionChanged;
        }

        behavior.SetVisibility();
      }
    }

    #endregion
  }
}
