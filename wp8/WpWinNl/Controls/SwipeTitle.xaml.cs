using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpWinNl.Controls
{
    /// <summary>
    /// An alternative to the pivot control
    /// </summary>
    public partial class SwipeTitle 
    {
        /// <summary>
        /// An internal linked list to make searching easier
        /// </summary>
        private LinkedList<object> _displayItems;
        private TranslateTransform _transform;
        private int _currentDisplayItem = 1;
        private bool _animationEnabled = true;

        public SwipeTitle()
        {
            InitializeComponent();
            _transform = new TranslateTransform();

            pnlSwipe.Loaded += (sender, args) => ProcessSelectedItemChanged();
            pnlSwipe.ManipulationDelta += pnlSwipe_ManipulationDelta;
            pnlSwipe.ManipulationCompleted += pnlSwipe_ManipulationCompleted;
            pnlSwipe.RenderTransform = _transform;
            tbPrevious.SizeChanged += tbPrevious_SizeChanged;
            tbNext.MouseLeftButtonUp += tbNext_MouseLeftButtonUp;
        }

        public void tbNext_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_animationEnabled) return;
            ScrollToDisplayItem(2, true);
        }

        void pnlSwipe_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (!_animationEnabled) return;
            _transform.X += e.DeltaManipulation.Translation.X;
        }

        /// <summary>
        /// Caculates the screen width
        /// </summary>
        private static double ScreenWidth
        {
            get
            {
                var appFrame = Application.Current.RootVisual as Frame;
                return null == appFrame ? 0.0 : appFrame.RenderSize.Width;
            }
        }
        /// <summary>
        /// Fired after manipulation is completed. When the title has been moved over 25% of the
        /// screen, the next or previous item is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pnlSwipe_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (!_animationEnabled) return;
            if (e.TotalManipulation.Translation.X > .25 * ScreenWidth)
            {
                ScrollToDisplayItem(_currentDisplayItem - 1, true);
            }
            else if (e.TotalManipulation.Translation.X < -.25 * ScreenWidth)
            {
                ScrollToDisplayItem(_currentDisplayItem + 1, true);
            }
            ScrollToDisplayItem(_currentDisplayItem, true);
        }

        /// <summary>
        /// Fired when new data is put into the last object. 
        /// Rendering is then finished - the middle text is showed in the middle again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tbPrevious_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollToDisplayItem(1, false);
        }

        /// <summary>
        /// Shows (possible) new texts in the three text boxes
        /// </summary>
        private void UpdateDisplayTexts()
        {
            if (Items != null)
            {
                tbNext.Visibility = Items.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
                _animationEnabled = Items.Count > 1;
            }
            if (_displayItems == null) return;
            if (SelectedItem == null)
            {
                SelectedItem = _displayItems.First.Value;
            }
            tbCurrent.Text = GetDisplayValue(SelectedItem);
            var currentNode = _displayItems.Find(SelectedItem);
            if (currentNode == null) return;
            tbNext.Text =
                GetDisplayValue(currentNode.Next != null ? 
                  currentNode.Next.Value : _displayItems.First.Value);
            tbPrevious.Text =
                GetDisplayValue(currentNode.Previous != null ? 
                  currentNode.Previous.Value : _displayItems.Last.Value);

        }



        /// <summary>
        /// Scrolls to one of the 3 display items
        /// </summary>
        /// <param name="item">0,1 or 2</param>
        /// <param name="animate">Animate the transition</param>
        void ScrollToDisplayItem(int item, bool animate)
        {
            _currentDisplayItem = item;
            if (_currentDisplayItem < 0) _currentDisplayItem = 0;
            if (_currentDisplayItem >= pnlSwipe.Children.Count)
            {
                _currentDisplayItem = pnlSwipe.Children.Count - 1;
            }
            var totalTransform = 0.0;
            for (var counter = 0; counter < _currentDisplayItem; counter++)
            {
                double leftMargin = 0;
                if (counter + 1 < pnlSwipe.Children.Count)
                {
                    leftMargin = 
                        ((Thickness)(pnlSwipe.Children[counter + 1].GetValue(MarginProperty))).Left;
                }
                totalTransform += 
                    pnlSwipe.Children[counter].RenderSize.Width + leftMargin;
            }
            var whereDoWeGo = -1 * totalTransform;

            if (animate)
            {
                //Set up the storyboard and animate the transition
                var sb = new Storyboard();
                var anim = 
                    new DoubleAnimation
                    {
                        From = _transform.X,
                        To = whereDoWeGo,
                        Duration = new Duration(
                            TimeSpan.FromMilliseconds(Math.Abs(_transform.X - whereDoWeGo)))
                    };
                Storyboard.SetTarget(anim, _transform);
                Storyboard.SetTargetProperty(anim, new PropertyPath(TranslateTransform.XProperty));
                sb.Children.Add(anim);
                sb.Completed += sb_Completed;
                sb.Begin();
            }
            else
            {
                _transform.X = whereDoWeGo;
            }
        }

        /// <summary>
        /// Fired when an animation is completed. Then a new items must be selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sb_Completed(object sender, EventArgs e)
        {
            if (SelectedItem != null)
            {
                var currentNode = _displayItems.Find(SelectedItem);
                if (_currentDisplayItem == 0)
                {
                    SelectedItem = (currentNode.Previous != null ? currentNode.Previous.Value : _displayItems.Last.Value);
                }
                if (_currentDisplayItem == 2)
                {
                    SelectedItem = (currentNode.Next != null ? currentNode.Next.Value : _displayItems.First.Value);
                }
                UpdateDisplayTexts();
            }
        }

        /// <summary>
        /// Retrieve the display value of an object
        /// </summary>
        /// <param name="displayObject"></param>
        /// <returns></returns>
        private string GetDisplayValue( object displayObject)
        {
            if (DisplayField != null)
            {
                var pinfo = displayObject.GetType().GetProperty(DisplayField);
                if (pinfo != null)
                {
                    return pinfo.GetValue(displayObject, null).ToString();
                }
            }

            return displayObject.ToString();
        }

        #region Items
        /// <summary>
        /// Dependency property holding the items selectable in this control
        /// </summary>
        public static readonly DependencyProperty ItemsProperty =
             DependencyProperty.Register("Items", typeof(IList),
             typeof(SwipeTitle), new PropertyMetadata(ItemsChanged));


        private static void ItemsChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var c = sender as SwipeTitle;
            if (c != null)
            {
                //c.UpdateDisplayTexts();
                c.ProcessItemsChanged();
            }
        }

        public IList Items
        {
            get { return (IList)GetValue(ItemsProperty); }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        public void ProcessItemsChanged()
        {
            _displayItems = new LinkedList<object>();
            if( Items == null || Items.Count == 0) return;
            foreach (var obj in Items) _displayItems.AddLast(obj);
            if (!Items.Contains( SelectedItem)) SelectedItem = Items[0];


        }
        #endregion

        #region SelectedItem
        /// <summary>
        /// Dependency property holding the currently selected object
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
             DependencyProperty.Register("SelectedItem", typeof(object),
             typeof(SwipeTitle), new PropertyMetadata(SelectedItemChanged));

        private static void SelectedItemChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var c = sender as SwipeTitle;
            if (c != null)
            {
                c.ProcessSelectedItemChanged();
                if (c.SelectedValueChanged != null)
                {
                    c.SelectedValueChanged(c,
                        new RoutedPropertyChangedEventArgs<object>(args.OldValue, args.NewValue));
                }
             }
        }

        // .NET Property wrapper
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public void ProcessSelectedItemChanged()
        {
            UpdateDisplayTexts();
            ScrollToDisplayItem(1, false);
        }

        public event RoutedPropertyChangedEventHandler<Object> SelectedValueChanged;
        #endregion
        
        #region DisplayField
        public string DisplayField
        {
            get { return (string)GetValue(DisplayFieldProperty); }
            set { SetValue(DisplayFieldProperty, value); }
        }

 
        public static readonly DependencyProperty DisplayFieldProperty =
            DependencyProperty.Register("DisplayField", typeof(string), typeof(SwipeTitle), null);

        #endregion
    }
}
