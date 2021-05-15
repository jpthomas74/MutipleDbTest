using LearnNewMultiConnectionAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnNewMultiConnectionAPI.DataContext
{
    public class CustomerDataContext : DbContext
    {
        public CustomerDataContext(DbContextOptions<CustomerDataContext> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }

    }
}
