using CosmosDemo.Database;
using CosmosDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmosDemo.Repository
{
    public class BookRepository : Repository<Book>
    {
        public BookRepository(CosmosContext dbContext) : base(dbContext)
        {
        }       
    }
}
