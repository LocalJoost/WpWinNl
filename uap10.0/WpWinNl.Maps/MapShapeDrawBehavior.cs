using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;


namespace WpWinNl.Maps
{
  public class MapShapeDrawBehavior : Behavior<MapControl>
  {
    public MapShapeDrawBehavior()
    {
      EventToHandlerMappers = new ObservableCollection<EventToHandlerMapper>();
    }
    protected override void OnAttached()
    {
      base.OnAttached();
      AddEventMappings();
    }

    protected override void OnDetaching()
    {
      _collectionObserver?.Dispose();
      RemoveShapes(AssociatedObject.MapElements.Select(p=> p.ReadData()).ToList());
      base.OnDetaching();
    }

    /// <summary>
    /// Creates a new shape
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    private MapElement CreateShape(object viewModel)
    {
      var locationData = GetPathValue(viewModel);
      if (locationData != null)
      {
        var newShape = CreateDrawable(viewModel, locationData);
        if (newShape != null)
        {
          newShape.AddData(viewModel);
          newShape.SetLayer(LayerName);

          // Listen to property changed event of geometry property to check if the shape needs to
          // be redrawed.
          var evt = viewModel.GetType().GetRuntimeEvent("PropertyChanged");
          if (evt != null)
          {
            var observable = Observable.FromEventPattern<PropertyChangedEventArgs>(viewModel, "PropertyChanged")
              .Subscribe(se =>
              {
                if (!LegacyMode || se.EventArgs.PropertyName == PathPropertyName)
                {
                  ReplaceShape(se.Sender);
                }
              });

            TrackObservable(viewModel, observable);
          }
          return newShape;
        }
      }
      return null;
    }

    protected virtual MapElement CreateDrawable(object viewModel, object locationData)
    {
      if (locationData is BasicGeoposition)
      {
        return ShapeDrawer.CreateShape(viewModel, (BasicGeoposition)locationData);
      }

      var p = locationData as Geopath;
      if (p != null && p.Positions.Any())
      {
        return ShapeDrawer.CreateShape(viewModel, p);
      }

      var l = locationData as IList<Geopath>;
      if (l != null && l.Any() && l.All(path => path.Positions.Any()))
      {
        return ShapeDrawer.CreateShape(viewModel, l);
      }

      return null;
    }

    private void AddEventMappings()
    {
      foreach (var mapper in EventToHandlerMappers)
      {
        var evt = AssociatedObject.GetType().GetRuntimeEvent(mapper.EventName);
        if (evt != null)
        {
          AddEventMapping(mapper);
        }
      }
    }

    private Point? GetTapPoint(object args)
    {
      Point? tapPoint = null;
      var mapInputEventArgs = args as MapInputEventArgs;
      if (mapInputEventArgs != null)
      {
        tapPoint = mapInputEventArgs.Position;
      }
      if (tapPoint == null)
      {
        var tappedRoutedEventArgs = args as TappedRoutedEventArgs;
        if (tappedRoutedEventArgs != null)
        {
          tapPoint = tappedRoutedEventArgs.GetPosition(this.AssociatedObject);
        }
      }
      return tapPoint;
    }

    private void AddEventMapping(EventToHandlerMapper mapper)
    {
      Observable.FromEventPattern<object>(AssociatedObject, mapper.EventName)
        .Subscribe(se =>
        {
          var tapPoint = GetTapPoint(se.EventArgs);
          IList<MapElement> shapes = null;
          if (tapPoint != null)
          {
            Debug.WriteLine("Tapped on {0},{1} on LayerName {2}", tapPoint.Value.X, tapPoint.Value.Y, LayerName);
            shapes = new List<MapElement>(AssociatedObject.FindMapElementsAtOffset(tapPoint.Value));
          }
          else
          {
            var mapClickArgs = se.EventArgs as MapElementClickEventArgs;
            if (mapClickArgs != null)
            {
              shapes = mapClickArgs.MapElements;
            }
          }

          if (shapes != null)
          {
            var shapesOnLayer = shapes.Where(p => p.GetLayerName() == LayerName).ToList();
            var selParams = new MapSelectionParameters
            {
              LayerName = LayerName,
              SelectTime = DateTimeOffset.Now
            };

            var t = shapesOnLayer.Count;
            if (t != 0)
            {
              Debug.WriteLine("Found {0} shapes on layer {1}", t, LayerName);
            }

            foreach (var shape in shapesOnLayer)
            {
              if (!string.IsNullOrWhiteSpace(mapper.MethodName))
              {
                FireViewmodelMethod(shape.ReadData<object>(), mapper.MethodName, selParams);
              }
              else if (!string.IsNullOrWhiteSpace(mapper.CommandName))
              {
                FireViewmodelCommand(shape.ReadData<object>(), mapper.CommandName, selParams);
              }
            }
          }
        });
    }

    private void FireViewmodelCommand(object viewModel, string commandName, MapSelectionParameters selParams)
    {
      if (viewModel != null && !string.IsNullOrWhiteSpace(commandName))
      {
        var dcType = viewModel.GetType();
        var commandGetter = dcType.GetRuntimeMethod("get_" + commandName, new Type[0]);
        var command = commandGetter?.Invoke(viewModel, null) as ICommand;
        command?.Execute(selParams);
      }
    }

    private void FireViewmodelMethod(object viewModel, string methodName, MapSelectionParameters selParams)
    {
      if (viewModel != null && !string.IsNullOrWhiteSpace(methodName))
      {
        var dcType = viewModel.GetType();
        var method = dcType.GetRuntimeMethod(methodName, new[] {typeof(MapSelectionParameters)});
        method?.Invoke(viewModel, new object[]{selParams});
      }
    }

