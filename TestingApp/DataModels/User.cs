using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.DataModels
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }


        [JsonProperty(PropertyName = "age")]
        public float Age { get; set; }

        [JsonProperty(PropertyName = "height")]
        public float Height { get; set; }


        [JsonProperty(PropertyName = "weight")]
        public float Weight { get; set; }


        [JsonProperty(PropertyName = "petname")]
        public string PetName { get; set; }


        [JsonProperty(PropertyName = "healthpoints")]
        public float HealthPoints { get; set; }


        [JsonProperty(PropertyName = "stars")]
        public float Stars { get; set; }


        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }


        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }


        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }


        [JsonProperty(PropertyName = "_deleted")]
        public bool isDeleted { get; set; }
    }
}
