using Newtonsoft.Json;

namespace CosmosDemo.Models
{
    public abstract class Entity : IEntity
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }
}
