using Newtonsoft.Json;
using System;

namespace TwitterBot.Framework.Types
{
    public class BaseType
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
