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
      EventToCommandMappers = new ObservableCollection<EventToCommandMapper>();
    }
    protected override void OnAttached()
    {
      base.OnAttached();
      AddEventMappings();
    }

    /// <summary>
    /// Creates a new shape
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    private MapElement CreateShape(object viewModel)
    {
      var path = GetPathValue(viewModel);
      if (path != null && path.Positions.Any())
      {
        var newShape = CreateDrawable(viewModel, path);
        newShape.AddData(viewModel);
        newShape.SetLayer(LayerName);

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

    protected virtual MapElement CreateDrawable(object viewModel, Geopath path)
    {
      var newShape = ShapeDrawer.CreateShape(viewModel, path);
      return newShape;
    }

    private void AddEventMappings()
    {
      foreach (var mapper in EventToCommandMappers)
      {
        var evt = AssociatedObject.GetType().GetRuntimeEvent(mapper.EventName);
        if (evt != null)
        {
          AddEventMapping(mapper);
        }
      }
    }

    private void AddEventMapping(EventToCommandMapper mapper)
    {
      Observable.FromEventPattern<object>(AssociatedObject, mapper.EventName)
        .Subscribe(se =>
        {
          Point? tapPoint = null;
          var mapInputEventArgs = se.EventArgs as MapInputEventArgs;
          if (mapInputEventArgs != null)
          {
            tapPoint = mapInputEventArgs.Position;
          }
          if (tapPoint == null)
          {
            var tappedRoutedEventArgs = se.EventArgs as TappedRoutedEventArgs;
            if (tappedRoutedEventArgs != null)
            {
              tapPoint = tappedRoutedEventArgs.GetPosition(this.AssociatedObject);
            }
          }

          if (tapPoint != null)
          {
            Debug.WriteLine("Tapped on {0},{1} on LayerName {2}", tapPoint.Value.X, tapPoint.Value.Y, LayerName);

            var shapes = AssociatedObject.FindMapElementsAtOffset(tapPoint.Value);
            var selParams = new MapSelectionParameters
            {
              LayerName = LayerName,
              SelectTime = DateTimeOffset.Now
            };
            var t = shapes.Count(p => p.GetLayerName() == LayerName);
            if (t != 0)
            {
              Debug.WriteLine("Found {0} shapes on layer {1}", t, LayerName);
            }

            foreach (var shape in shapes.Where(p => p.GetLayerName() == LayerName))
            {
              FireViewmodelCommand(shape.ReadData<object>(), mapper.CommandName, selParams);
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
            AssociatedObject.MapElements[shapeLocation] = CreateShape(viewModel);
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
      var shape = AssociatedObject.MapElements.FirstOrDefault(p => p.ReadData() == viewModel);
      if (shape != null)
      {
        AssociatedObject.MapElements.Remove(shape);
      }
    }

    private Geopath GetPathValue(object viewModel)
    {
      if (viewModel != null)
      {
        var dcType = viewModel.GetType();

        var methodInfo = dcType.GetRuntimeMethod("get_" + PathPropertyName, new Type[0]);
          return methodInfo?.Invoke(viewModel, null) as Geopath;
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

    #region EventToCommandMappers

    /// <summary>
    /// EventToCommandMappers Property name
    /// </summary>
    public const string EventToCommandMappersPropertyName = "EventToCommandMappers";

    public ObservableCollection<EventToCommandMapper> EventToCommandMappers
    {
      get { return (ObservableCollection<EventToCommandMapper>)GetValue(EventToCommandMappersProperty); }
      set { SetValue(EventToCommandMappersProperty, value); }
    }

    /// <summary>
    /// EventToCommandMappers Property definition
    /// </summary>
    public static readonly DependencyProperty EventToCommandMappersProperty = DependencyProperty.Register(
        EventToCommandMappersPropertyName,
        typeof(ObservableCollection<EventToCommandMapper>),
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
                                 var toDelete = thisobj.AssociatedObject.MapElements.Where(p => p.GetLayerName() == thisobj.LayerName).ToList();
                                 toDelete.ForEach(p => thisobj.AssociatedObject.MapElements.Remove(p));

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
  }
}
