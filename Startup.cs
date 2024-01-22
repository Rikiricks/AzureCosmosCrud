using CosmosDemo;
using CosmosDemo.Configuration;
using CosmosDemo.Database;
using CosmosDemo.Models;
using CosmosDemo.Repository;
using CosmosDemo.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

//[assembly: WebJobsStartup(typeof(Startup))]

[assembly: FunctionsStartup(typeof(Startup))]

namespace CosmosDemo
{
    //public class Startup : IWebJobsStartup
    //{
    //    public void Configure(IWebJobsBuilder builder)
    //    {
    //        var config = new ConfigurationBuilder()
    //            .SetBasePath(Environment.CurrentDirectory)
    //            .AddJsonFile("local.settings.json", true, true)
    //            .AddEnvironmentVariables()
    //            .Build();


    //        builder.Services.AddSingleton(new FunctionConfiguration(config));

    //        builder.Services.AddDbContext<CosmosContext>();

    //        builder.Services.AddScoped<IRepository<Book>, BookRepository>();
    //        builder.Services.AddScoped<IBookService, BookService>();
    //    }
    //}

    public class Startup : FunctionsStartup
    {
        public async override void Configure(IFunctionsHostBuilder builder)
        {
            var serviceProvider = ConfigureServices(builder.Services).BuildServiceProvider(true);
            var intiContainer = serviceProvider.GetRequiredService<InitializeContainer>();
            await intiContainer.CreateContainer();


        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            services
                .AddSingleton(new FunctionConfiguration(config));

            services
                .AddDbContext<CosmosContext>();

            services.AddTransient<InitializeContainer>();

            services
                .AddScoped<IRepository<Book>, BookRepository>()
                .AddScoped<IBookService, BookService>();


            services
                .AddScoped<IRepository<AuthorNew>, AuthorRepository>()
                .AddScoped<IAuthorService, AuthorService>();




            return services;
        }

        
    }
}