﻿using System;
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
using Windows.Devices.Geolocation;
using System.Data.Linq.SqlClient;

namespace TestingApp
{
    public partial class MainHub : PhoneApplicationPage
    { 
        #region Decrarations


        private ObservableCollection<Achievements> obs_achvments = new ObservableCollection<Achievements>();
        private ObservableCollection<vene> obs_NearbyParks = new ObservableCollection<vene>();
        private ObservableCollection<Article> obs_Articles = new ObservableCollection<Article>();
        private MobileServiceCollection<Post, Post> items;
        private IMobileServiceTable<Post> todoTable = App.MobileService.GetTable<Post>();
        private MobileServiceCollection<Achievements, Achievements> Achitems;
        private IMobileServiceTable<Achievements> achivTable = App.MobileService.GetTable<Achievements>();

        
        double latitude, longitude;


        #endregion
        public MainHub()
        {
            InitializeComponent();
            GetArticlesData();
            GetLocation();

        }
        #region LOCATION

        private async void GetLocation()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                latitude = geoposition.Coordinate.Latitude;
                longitude = geoposition.Coordinate.Longitude;
                GetNearbyParks();
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    MessageBox.Show("location  is disabled in phone settings.");
                }
                //else
                {
                   
                }
            }
        }
        #endregion
        #region SelectionChanged
        void lbx_Posts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App._PostData = lbx_Posts.SelectedItem as Post;
            if (lbx_Posts.SelectedIndex == -1) 
            {
            
            }
            else
            {
                NavigationService.Navigate(new Uri("/Views/PostView.xaml", UriKind.RelativeOrAbsolute));
                lbx_Posts.SelectedIndex = -1;
            }

        }

        void lbx_articles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App._SelectedArticle = lbx_articles.SelectedItem as Article;
            if (lbx_articles.SelectedIndex == -1)
            {
             }
            else 
            {
                NavigationService.Navigate(new Uri("/Views/ViewArticle.xaml", UriKind.Relative));
                lbx_articles.SelectedIndex = -1;  
            }
        }

        void lbx_nearby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            GeoCoordinate geoCoord = new GeoCoordinate
            {
                Latitude = (lbx_nearby.SelectedItem as vene).lat,
                Longitude = (lbx_nearby.SelectedItem as vene).lng
            };
            BingMapsTask bmt = new BingMapsTask();
            bmt.Center = geoCoord;
            bmt.SearchTerm = (lbx_nearby.SelectedItem as vene).name;
            bmt.Show();

        }
#endregion
        #region Azure Mobile Service Code

        private async Task RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                    items = await todoTable
                    .Where(todoItem => todoItem.isDeleted == false)
                    .ToCollectionAsync();

                    Achitems = await achivTable
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
                foreach (Achievements ach in Achitems) 
                {
                    if (ach.AchievementBy.Equals(App._AppUser.Id)) 
                    {
                        obs_achvments.Add(ach);
                    }
                }


                lbx_Achv.ItemsSource = obs_achvments;
            }

        }

     
#endregion
        #region LifeCycle Events
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //Selection Changed Route Detached
            lbx_articles.SelectionChanged -= lbx_articles_SelectionChanged;
            lbx_nearby.SelectionChanged -= lbx_nearby_SelectionChanged;
            lbx_Posts.SelectionChanged -= lbx_Posts_SelectionChanged;

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            tbl_Stars.Text = App._AppUser.Stars + "";
            tbl_HealthPoints.Text = App._AppUser.HealthPoints + "";
            tbl_PetName.Text = App._AppUser.PetName;

            if (App.IsSessionDataAvailable == true)
            {
                tbl_avgspeed.Text = App.SessionData.AverageSpeed;
                tbl_calories.Text = App.SessionData.Calories;
                tbl_distance.Text = App.SessionData.Distance;
               tbl_pace.Text = App.SessionData.Pace;
                tbl_time.Text = App.SessionTime.ToString(@"hh\:mm\:ss");
            }
            
            //Selection Changed Route Attached
            lbx_articles.SelectionChanged += lbx_articles_SelectionChanged;
            lbx_nearby.SelectionChanged += lbx_nearby_SelectionChanged;
            lbx_Posts.SelectionChanged += lbx_Posts_SelectionChanged;


            try
            {
                await RefreshTodoItems();
            }
            catch (Exception exc)
            {
            }

        }
        #endregion
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
                wc_NearByParks.DownloadStringAsync(new Uri("https://api.foursquare.com/v2/venues/search?client_id=4UKK0YO0NDIKPWGOELRGU5TR2PZNQXOLJ3N42KKQRUX0DXLM&client_secret=XW1NITAJESHVCB3PTDPDMXJNALMGDDI21VYMX1Z5GSQWIVBU&v=20130815&ll="+latitude+","+longitude+"&query=park"));
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
        #region Appbar Navigations
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Calander.xaml", UriKind.Relative));
        }

        private void petStoreNavigation(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Store.xaml", UriKind.Relative));
     
        }

        private void postIInCommunityNavigation(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/CreatePost.xaml", UriKind.Relative));
        }
        #endregion
        #region Exercise Type Tap


        private void img_cycling_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ExcerciseType = "Cycling";
            NavigationService.Navigate(new Uri("/Views/ExerciseSession.xaml", UriKind.Relative));
        }

        private void img_running_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ExcerciseType = "Running";
            NavigationService.Navigate(new Uri("/Views/ExerciseSession.xaml", UriKind.Relative));
        
        }

        private void img_skating_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ExcerciseType = "Skating";
            NavigationService.Navigate(new Uri("/Views/ExerciseSession.xaml", UriKind.Relative));
        
        }

        private void img_sking_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ExcerciseType = "Sking";
            NavigationService.Navigate(new Uri("/Views/ExerciseSession.xaml", UriKind.Relative));
        
        }

        private void img_walk_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ExcerciseType = "Walk";
            NavigationService.Navigate(new Uri("/Views/ExerciseSession.xaml", UriKind.Relative));
        
        }

        #endregion

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/LeaderBoard.xaml", UriKind.Relative));
        }

        private void Path_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            lbx_Posts.ItemsSource = items.Where(x => x.PostTitle.Contains(tbx_search.Text));
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }
    
    }
}