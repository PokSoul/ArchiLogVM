using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Archi.Api.Data;
using Archi.Api.Models;
using Archi.Library.Controllers;

namespace Archi.Api.Controllers
{
   
    public class CustomersController : BaseController<ArchiDbContext, Customer>
    {

        public CustomersController(ArchiDbContext context) : base(context)
        {

        }


   
    }
}
