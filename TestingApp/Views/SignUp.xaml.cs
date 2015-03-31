using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using TestingApp.DataModels;
using System.Threading.Tasks;

namespace TestingApp.Views
{
    public partial class SignUp : PhoneApplicationPage
    {

        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> todoTable = App.MobileService.GetTable<User>();
      
        public SignUp()
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
                btn_signUp.IsEnabled = true;
            }

        }

        private async Task InsertTodoItem(User todoItem)
        {

            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);

            MessageBox.Show("SignedUp Successfully!");


        }

        private async void btn_signUp_Click(object sender, RoutedEventArgs e)
        {
            var usr = new User { Email = tbx_Email.Text, Password = tbx_Password.Text, UserName = tbx_UserName.Text };
            await InsertTodoItem(usr);
        }


    }
}