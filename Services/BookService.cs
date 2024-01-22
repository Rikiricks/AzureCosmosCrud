using CosmosDemo.Models;
using CosmosDemo.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDemo.Services
{
    public class BookService : Service<Book>, IBookService
    {
        public BookService(IRepository<Book> repository) : base(repository) { }

        public async Task<bool> CheckForConflictingBook(Book book)
        {
            return (await _repository.GetByCondition(x => x.Title == book.Title)).Any();
        }
    }
}
