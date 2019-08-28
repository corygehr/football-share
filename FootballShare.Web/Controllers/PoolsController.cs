using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FootballShare.Web.Controllers
{
    public class PoolsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}