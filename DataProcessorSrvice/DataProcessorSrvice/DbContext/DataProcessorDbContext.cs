using DataProcessorService.Models;
using Microsoft.EntityFrameworkCore;

namespace DataProcessorService.DbContext
{
    public class DataProcessorDbContext:Microsoft.EntityFrameworkCore.DbContext
    {

        public DataProcessorDbContext(DbContextOptions<DataProcessorDbContext> options) : base(options)
        {

        }
        public DbSet<RapidControlStatus> RapidControlStatus { get; set; }
    }
}
