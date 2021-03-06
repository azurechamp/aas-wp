﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Microsoft.Phone.Tasks;

namespace TestingApp.Views
{
    public partial class Calander : PhoneApplicationPage
    {

        const string CurrentEntryDateKey = "CurrentEntryDateKey";

        DateTime? _entryDate = DateTime.Now;

        Dictionary<DateTime, string> _dummyRepository = new Dictionary<DateTime, string>();

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (PhoneApplicationService.Current.State.ContainsKey(CurrentEntryDateKey))
                _entryDate = (DateTime)PhoneApplicationService.Current.State[CurrentEntryDateKey];

            InitializeCalendar(_entryDate.Value);

        }

        private void OnChangeMonth(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "NextBtn")
                _entryDate = _entryDate.Value.AddMonths(1);
            else
                _entryDate = _entryDate.Value.AddMonths(-1);

            //saving the entry date to restore the state after Tombstoning
            PhoneApplicationService.Current.State[CurrentEntryDateKey] = _entryDate;

            CalendarListBox.Visibility = Visibility.Collapsed;

            //Redraw the calendar
            InitializeCalendar(_entryDate.Value);
        }

        protected void InitializeCalendar(DateTime entryDate)
        {
            MonthYear.Text = String.Format("{0:MMMM yyyy}", _entryDate.Value);

            DateTime todaysDate = DateTime.Now;
            bool isTodaysDate = false;

     
            int numDays = DateTime.DaysInMonth(entryDate.Year, entryDate.Month);
            //check if the day buttons are already added
            int count = CalendarWrapPanel.Children.Count;
            if (count > numDays)
            {
                //remove days from the end
                for (int i = 1; i <= count - numDays; i++)
                    CalendarWrapPanel.Children.RemoveAt(count - i);
            }
            else
            {
                //calculate number of days to add
                int start = count + 1;
                for (int i = start; i <= numDays; i++)
                {
                    Border border = new Border();
                    border.Background = new SolidColorBrush(Color.FromArgb(255, 103, 183, 212));
                    border.Margin = new Thickness(0, 0, 5, 5);
                    border.Width = 99;
                    border.Height = 99;
                    border.CornerRadius = new CornerRadius(20);

                    Button btn = new Button();
                    btn.Name = "Day" + i;
                    btn.Content = i.ToString();
                    btn.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    btn.Width = 99;
                    btn.Height = 99;
                    btn.FontSize = 32;
                    border.Child = btn;
                    btn.Style = this.Resources["ButtonStyle1"] as Style;

                    //btn.Margin = new Thickness(0, 0, 5, 5);
                    btn.Click += new RoutedEventHandler(OnDayButtonClick);

                    CalendarWrapPanel.Children.Add(border);
                }
            }

           

            //reset the backgrounds as necessary
            for (int i = 0; i < numDays; i++)
            {
                Border border = (Border)CalendarWrapPanel.Children[i];
                if (border != null)
                {
                    Button btn = (Button)border.Child;
                    //check if user has entered data for this day
                    if (isTodaysDate && (i + 1) > todaysDate.Day)
                    {
                        //disable future days
                        btn.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        btn.IsEnabled = false;
                    }
                    else
                        btn.IsEnabled = true;

                    bool isToday = false;
                    DateTime currDate = new DateTime(entryDate.Year, entryDate.Month, i + 1);
                    //if this is the current date, set the background color to orange
                    if (currDate.Date.CompareTo(DateTime.Now.Date) == 0)
                    {
                        border.Background = new SolidColorBrush(Color.FromArgb(255, 255, 165, 0));
                        isToday = true;
                    }
                    else
                    {
                        border.Background = new SolidColorBrush(Color.FromArgb(255, 103, 183, 212));
                    }

                    //check if there's any data available for this day
                    string data;
                    _dummyRepository.TryGetValue(new DateTime(entryDate.Year, entryDate.Month, i + 1), out data);

                    if (data != null)
                    {
                        //if there's data for this day, set the button fore ground color to Orange
                        if (isToday)
                            btn.Style = this.Resources["TodayHasDataButtonStyle"] as Style;
                        else
                            btn.Style = this.Resources["HasDataButtonStyle"] as Style;
                    }
                    else
                    {
                        //there's no data for this day, set the button foreground to White
                        btn.Style = this.Resources["ButtonStyle1"] as Style;
                    }

                }
            }
            CalendarWrapPanel.UpdateLayout();
            CalendarListBox.Visibility = Visibility.Visible;

        }

        private void OnDayButtonClick(object sender, RoutedEventArgs e)
        {
            
            //InitializeCalendar(_entryDate.Value);
            DateTime selectedDate = new DateTime(_entryDate.Value.Year, _entryDate.Value.Month, Int32.Parse((string)((Button)sender).Content));
            SaveAppointmentTask saveAppointmentTask = new SaveAppointmentTask();
            saveAppointmentTask.StartTime = selectedDate;

            saveAppointmentTask.Subject = "Exercise - Gymnasio";
            saveAppointmentTask.IsAllDayEvent = false;
            saveAppointmentTask.Details = "GYMNASIO - GET READY FOR WORK OUT !";
            saveAppointmentTask.Show();

        }

        public Calander()
        {
            InitializeComponent();
        }

        private void CalendarListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}