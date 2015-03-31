using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TestingApp.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Microsoft.Phone.Tasks;
using System.Device.Location;
using Microsoft.WindowsAzure.MobileServices;
using TestingApp.DataModels;
using System.Threading.Tasks;

namespace TestingApp
{
    public partial class MainHub : PhoneApplicationPage
    { 
        
#region Decrarations

        ObservableCollection<vene> obs_NearbyParks = new ObservableCollection<vene>();
        ObservableCollection<Article> obs_Articles = new ObservableCollection<Article>();
        private MobileServiceCollection<Post, Post> items;
        private IMobileServiceTable<Post> todoTable = App.MobileService.GetTable<Post>();
     

#endregion
       

        public MainHub()
        {
            InitializeComponent();
            GetNearbyParks();
            GetArticlesData();


            lbx_nearby.SelectionChanged += lbx_nearby_SelectionChanged;
        }

#region azure code

        void lbx_Posts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // App._PostData = lbx_Posts.SelectedItem as Post;
            // NavigationService.Navigate(new Uri("/PostView.xaml", UriKind.RelativeOrAbsolute));  
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            tbl_Stars.Text = App._AppUser.Stars +"";
            tbl_HealthPoints.Text = App._AppUser.HealthPoints +"";
            tbl_PetName.Text = App._AppUser.PetName;


            try
            {
                await RefreshTodoItems();
                lbx_Posts.SelectionChanged += lbx_Posts_SelectionChanged;
            }
            catch (Exception exc)
            {
            }
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
                lbx_Posts.ItemsSource = items;

            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            lbx_Posts.SelectionChanged -= lbx_Posts_SelectionChanged;

        }





#endregion

        //NearbyPark SelectionChanged Routed Event
        void lbx_nearby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            GeoCoordinate geoCoord = new GeoCoordinate
            {
                 Latitude =(lbx_nearby.SelectedItem as vene).lat,
                 Longitude = (lbx_nearby.SelectedItem as vene).lng
            };
          BingMapsTask bmt = new BingMapsTask();
          bmt.Center = geoCoord;
          bmt.SearchTerm = (lbx_nearby.SelectedItem as vene).name;
          bmt.Show();
            
        }

 #region JSON data
        void GetArticlesData() 
        {
            try
            {
                WebClient wc_Articles = new WebClient();

                //Passing Dummy Location Right Now (3/3/2015)
                wc_Articles.DownloadStringAsync(new Uri("http://hello987.azurewebsites.net/art.html"));
                wc_Articles.DownloadStringCompleted+=wc_Articles_DownloadStringCompleted;

            }
            catch (Exception exc)
            {
                MessageBox.Show("ErrorLoadingData!");
            }
        }

        private void wc_Articles_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rootObject = JsonConvert.DeserializeObject<RootArticles>(e.Result);
            foreach (Article act in rootObject.Articles) 
            {
                obs_Articles.Add(act);
            }

            lbx_articles.ItemsSource = obs_Articles;
        }


        //Gets Nearby Parks JSON DATA 
         void GetNearbyParks()
        {
            try
            {
                WebClient wc_NearByParks = new WebClient();
               
                //Passing Dummy Location Right Now (3/3/2015)
                wc_NearByParks.DownloadStringAsync(new Uri("https://api.foursquare.com/v2/venues/search?client_id=4UKK0YO0NDIKPWGOELRGU5TR2PZNQXOLJ3N42KKQRUX0DXLM&client_secret=XW1NITAJESHVCB3PTDPDMXJNALMGDDI21VYMX1Z5GSQWIVBU&v=20130815&ll=40.7,-74&query=park"));
                wc_NearByParks.DownloadStringCompleted += wc_NearByParks_DownloadStringCompleted;
            }
            catch (Exception exc) 
            {
                MessageBox.Show("ErrorLoadingData!");
            }
        }
        
        
        //
        void wc_NearByParks_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rootObject = JsonConvert.DeserializeObject<RootParks>(e.Result);
            foreach (Venue ven in rootObject.response.venues) 
            {
                vene nearbyVar = new vene();
                nearbyVar.name = ven.name;
                nearbyVar.country = ven.location.country;
                nearbyVar.city = ven.location.city;
                nearbyVar.phone = ven.contact.phone;
                nearbyVar.distance = ven.location.distance;
                nearbyVar.url = ven.url;
                nearbyVar.lat = ven.location.lat;
                nearbyVar.lng = ven.location.lng;
                nearbyVar.checkinsCount = ven.stats.checkinsCount;

                obs_NearbyParks.Add(nearbyVar);
            }

            lbx_nearby.ItemsSource = obs_NearbyParks;
        }
        #endregion

       

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Calander.xaml", UriKind.Relative));
        }
    }
}