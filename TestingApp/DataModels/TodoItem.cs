using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestingApp
{
    public class TodoItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "petname")]
        public string PetName { get; set; }

        [JsonProperty(PropertyName = "age")]
        public string Age { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "height")]
        public string Height { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }




    }
}
