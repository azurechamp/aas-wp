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
using System.Threading.Tasks;
using TestingApp.DataModels;
using System.Globalization;

namespace TestingApp.Views
{
    public partial class Profile : PhoneApplicationPage
    {
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> todoTable = App.MobileService.GetTable<User>();
      

        public Profile()
        {
            InitializeComponent();
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
       

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            tbx_Name.Text = App._AppUser.Name;
            tbx_Age.Text = App._AppUser.Age + "";
            tbx_Password.Text = App._AppUser.Password + "";
            tbx_Email.Text = App._AppUser.Email;
            tbx_UserName.Text = App._AppUser.UserName;
            tbx_Weight.Text = App._AppUser.Weight + "";
            tbx_Height.Text = App._AppUser.Height + "";
            tbx_PetName.Text = App._AppUser.PetName;
            tbx_Question.Text = App._AppUser.Question;
            tbx_Answer.Text = App._AppUser.Answer;

          await  RefreshTodoItems();

            
        }
        //private async Task UpdateCheckedTodoItem(User item)
        //{
        //    await todoTable.UpdateAsync(item);
        //}

        private async void btn_signUp_Click(object sender, RoutedEventArgs e)
        {
            var usr = new User { Email = tbx_Email.Text, Password = tbx_Password.Text, UserName = tbx_UserName.Text, Age = float.Parse(tbx_Age.Text, CultureInfo.InvariantCulture.NumberFormat), Height = float.Parse(tbx_Height.Text, CultureInfo.InvariantCulture.NumberFormat), Weight = float.Parse(tbx_Weight.Text, CultureInfo.InvariantCulture.NumberFormat), PetName = tbx_PetName.Text, Question = tbx_Question.Text, Answer = tbx_Answer.Text, HealthPoints = 100f, Name = tbx_Name.Text, Stars = 250f , Id = App._AppUser.Id};
            await UpdateCheckedTodoItem(usr);
            MessageBox.Show("Profile Updated! ");
        }

        private async Task UpdateCheckedTodoItem(User item)
        {
           await todoTable.UpdateAsync(item);
            items.Remove(item);
            

            //await SyncAsync(); // offline sync
        }
    }
}