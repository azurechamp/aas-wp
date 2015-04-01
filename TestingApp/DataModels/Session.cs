using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.DataModels
{
    public class Session
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "SessionBy")]
        public string SessionBy { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public string Distance { get; set; }

        [JsonProperty(PropertyName = "calories")]
        public string Calories { get; set; }

        [JsonProperty(PropertyName = "pace")]
        public string Pace { get; set; }

        [JsonProperty(PropertyName = "avgspeed")]
        public string AverageSpeed { get; set; }

        [JsonProperty(PropertyName = "points")]
        public string Points { get; set; }

        [JsonProperty(PropertyName = "starttime")]
        public string StartTime { get; set; }

        [JsonProperty(PropertyName = "endtime")]
        public string EndTime { get; set; }

        [JsonProperty(PropertyName = "exercisetype")]
        public string ExerciseType { get; set; }

        [JsonProperty(PropertyName = "_deleted")]
        public bool isDeleted { get; set; }
  
    }
}
