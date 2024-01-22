using Microsoft.Extensions.Configuration;

namespace CosmosDemo.Configuration
{
    public class FunctionConfiguration
    {
        public string CosmosAccountEndpoint { get; }
        public string CosmosAccountKey { get; }
        public string CosmosDatabaseName { get; }

        public FunctionConfiguration(IConfiguration config)
        {
            CosmosAccountEndpoint = config["CosmosAccountEndpoint"];
            CosmosAccountKey = config["CosmosAccountKey"];
            CosmosDatabaseName = config["CosmosDatabaseName"];
        }
    }
}
