using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TestingApp.Views
{
    public partial class LeaderBoard : PhoneApplicationPage
    {
        public LeaderBoard()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
            //TO SORT by STARS
          lbx_LeaderBoardShow.ItemsSource=  App._userData;
        }

    }
}