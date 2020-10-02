using System.Linq;
using DB_KP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DB_KP.Controllers
{
    public class BlackJackController : Controller
    {
        private ApplicationContext db;
        private readonly ILogger<BlackJackController> _logger;

        public BlackJackController(ApplicationContext db, ILogger<BlackJackController> logger)
        {
            _logger = logger;
            this.db = db;
        }

        [HttpGet]
        public IActionResult BlackJack()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("Login", "Please, login in");
                return RedirectToAction("Loginin", "Authentication");
            }
            
            // db.GetService<ILoggerFactory>().AddProvider(new DBLoggerProvider());
            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name );
            MoneyModel moneyModel = db.Money.FirstOrDefault(u => u.Id == userModel.Id);
            
            GameStatsModel gameStatsModel = db.GameStats.FirstOrDefault(u => u.Id == userModel.Id);
            _logger.LogInformation("BlackJack: real userId: " + userModel.Id + " id from money: "
            + moneyModel.Id + " money: " + moneyModel.Chips);
            userModel.Money = moneyModel;
            userModel.GameStats = gameStatsModel;
            
            return View(userModel);
        }

        [HttpPost]
        public JsonResult Bet(int count)
        {
           
            const bool NOT_ENOUGH_CHIPS = false;
            const bool OK = true;
            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name );
            MoneyModel moneyModel = db.Money.FirstOrDefault(u => u.Id == userModel.Id);
            _logger.LogDebug("BlackJack - Bet -  count: " + count);
            // ViewBag.Count = count;
            bool status;
            if (moneyModel == null) RedirectToAction("Error", "Home");
            if (moneyModel.Chips >= count && count > 0)
            {
                moneyModel.Chips -= count;
                status = OK;
            }
            else
            {
                status = NOT_ENOUGH_CHIPS;
            }

            db.SaveChanges();
            
            return Json(new
            {
                chips = moneyModel.Chips,
                operationStatus = status
            });

        }
        
    }
}