    private void AddNewShapes(IEnumerable viewModels)
    {
      foreach (var item in viewModels)
      {
        AddNewShape(item);
      }
    }

    private readonly Dictionary<object,IDisposable> _observedObjects = new Dictionary<object, IDisposable>();

    private void TrackObservable(object viewModel, IDisposable observable)
    {
      if (!_observedObjects.ContainsKey(viewModel))
      {
        _observedObjects.Add(viewModel, observable);
      }
    }

    private void RemoveObservable(object viewModel)
    {
      if (_observedObjects.ContainsKey(viewModel))
      {
        _observedObjects[viewModel].Dispose();
        _observedObjects.Remove(viewModel);
      }
    }

    private void AddNewShape(object viewModel)
    {
      var shape = CreateShape(viewModel);
      if (shape != null)
      {
        AssociatedObject.MapElements.Add(shape);
      }
    }


    private void ReplaceShape(object viewModel)
    {
      var shape = AssociatedObject.MapElements.FirstOrDefault(p => p.ReadData() == viewModel);
      if (shape != null)
      {
        var shapeLocation = AssociatedObject.MapElements.IndexOf(shape);
        if (shapeLocation != -1)
        {
          var newShape = CreateShape(viewModel);
          if (newShape != null)
          {
            AssociatedObject.MapElements.RemoveAt(shapeLocation);
            AssociatedObject.MapElements.Insert(shapeLocation, newShape);
          }
        }
      }
      else
      {
        AddNewShape(viewModel);
      }
    }

    private void RemoveShapes(IList viewModels)
    {
      foreach (var item in viewModels)
      {
        RemoveShape(item);
      }
    }

    private void RemoveShape(object viewModel)
    {
      var shape = AssociatedObject.MapElements.FirstOrDefault(p => p.ReadData() == viewModel);
      if (shape != null)
      {
        AssociatedObject.MapElements.Remove(shape);
        RemoveObservable(viewModel);
      }
    }

    private object GetPathValue(object viewModel){
      if (viewModel != null)
      {
        var dcType = viewModel.GetType();

        var methodInfo = dcType.GetRuntimeMethod("get_" + PathPropertyName, new Type[0]);
        return methodInfo?.Invoke(viewModel, null);
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

    #region EventToHandlerMappers

    /// <summary>
    /// EventToHandlerMappers Property name
    /// </summary>
    public const string EventToCommandMappersPropertyName = "EventToHandlerMappers";

    public ObservableCollection<EventToHandlerMapper> EventToHandlerMappers
    {
      get { return (ObservableCollection<EventToHandlerMapper>)GetValue(EventToHandlerMappersProperty); }
      set { SetValue(EventToHandlerMappersProperty, value); }
    }

    /// <summary>
    /// EventToHandlerMappers Property definition
    /// </summary>
    public static readonly DependencyProperty EventToHandlerMappersProperty = DependencyProperty.Register(
        EventToCommandMappersPropertyName,
        typeof(ObservableCollection<EventToHandlerMapper>),
        typeof(MapShapeDrawBehavior),
        new PropertyMetadata(null));

    #endregion

    #region ItemsSource

    /// <summary>
    /// ItemsSource Property name
    /// </summary>
    public const string ItemsSourcePropertyName = "ItemsSource";

    public IEnumerable<object> ItemsSource
    {
      get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// ItemsSource Property definition
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        ItemsSourcePropertyName,
        typeof(IEnumerable<object>),
        typeof(MapShapeDrawBehavior),
        new PropertyMetadata(default(IEnumerable<object>), ItemsSourceChanged));

    /// <summary>
    /// ItemsSource property changed callback.
    /// </summary>
    /// <param name="d">The depency object (i.e. the behavior).</param>
    /// <param name="e">The property event args <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
    public static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var thisobj = d as MapShapeDrawBehavior;
      var newValue = (IEnumerable<object>)e.NewValue;
      if (thisobj != null)
      {
        var evt = newValue.GetType().GetRuntimeEvent("CollectionChanged");
        if (evt != null)
        {
          thisobj._collectionObserver?.Dispose();
          thisobj._collectionObserver = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(newValue, "CollectionChanged")
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
                                 var toDelete = thisobj.AssociatedObject.MapElements.Where(p => p.GetLayerName() == thisobj.LayerName).Select(p=> p.ReadData()).ToList();
                                 thisobj.RemoveShapes(toDelete);
                                 thisobj.AddNewShapes(thisobj.ItemsSource);
                                 break;
                               }
                           }
                         });
        }
        // Add initial shapes
        thisobj.AddNewShapes(thisobj.ItemsSource);
      }
    }

    private IDisposable _collectionObserver;

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


      
    #region LegacyMode

    /// <summary>
    /// LegacyMode Property name
    /// </summary>
    public const string LegacyModePropertyName = "LegacyMode";

    public bool LegacyMode
    {
      get { return (bool)GetValue(LegacyModeProperty); }
      set { SetValue(LegacyModeProperty, value); }
    }

    /// <summary>
    /// LegacyMode Property definition
    /// </summary>
    public static readonly DependencyProperty LegacyModeProperty = DependencyProperty.Register(
        LegacyModePropertyName,
        typeof(bool),
        typeof(MapShapeDrawBehavior),
        new PropertyMetadata(default(bool)));

    #endregion
  }
}
