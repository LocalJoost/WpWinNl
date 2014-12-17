using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Tasks;

namespace WpWinNl.Behaviors
{
    /// <summary>
    /// Action that executes the MarketplaceReviewTask. 
    /// </summary>
    public class NavigatieToReviewAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();
            try
            {
                marketplaceReviewTask.Show();
            }
            catch (InvalidOperationException ex)
            {
                
            }
        }
    }
}