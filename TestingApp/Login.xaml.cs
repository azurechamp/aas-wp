using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using TestingApp.DataModels;

namespace TestingApp
{
    public partial class Login : PhoneApplicationPage
    {
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> todoTable = App.MobileService.GetTable<User>();
     

        public Login()
        {
            InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await RefreshTodoItems();
        }

        private async Task RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems
                items = await todoTable
                    .Where(todoItem => todoItem.isDeleted == false)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                MessageBox.Show("Error Loading Items!");
            }
            else
            {
                btn_signIn.IsEnabled = true;
            }

        }

        bool _userExist;
        private void btn_signIn_Click(object sender, RoutedEventArgs e)
        {
            foreach (User usr in items)
            {
                if (usr.UserName.Equals(tbx_UserName.Text.Trim()) && usr.Password.Equals(tbx_Password.Text.Trim()))
                {
                    _userExist = true;
                    App._AppUser = usr;
                }
            }

            if (_userExist == true)
            {
                MessageBox.Show("Login Successful");
                MessageBox.Show(App._AppUser.Id);
                NavigationService.Navigate(new Uri("/Views/MainHub.xaml", UriKind.Relative));
                _userExist = false;
            }
        }
    }
}