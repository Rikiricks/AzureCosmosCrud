using CosmosDemo.Models;
using CosmosDemo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDemo.Services
{
    public class AuthorService : Service<AuthorNew>, IAuthorService
    {
        public AuthorService(IRepository<AuthorNew> repository) : base(repository)
        {
        }
    }
}
