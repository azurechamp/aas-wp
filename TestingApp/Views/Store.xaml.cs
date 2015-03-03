using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TestingApp.ViewModels;

namespace TestingApp.Views
{
    public partial class Store : PhoneApplicationPage
    {
        public Store()
        {
            InitializeComponent();
            this.DataContext = new StorePageView_Model();
            
        }
    }
}