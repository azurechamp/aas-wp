using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp.DataModels
{
    public class Achievements
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "AchievementBy")]
        public string AchievementBy { get; set; }

        [JsonProperty(PropertyName = "ondistance")]
        public string onDistance { get; set; }

        [JsonProperty(PropertyName = "achtitle")]
        public string achTitle { get; set; }

        [JsonProperty(PropertyName = "achdisc")]
        public string achDisc { get; set; }

        [JsonProperty(PropertyName = "_deleted")]
        public bool isDeleted { get; set; }

    }
}
