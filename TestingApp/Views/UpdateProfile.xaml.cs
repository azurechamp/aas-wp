using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TestingApp.Views
{
    public partial class UpdateProfile : PhoneApplicationPage
    {
        public UpdateProfile()
        {
            InitializeComponent();
        }
        protected  override void OnNavigatedTo(NavigationEventArgs e)
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
        }
    }
}