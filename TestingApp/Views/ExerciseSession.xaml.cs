using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using Microsoft.WindowsAzure.MobileServices;
using TestingApp.DataModels;
using System.Threading.Tasks;
using Coding4Fun.Toolkit.Controls;

namespace TestingApp.Views
{
    public partial class ExerciseSession : PhoneApplicationPage
    {


        private MobileServiceCollection<Session, Session> items;
        private IMobileServiceTable<Session> todoTable = App.MobileService.GetTable<Session>();
        private MobileServiceCollection<Achievements, Achievements> Achitems;
        private IMobileServiceTable<Achievements> achivTable = App.MobileService.GetTable<Achievements>();




        private double _kilometres;
        private long _previousPositionChangeTick;
        private GeoCoordinateWatcher _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        private MapPolyline _line;
        private DispatcherTimer _timer = new DispatcherTimer();
        private long _startTime;
        private string _azureStartTime;
        private string _azureEndTime;
        private DateTime _tempDateTime;



        public ExerciseSession()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;



            // create a line which illustrates the run
            _line = new MapPolyline();
            _line.StrokeColor = Colors.Red;
            _line.StrokeThickness = 5;
            Map.MapElements.Add(_line);

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            _watcher.Start();
            _watcher.PositionChanged += Watcher_PositionChanged;
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
                StartButton.IsEnabled = true;
            }
          
        }

        private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var coord = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            if (_line.Path.Count > 0)
            {
                // find the previos point and measure the distance travelled
                var previousPoint = _line.Path.Last();
                var distance = coord.GetDistanceTo(previousPoint);

                // compute pace
                var millisPerKilometer = (1000.0 / distance) * (System.Environment.TickCount - _previousPositionChangeTick);

                // compute total distance travelled
                _kilometres += distance / 1000.0;

                paceLabel.Text = TimeSpan.FromMilliseconds(millisPerKilometer).ToString(@"mm\:ss");
                distanceLabel.Text = string.Format("{0:f2} km", _kilometres);
                caloriesLabel.Text = string.Format("{0:f0}", _kilometres * 65);
            }


            Map.Center = coord;

            _line.Path.Add(coord);
            _previousPositionChangeTick = System.Environment.TickCount;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan runTime = TimeSpan.FromMilliseconds(System.Environment.TickCount - _startTime);
            timeLabel.Text = runTime.ToString(@"hh\:mm\:ss");
            App.SessionTime = runTime;
        }

        private  async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled)
            {

                _timer.Stop();
                _tempDateTime = DateTime.Now;
                _azureEndTime = _tempDateTime.ToString();
                StartButton.Content = "Start";

                int points = 0;

                /// <---TESTING-->
                //#region TESTING
                //if (_kilometres >=0)
                //{
                //    points = 100;
                //    App._isAchieved = true;
                //    App._nameOfAchi = "Moon Walk";

                //}
                //#endregion

                if (_kilometres <= 3) 
                {
                    points = 100;
                    App._isAchieved = true;
                    App._nameOfAchi = "Moon Walk";

                }
                if (_kilometres <= 5)
                {
                    points = 200;
                     App._isAchieved = true;
                     App._nameOfAchi = "Dragon On Fire";
                }
                if (_kilometres > 5)
                {
                    points = 300;
                    App._isAchieved = true;
                    App._nameOfAchi = "Rocking Dragon!!";

                }
                App.SessionData = new Session { AverageSpeed = paceLabel.Text , Calories = caloriesLabel.Text , Distance = distanceLabel.Text , StartTime = _azureStartTime , EndTime = _azureEndTime , Pace = paceLabel.Text , SessionBy = App._AppUser.Id , Points=  points.ToString() , ExerciseType = App.ExcerciseType };
                try
                {

                    if(App._isAchieved == true )
                    {
                        await InsertAchievement(new Achievements { achTitle = App._nameOfAchi , achDisc = "Gymnasio Achievement : " + App._nameOfAchi , onDistance = _kilometres.ToString() , AchievementBy = App._AppUser.Id  });
                        
                        
                        App._isAchieved = false;
                    }
                    await InsertTodoItem(App.SessionData);
                    MessageBox.Show("Peeep Peeep !! Your Session is Saved!");
                    MessagePrompt msgpmpt = new MessagePrompt();
                    msgpmpt.Title = "Achievement Unlocked";
                    msgpmpt.Body ="You have unlocked "+ App._nameOfAchi+"\nCONGRATULATIONS!!!!";
                    msgpmpt.Show();
                    App.IsSessionDataAvailable = true;
                }
                catch (Exception exc) 
                {
                    MessageBox.Show("Internet Connection Problem!");
                }
               }
                
            else
            {
                _timer.Start();
                _startTime = System.Environment.TickCount;
                _tempDateTime = DateTime.Now;
                _azureStartTime = _tempDateTime.ToString();
                StartButton.Content = "Stop";
            }
        }

        private async Task InsertAchievement(Achievements todoItem)
        {

            await achivTable.InsertAsync(todoItem);
            Achitems.Add(todoItem);
        }

        private async Task InsertTodoItem(Session todoItem)
        {

            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);


        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MainHub.xaml", UriKind.Relative));
        }

    }
}