using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.DataModels
{
    public class Post
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "PostedBy")]
        public string PostBy { get; set; }

        [JsonProperty(PropertyName = "posttitle")]
        public string PostTitle { get; set; }

        [JsonProperty(PropertyName = "postdis")]
        public string PostDisc { get; set; }


        [JsonProperty(PropertyName = "_deleted")]
        public bool isDeleted { get; set; }

    }
}
