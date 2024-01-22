using Newtonsoft.Json;

namespace CosmosDemo.Models
{
    public interface IEntity
    {
        [JsonProperty("id",NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }
}
