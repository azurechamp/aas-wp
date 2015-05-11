using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TestingApp
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.MarketplaceReviewTask task = new Microsoft.Phone.Tasks.MarketplaceReviewTask();
            task.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.EmailComposeTask email = new Microsoft.Phone.Tasks.EmailComposeTask();
            email.To = "saad-mehmood@outlook.com";
            email.Subject = "Contact | Gymnasio";
            email.Show();
        }
    }
}