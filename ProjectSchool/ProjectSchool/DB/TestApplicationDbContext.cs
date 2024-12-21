using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProjectSchool.DB;
using ProjectSchool.DB; 

namespace ProjectSchool.Tests
{
    public class TestApplicationDbContext : ApplicationDbContext
    {
        public TestApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ISqlQueryExecutor sqlQueryExecutor) : base(options, sqlQueryExecutor)
        {
        }

        public new DatabaseFacade Database { get; set; }
    }
}