using Archi.Api.Controllers;
using Archi.Api.Models;
using Archi.Library.Models;
using Archi.Library.Test.data;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Archi.Library.Test
{
    public class CustomerControllerTest
    {

        private CustomersController _controller;
        private MockDbContext _context;

        [SetUp]
        public void Setup()
        {
            _context = MockDbContext.GetDbContext();
            _controller = new CustomersController(_context);
        }

        [Test]
        public async Task TestGetAll()
        {
            var actionResult = await _controller.GetAll(new Params());
            //var result = actionResult.Result as ObjectResult;
            var values = actionResult.Value as IEnumerable<Customer>;

            //Assert.AreEqual((int)HttpStatusCode.OK, values);
            Assert.IsNotNull(values);
            Assert.AreEqual(_context.Customers.Count(), values.Count());
        }

        [Test]
        public void TestPost()
        {
            
            Assert.Pass();
        }
    }
}
