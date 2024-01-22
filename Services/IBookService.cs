using CosmosDemo.Models;
using System.Threading.Tasks;

namespace CosmosDemo.Services
{
    public interface IBookService : IService<Book>
    {
        public Task<bool> CheckForConflictingBook(Book book);
    }
}
