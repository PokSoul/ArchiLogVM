using Archi.Api.Data;
using Archi.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archi.Library.Test.data
{
    class MockDbContext : ArchiDbContext
    {
        public MockDbContext(DbContextOptions options) : base(options)
        {

        }

        public static MockDbContext GetDbContext(bool withData = true)
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase("dbtest").Options;
            var db = new MockDbContext(options);
            if (withData)
            {
                //ajouter plusieurs
                db.Customers.Add(new Customer { Active = true, Firstname = "Melvin 1", Email = "Melvin1@gmail.com", Phone = "Test" });
                db.Customers.Add(new Customer { Active = true, Firstname = "Melvin 2", Email = "Melvin2@gmail.com", Phone = "Test" });
                db.Customers.Add(new Customer { Active = true, Firstname = "Melvin 3", Email = "Melvin3@gmail.com", Phone = "Test" });
                db.SaveChanges();
            }
            return db;
        }
    }
}
