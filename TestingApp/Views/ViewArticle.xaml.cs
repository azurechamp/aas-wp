using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;


namespace TestingApp.Views
{
    public partial class ViewArticle : PhoneApplicationPage
    {
        public ViewArticle()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            tbl_Article.Text = App._SelectedArticle.title;
            tbl_disc.Text = App._SelectedArticle.disc;
            img_articleTitle.Source = new BitmapImage(new Uri(App._SelectedArticle.image));
           
        }

        private void appBar_browser_Click(object sender, EventArgs e)
        {
            WebBrowserTask showInWeb = new WebBrowserTask();
            showInWeb.URL = App._SelectedArticle.link;
            showInWeb.Show();
        }


        private void appBar_Share_Click(object sender, EventArgs e)
        {
            ShareLinkTask shareLink = new ShareLinkTask();
            shareLink.LinkUri = new Uri(App._SelectedArticle.link);
            shareLink.Title = App._SelectedArticle.title;
            shareLink.Message = App._SelectedArticle.disc;
            shareLink.Show();

        }
    }
}