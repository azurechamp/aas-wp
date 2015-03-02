using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.Models
{
    public class NearyByJson
    {
        public List<vene> venes { get; set; }
    }
    public class vene 
    {
        public string name { get; set; }
        public string url { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public int distance { get; set; }

        public double lat { get; set; }
        public double lng { get; set; }
        public int checkinsCount { get; set; }
    }


}
