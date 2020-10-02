using Newtonsoft.Json;
using System.Collections.Generic;

namespace TwitterBot.Framework.Types
{
    public class User : BaseType
    {
        [JsonProperty(PropertyName = "uid")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "hts")]
        public List<Hashtag> Hashtags { get; set; }
    }
}
