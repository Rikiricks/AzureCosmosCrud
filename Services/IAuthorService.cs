using CosmosDemo.Models;
using CosmosDemo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDemo.Services
{
    public interface IAuthorService : IRepository<AuthorNew>
    {
    }
}
