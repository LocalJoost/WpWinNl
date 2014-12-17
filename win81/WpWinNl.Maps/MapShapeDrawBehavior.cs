using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Bing.Maps;
using Windows.UI.Xaml;
using System.Reactive.Linq;
using Windows.UI.Xaml.Input;
using WpWinNl.Behaviors;

namespace WpWinNl.Maps
{
  public class MapShapeDrawBehavior : Behavior<Map>
  {
    private MapShapeLayer _mapLayer;

    protected override void OnAttached()
    {
      AssociatedObject.Loaded += AssociatedObjectLoaded;
      base.OnAttached();
    }

    void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      _mapLayer = new MapShapeLayer();
      AssociatedObject.ShapeLayers.Add(_mapLayer);
    }

    /// <summary>
    /// Creates a new shape
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    private MapShape CreateShape(object viewModel)
    {
      var path = GetPathValue(viewModel);
      if (path != null && path.Any())
      {
        var newShape = CreateDrawable(viewModel, path);
        newShape.Tapped += ShapeTapped;

        MapElementProperties.SetViewModel(newShape, viewModel);
        MapElementProperties.SetLayerName(newShape, LayerName);

        // Listen to property changed event of geometry property to check if the shape needs to
        // be redrawed
        var evt = viewModel.GetType().GetRuntimeEvent("PropertyChanged");
        if (evt != null)
        {
          Observable.FromEventPattern<PropertyChangedEventArgs>(viewModel, "PropertyChanged")
            .Subscribe(se =>
                         {
                           if (se.EventArgs.PropertyName == PathPropertyName)
                           {
                             ReplaceShape(se.Sender);
                           }
                         });
        }
        return newShape;
      }
      return null;
    }

    protected virtual MapShape CreateDrawable(object viewModel, LocationCollection path )
    {
      var newShape = ShapeDrawer.CreateShape(viewModel, path);
      return newShape;
    }

    private void ShapeTapped(object sender, TappedRoutedEventArgs e)
    {
      var shape = sender as MapShape;
      if( shape != null )
      {
        var viewModel = MapElementProperties.GetViewModel(shape);
        if( viewModel != null )
        {
          FireViewmodelCommand(viewModel, TapCommand);
        }
      }
    }

    private void FireViewmodelCommand(object viewModel, string commandName)
    {
      if (viewModel != null && !string.IsNullOrWhiteSpace(commandName))
      {
        var dcType = viewModel.GetType();
        var commandGetter = dcType.GetRuntimeMethod("get_" + commandName, new Type[0]);
        if (commandGetter != null)
        {
          var command = commandGetter.Invoke(viewModel, null) as ICommand;
          if (command != null)
          {
            command.Execute(viewModel);
          }
        }
      }
    }

    private void AddNewShapes(IEnumerable viewModels)
    {
      foreach (var item in viewModels)
      {
        AddNewShape(item);
      }
    }
    
    private void AddNewShape(object viewModel)
    {
      var shape = CreateShape(viewModel);
      if (shape != null)
      {
        _mapLayer.Shapes.Add(shape);
      }
    }
    
    private void ReplaceShape(object viewModel)
    {
      var shape = _mapLayer.Shapes.FirstOrDefault(p => MapElementProperties.GetViewModel(p) == viewModel);
      if (shape != null)
      {
        var shapeLocation = _mapLayer.Shapes.IndexOf(shape);
        if (shapeLocation != -1)
        {
          var newShape = CreateShape(viewModel);
          if (newShape != null)
          {
            _mapLayer.Shapes[shapeLocation].Tapped -= ShapeTapped;
            _mapLayer.Shapes[shapeLocation] = CreateShape(viewModel);
          }
        }
      }
      else
      {
        AddNewShape(viewModel);
      }
    }

    private void RemoveShapes(IEnumerable viewModels)
    {
      foreach (var item in viewModels)
      {
        RemoveShape(item);
      }
    }

    private void RemoveShape(object viewModel)
    {
      var shape = _mapLayer.Shapes.FirstOrDefault(p => MapElementProperties.GetViewModel(p) == viewModel);
      if (shape != null)
      {
        _mapLayer.Shapes.Remove(shape);
        shape.Tapped -= ShapeTapped;
      }
    }

    private LocationCollection GetPathValue(object viewModel)
    {
      if (viewModel != null)
      {
        var dcType = viewModel.GetType();

        var methodInfo = dcType.GetRuntimeMethod("get_" + PathPropertyName, new Type[0]);
        if (methodInfo != null)
        {
          return methodInfo.Invoke(viewModel, null) as LocationCollection;
        }
      }
      return null;
    }

    #region PathPropertyName

