using CosmosDemo.Database;
using CosmosDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDemo.Repository
{
    public class AuthorRepository : Repository<AuthorNew>
    {
        public AuthorRepository(CosmosContext dbContext) : base(dbContext)
        {
        }
    }
}
