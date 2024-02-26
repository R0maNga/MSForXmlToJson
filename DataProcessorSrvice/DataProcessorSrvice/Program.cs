using DataProcessorService.DbContext;
using DataProcessorService.Services;
using Microsoft.EntityFrameworkCore;

namespace DataProcessorService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHostedService<BackgroundConsumerForJson>();
            builder.Services.AddDbContext<DataProcessorDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);

            var app = builder.Build();

            app.Run();
        }
    }
}