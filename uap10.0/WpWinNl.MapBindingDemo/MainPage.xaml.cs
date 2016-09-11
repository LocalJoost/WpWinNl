﻿using Windows.UI.Xaml.Controls;
using WpWinNl.MapBindingDemo.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WpWinNl.MapBindingDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MapBindingViewModel();
        }

        public MapBindingViewModel ViewModel { get; set; }
    }
}
