using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Admin.DBContext;
using Admin.Models;
using Admin.Data;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly ApplicationContext db;
        public ApplicationsController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {

            return PartialView();
        }




    }
}