    /// <summary>
    /// PathPropertyName Property name
    /// </summary>
    public const string PathPropertyNamePropertyName = "PathPropertyName";

    public string PathPropertyName
    {
      get { return (string)GetValue(PathPropertyNameProperty); }
      set { SetValue(PathPropertyNameProperty, value); }
    }

    /// <summary>
    /// PathPropertyName Property definition
    /// </summary>
    public static readonly DependencyProperty PathPropertyNameProperty = DependencyProperty.Register(
      PathPropertyNamePropertyName,
      typeof(string),
      typeof(MapShapeDrawBehavior),
      new PropertyMetadata(default(string)));

    #endregion

    #region ItemsSource

    /// <summary>
    /// ItemsSource Property name
    /// </summary>
    public const string ItemsSourcePropertyName = "ItemsSource";

    public IEnumerable ItemsSource
    {
      get { return (IEnumerable)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// ItemsSource Property definition
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        ItemsSourcePropertyName,
        typeof(IEnumerable),
        typeof(MapShapeDrawBehavior),
        new PropertyMetadata(default(IEnumerable), ItemsSourceChanged));

    /// <summary>
    /// ItemsSource property changed callback.
    /// </summary>
    /// <param name="d">The depency object (i.e. the behavior).</param>
    /// <param name="e">The property event args <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
    public static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var thisobj = d as MapShapeDrawBehavior;
      var newValue = (IEnumerable)e.NewValue;
      if (thisobj != null)
      {
        var evt = newValue.GetType().GetRuntimeEvent("CollectionChanged");
        if (evt != null)
        {
          Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(newValue, "CollectionChanged")
            .Subscribe(se =>
                         {
                           switch (se.EventArgs.Action)
                           {
                             case NotifyCollectionChangedAction.Add:
                               {
                                 thisobj.AddNewShapes(se.EventArgs.NewItems);
                                 break;
                               }

                             case NotifyCollectionChangedAction.Remove:
                               {
                                 thisobj.RemoveShapes(se.EventArgs.OldItems);
                                 break;
                               }

                             case NotifyCollectionChangedAction.Replace:
                               {
                                 thisobj.RemoveShapes(se.EventArgs.OldItems);
                                 thisobj.AddNewShapes(se.EventArgs.NewItems);
                                 break;
                               }

                             case NotifyCollectionChangedAction.Reset:
                               {
                                 thisobj._mapLayer.Shapes.Clear();
                                 thisobj.AddNewShapes(thisobj.ItemsSource);
                                 break;
                               }
                           }
                         });
        }
        if( newValue != null )
        {
          thisobj.AddNewShapes(newValue);
        }
      }
    }

    #endregion

    #region LayerName

    /// <summary>
    /// LayerName Property name
    /// </summary>
    public const string LayerNamePropertyName = "LayerName";

    public string LayerName
    {
      get { return (string)GetValue(LayerNameProperty); }
      set { SetValue(LayerNameProperty, value); }
    }

    /// <summary>
    /// LayerName Property definition
    /// </summary>
    public static readonly DependencyProperty LayerNameProperty = DependencyProperty.Register(
        LayerNamePropertyName,
        typeof(string),
        typeof(MapShapeDrawBehavior),
        new PropertyMetadata(default(string)));

    #endregion

    #region TapCommand

    /// <summary>
    /// TapCommand Property name
    /// </summary>
    public const string TapCommandPropertyName = "TapCommand";

    public string TapCommand
    {
      get { return (string)GetValue(TapCommandProperty); }
      set { SetValue(TapCommandProperty, value); }
    }

    /// <summary>
    /// TapCommand Property definition
    /// </summary>
    public static readonly DependencyProperty TapCommandProperty = DependencyProperty.Register(
        TapCommandPropertyName,
        typeof(string),
        typeof(MapShapeDrawBehavior),
        new PropertyMetadata(default(string)));

    #endregion

    #region ShapeDrawer

    /// <summary>
    /// ShapeDrawer Property name
    /// </summary>
    public const string ShapeDrawerPropertyName = "ShapeDrawer";

    public MapShapeDrawer ShapeDrawer
    {
      get { return (MapShapeDrawer)GetValue(ShapeDrawerProperty); }
      set { SetValue(ShapeDrawerProperty, value); }
    }

    /// <summary>
    /// ShapeDrawer Property definition
    /// </summary>
    public static readonly DependencyProperty ShapeDrawerProperty = DependencyProperty.Register(
        ShapeDrawerPropertyName,
        typeof(MapShapeDrawer),
        typeof(MapShapeDrawBehavior),
        new PropertyMetadata(new MapPolylineDrawer()));

    #endregion
  }
}
