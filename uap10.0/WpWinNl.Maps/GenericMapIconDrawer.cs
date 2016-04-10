using Windows.Devices.Geolocation;
using Windows.Foundation;
using System;
using System.Reflection;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;

namespace WpWinNl.Maps
{
    public class GenericMapIconDrawer : MapShapeDrawer
    {
        protected object ViewModel;

        protected MapIcon Icon;


        public GenericMapIconDrawer()
        {
        }

        public string TitlePropertyName { get; set; }

        public string AnchorPropertyName { get; set; }

        public string ImageUriPropertyName { get; set; }

        public string IsVisiblePropertyName { get; set; }

        public MapElementCollisionBehavior CollisionBehaviorDesired { get; set; }

        public override MapElement CreateShape(object viewModel, BasicGeoposition pos)
        {
            ViewModel = viewModel;

            Icon = new MapIcon
            {
                Location = new Geopoint(pos),
                CollisionBehaviorDesired = CollisionBehaviorDesired,
                ZIndex = ZIndex
            };

            SetPropertyValuesFromViewModel();

            return Icon;
        }

        private void SetPropertyValuesFromViewModel(string propName = null)
        {

            string title = null;
            if (TryGetPropertyValue(ViewModel, TitlePropertyName, ref title))
            {
                Icon.Title = title;
            }

            Point anchorPoint;
            if (TryGetPropertyValue(ViewModel, AnchorPropertyName, ref anchorPoint))
            {
                Icon.NormalizedAnchorPoint = anchorPoint;
            }

            string imageUri = null;
            if (TryGetPropertyValue(ViewModel, ImageUriPropertyName, ref imageUri))
            {
                Icon.Image = RandomAccessStreamReference.CreateFromUri(new Uri(imageUri));
            }

            bool isVisble = true;
            if (TryGetPropertyValue(ViewModel, IsVisiblePropertyName, ref isVisble))
            {
                Icon.Visible = isVisble;
            }
        }

        private static bool TryGetPropertyValue<T>(object obj, string propertyName, ref T outValue)
        {
            var prop = obj.GetType().GetRuntimeProperty(propertyName);
            var result = prop?.GetValue(obj);
            if (result is T)
            {
                outValue = (T)prop.GetValue(obj);
                return true;
            }
            return false;
        }
    }
}
