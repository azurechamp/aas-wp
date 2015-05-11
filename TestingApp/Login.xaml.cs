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
using Coding4Fun.Toolkit.Controls;

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
                MessageBox.Show("Gymnasio and Internet are unable to connect","Connection", MessageBoxButton.OK);
            }
            else
            {
                btn_signIn.IsEnabled = true;
                App._userData = items;
            }

        }

        bool _userExist;
        private void btn_signIn_Click(object sender, RoutedEventArgs e)
        {
            foreach (User usr in items)
            {
                if (usr.UserName.Equals(tbx_UserName.Text.Trim()) && usr.Password.Equals(tbx_Password.Password.Trim()))
                {
                    _userExist = true;
                    App._AppUser = usr;
                }
            }

            if (_userExist == true)
            {

                NavigationService.Navigate(new Uri("/CharacterDance.xaml", UriKind.Relative));
                _userExist = false;
            }
            else 
            {
                MessageCustom("Whoops!!", "User with this username and password \ndoes not exists");
                tbx_Password.Password = "";
                tbx_UserName.Text = "";
      
            }
        }

        public void MessageCustom(string title, string details)
        {
            MessagePrompt msg = new MessagePrompt();
            msg.Title = title;
            msg.Body = details;
            msg.Show();
        }
    }
}