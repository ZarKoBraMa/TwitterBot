using Newtonsoft.Json;
using System;

namespace TwitterBot.Framework.Types
{
    public class BaseType
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "con")]
        public DateTime CreatedOn { get; set; }
        
        [JsonProperty(PropertyName = "cby")]
        public string CreatedBy { get; set; }
        
        [JsonProperty(PropertyName = "mon")]
        public DateTime ModifiedOn { get; set; }
        
        [JsonProperty(PropertyName = "mby")]
        public string ModifiedBy { get; set; }
        
        [JsonProperty(PropertyName = "isd")]
        public bool IsDeleted { get; set; }
    }
}
