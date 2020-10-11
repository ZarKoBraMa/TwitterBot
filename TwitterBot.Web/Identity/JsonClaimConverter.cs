using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace TwitterBot.Web.Identity
{
    public class JsonClaimConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => (objectType == typeof(ClaimsPrincipal));

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var claimsDictionary = new Dictionary<string, string>();

            foreach (var claim in ((ClaimsIdentity)(value as ClaimsPrincipal).Identity).Claims)
            {
                claimsDictionary.Add(claim.Type, claim.Value);
            }

            new JsonSerializer().Serialize(writer, claimsDictionary);
        }
    }
}
