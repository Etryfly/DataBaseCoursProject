using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DB_KP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DB_KP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext db;
        
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> Leaderboard()
        {
            // _logger.LogInformation(db.Users.ToListAsync().Result[0].Name);
            return View(await db.Users.ToListAsync());
        }

        // [HttpPost] //TODO fix this
        // public async Task<IActionResult> Create(UserModel user)
        // {
        //     
        //     
        //     db.Users.Add(user);
        //     await db.SaveChangesAsync();
        //     return RedirectToAction("Leaderboard");
        // }
       
        
    }
}