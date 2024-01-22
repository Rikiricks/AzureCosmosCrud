using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CosmosDemo.Services;
using CosmosDemo.Models;
using Microsoft.Extensions.Logging;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;


namespace CosmosDemo
{
    public class BookFunction
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        public BookFunction(IBookService bookService, IAuthorService authorService)
        {
            _bookService = bookService;
            _authorService = authorService;
        }

        [FunctionName("BookFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("CreateBook")]
        public async Task<IActionResult> CreateBook([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "books")] HttpRequest req, ILogger log)
        {
            var bookJson = await req.ReadAsStringAsync();

            try
            {
                var book = JsonConvert.DeserializeObject<Book>(bookJson);

                if (await _bookService.CheckForConflictingBook(book))
                {
                    return new ConflictObjectResult($"Book with matching title already exists in library: \"{book.Title}\"");
                }

                await _bookService.Create(book);

                var authors = book.Authors;
                if (authors.Any())
                {
                    var authorsNew = new List<AuthorNew>();
                    foreach (var author in authors)
                    {
                        authorsNew.Add(new AuthorNew()
                        {
                            BookId = book.Id,
                            FirstName = author.FirstName,
                            LastName = author.LastName,
                            Description = author.Description,
                        });
                    }

                    await _authorService.CreateMany(authorsNew);
                }
                
                return new OkObjectResult(book);
            }
            catch (Exception e)
            {
                var errorMessage = $"Failed to create a book: {bookJson}";

                log.LogError(e, errorMessage);
                return new BadRequestObjectResult(errorMessage);
            }
        }

        [FunctionName("GetBooks")]
        public async Task<IActionResult> GetBooks([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books")] HttpRequest req, ILogger log)
        {

            try
            {
                var books = await _bookService.GetAll();

                return new OkObjectResult(books);
            }
            catch (Exception e)
            {
                var errorMessage = $"Failed to fetch all books";

                log.LogError(e, errorMessage);
                return new InternalServerErrorResult();
            }
        }

        [FunctionName("GetBook")]
        public async Task<IActionResult> GetBook([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books/{id}")] HttpRequest req, ILogger log, string id)
        {

            try
            {
                var book = await _bookService.GetById(id);

                if (book == null)
                {
                    return new UnprocessableEntityObjectResult($"No book exists with id: {id}");
                }

                return new OkObjectResult(book );
            }
            catch (Exception e)
            {
                var errorMessage = $"Failed to fetch a book with id: {id}";

                log.LogError(e, errorMessage);
                return new InternalServerErrorResult();
            }
        }

        [FunctionName("UpdateBook")]
        public async Task<IActionResult> UpdateBook([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "books/{id}")] HttpRequest req,
            ILogger log,
            string id)
        {
            var bookJson = await req.ReadAsStringAsync();

            try
            {
                var book = JsonConvert.DeserializeObject<Book>(bookJson);
                book.Id = id;

                await _bookService.Update(book);

                return new OkObjectResult(book);
            }
            catch (Exception e)
            {
                var errorMessage = $"Failed to update book with id: {id} with details: {bookJson}";

                log.LogError(e, errorMessage);
                return new BadRequestObjectResult(errorMessage);
            }
        }

        [FunctionName("DeleteBook")]
        public async Task<IActionResult> DeleteBook([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "books/{id}")] HttpRequest req,
            ILogger log,
            string id)
        {
            try
            {
                await _bookService.Delete(id);

                return new NoContentResult();
            }
            catch (Exception e)
            {
                var errorMessage = $"Failed to delete book with id: {id}";

                log.LogError(e, errorMessage);
                return new InternalServerErrorResult();
            }
        }

    }
}
