using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.Models
{
    public class Article
    {
        public string image { get; set; }
        public string title { get; set; }
        public string source { get; set; }
        public string disc { get; set; }
        public string link { get; set; }
    }

    public class RootArticles
    {
        public List<Article> Articles { get; set; }
    }
}
