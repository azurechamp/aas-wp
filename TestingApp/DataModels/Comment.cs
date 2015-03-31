using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.DataModels
{
    public class Comment
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "CommentBy")]
        public string CommentOf { get; set; }

        [JsonProperty(PropertyName = "com")]
        public string CommentDisc { get; set; }

        [JsonProperty(PropertyName = "PostId")]
        public string PostNumber { get; set; }

        [JsonProperty(PropertyName = "_deleted")]
        public bool isDeleted { get; set; }
    }
}
