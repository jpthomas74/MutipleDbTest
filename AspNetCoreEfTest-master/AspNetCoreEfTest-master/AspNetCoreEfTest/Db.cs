using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreEfTest
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Db: DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
    }
}
