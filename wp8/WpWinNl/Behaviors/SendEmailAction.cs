using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Tasks;

namespace WpWinNl.Behaviors
{
    /// <summary>
    /// Action that executes the EmailComposeTask. 
    /// The "To", "Subject" and "Body" properties can be bound to.
    /// </summary>
    public class SendEmailAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To", typeof (string), typeof (SendEmailAction), new PropertyMetadata(null));

        public static readonly DependencyProperty SubjectProperty = DependencyProperty.Register(
            "Subject", typeof (string), typeof (SendEmailAction), new PropertyMetadata(null));

        public static readonly DependencyProperty BodyProperty = DependencyProperty.Register(
            "Body", typeof (string), typeof (SendEmailAction), new PropertyMetadata(null));

        public string To
        {
            get { return (string) GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public string Subject
        {
            get { return (string) GetValue(SubjectProperty); }
            set { SetValue(SubjectProperty, value); }
        }

        public string Body
        {
            get { return (string) GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            var email = new EmailComposeTask {To = To, Subject = Subject, Body = Body};
            try
            {
                email.Show();
            }
            catch (InvalidOperationException e)
            {
                
            }
        }
    }
}