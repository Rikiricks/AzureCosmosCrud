using CosmosDemo.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Configuration.Internal;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CosmosDemo.Database
{
    public class InitializeContainer
    {
        private readonly string[] _containers = new string[] { "Book", "AuthorNew" };
        private readonly FunctionConfiguration _config;
        public InitializeContainer(FunctionConfiguration config)
        {
            _config = config;
        }
        public async Task CreateContainer()
        {
            var client = new Microsoft.Azure.Cosmos.CosmosClient(_config.CosmosAccountEndpoint, _config.CosmosAccountKey);
            var database = await client.CreateDatabaseIfNotExistsAsync(_config.CosmosDatabaseName);
            foreach (var item in _containers)
            {
                await database.Database.CreateContainerIfNotExistsAsync(item, "/id");

            }
        }
    }
}
