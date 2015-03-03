using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.Models
{
    public class Item
    {
        public string name { get; set; }
        public string stars { get; set; }
        public string health { get; set; }
        public string tag { get; set; }
        public string image { get; set; }
    }

    public class RootStore
    {
        public List<Item> items { get; set; }
    }
}
