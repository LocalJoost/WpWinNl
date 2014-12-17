using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Tasks;

namespace WpWinNl.Behaviors
{
    /// <summary>
    /// Action that executes the MarketPlaceDetailTask. 
    /// </summary>
    public class NavigatieToMarketplaceDetailAction:TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            var marketplaceDetailTask = new MarketplaceDetailTask();
            try
            {
                marketplaceDetailTask.Show(); 
            }
            catch (InvalidOperationException ex)
            {
               
            }
                     
        }


    }
}