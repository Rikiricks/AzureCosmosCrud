using Newtonsoft.Json;

namespace CosmosDemo.Models
{
    public class AuthorNew : Entity
    {
        [JsonProperty(PropertyName = "bookId", Required = Required.Always)]
        public string BookId { get; set; }
        [JsonProperty(PropertyName = "firstName", Required = Required.Always)]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName", Required = Required.Always)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
