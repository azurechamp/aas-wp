using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestingApp.Models;

namespace TestingApp.ViewModels
{
     public class StorePageView_Model
    {
         public ObservableCollection<Item> obs_Food { get; set; }
         public ObservableCollection<Item> obs_Drink { get; set; }
         
        public StorePageView_Model() 
        {
              obs_Food = new ObservableCollection<Item>();
              obs_Drink = new ObservableCollection<Item>();
              GetJson();
        }


        public void GetJson() 
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringAsync(new Uri("http://hello987.azurewebsites.net/store.html"));
                webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
            }
            catch (Exception exc) 
            {
            
            
            }
        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rootObject = JsonConvert.DeserializeObject<RootStore>(e.Result);
            foreach (Item everyItemInList in rootObject.items) 
            {
                if (everyItemInList.tag.Equals("food"))
                {
                    obs_Food.Add(everyItemInList);
                }
                if (everyItemInList.tag.Equals("drink"))
                {
                    obs_Drink.Add(everyItemInList);
                    
                }
            }
        }
    }
}
