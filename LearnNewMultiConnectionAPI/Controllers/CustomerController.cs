using LearnNewMultiConnectionAPI.DataContext;
using LearnNewMultiConnectionAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnNewMultiConnectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDataContext CustomerDataContext;

        public CustomerController(CustomerDataContext customerDataContext)
        {
            CustomerDataContext = customerDataContext;
        }

        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return CustomerDataContext.Customers;
        }
    }
}
