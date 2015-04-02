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
using TestingApp.Models;
using Coding4Fun.Toolkit;
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using TestingApp.DataModels;
namespace TestingApp.Views
{
    public partial class Store : PhoneApplicationPage
    {
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> todoTable = App.MobileService.GetTable<User>();
        private Item selectedItem;

        public Store()
        {
            InitializeComponent();
            this.DataContext = new StorePageView_Model();
            
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
               btn_buy.IsEnabled = true;
            
            }


        }
        protected  async override void OnNavigatedTo(NavigationEventArgs e)
        {
            tbl_Stars.Text = App._AppUser.Stars + "";
            tbl_HealthPoints.Text = App._AppUser.HealthPoints + "";

            lbx_store.SelectionChanged += lbx_store_SelectionChanged;
            lbx_drinks.SelectionChanged += lbx_drinks_SelectionChanged;
            try
            {
                await RefreshTodoItems();
            }
            catch (Exception exc)
            {
            
            }
        }

        void lbx_drinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbl_HealthPoints_Copy.Text = (lbx_drinks.SelectedItem as Item).health;
            tbl_Stars_Copy.Text = (lbx_drinks.SelectedItem as Item).stars;
            tbl_prodtitle.Text = (lbx_drinks.SelectedItem as Item).name;
            img_prod.Source = new BitmapImage(new Uri((lbx_drinks.SelectedItem as Item).image));
            selectedItem = lbx_drinks.SelectedItem as Item;
            BuyItemPopUp.IsOpen = true;
        }


        void lbx_store_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbl_HealthPoints_Copy.Text = (lbx_store.SelectedItem as Item).health;
            tbl_Stars_Copy.Text = (lbx_store.SelectedItem as Item).stars;
            tbl_prodtitle.Text = (lbx_store.SelectedItem as Item).name;
            img_prod.Source = new BitmapImage(new Uri((lbx_store.SelectedItem as Item).image));
            selectedItem = lbx_store.SelectedItem as Item;
            BuyItemPopUp.IsOpen = true;
        }


     

        private async Task UpdateCheckedTodoItem(User item)
        {
            await todoTable.UpdateAsync(item);
            items.Remove(item);


            //await SyncAsync(); // offline sync
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            lbx_store.SelectionChanged -= lbx_store_SelectionChanged;
            lbx_drinks.SelectionChanged -= lbx_drinks_SelectionChanged;
          
        }

        private void closePopUp(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            BuyItemPopUp.IsOpen = false;
        
        }

        private async void buyItem(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            float finalStars, finalHealth ;
            if (App._AppUser.Stars > float.Parse(selectedItem.stars))
            {
                finalStars = App._AppUser.Stars - float.Parse(selectedItem.stars);
                finalHealth = App._AppUser.HealthPoints + float.Parse(selectedItem.health);
                var usr = new User { Email = App._AppUser.Email, Password = App._AppUser.Password, UserName = App._AppUser.UserName, Age = App._AppUser.Age, Height = App._AppUser.Height, Weight = App._AppUser.Weight, PetName = App._AppUser.PetName, Question = App._AppUser.Question, Answer = App._AppUser.Answer, HealthPoints = finalHealth, Name = App._AppUser.Name, Stars = finalStars, Id = App._AppUser.Id };
                await UpdateCheckedTodoItem(usr);
                MessageBox.Show("Item Bought !!! Yepiee! ");
                BuyItemPopUp.IsOpen = false;
            
            }
            else 
            {
                BuyItemPopUp.IsOpen = false;
            
                MessagePrompt msg = new MessagePrompt();
                msg.Title = "More Workout Needed";
                msg.Body = "Your stars are less then \nrequired to buy this item\nWorkout more to buy";
                msg.Show();
            }
            
        }

    }
}