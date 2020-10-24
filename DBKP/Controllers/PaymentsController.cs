using System.Linq;
using DBKP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DBKP.Controllers
{
    public class PaymentsController : Controller
    { 
        private ApplicationContext db;

        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ApplicationContext db, ILogger<PaymentsController> logger)
        {
            _logger = logger;

            this.db = db;
        }
        // GET
        public IActionResult Pay()
        {
            ViewBag.message = "Enter your card information and count of chips";
            return View();
        }

        [HttpPost]
        public IActionResult Pay(decimal value)
        {
            if (value > 0)
            {
                UserModel user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                db.Money.FirstOrDefault(m => m.Id == user.Id).Chips += value;
                db.GameStats.FirstOrDefault(g => g.Id == user.Id).payed += value;
                db.SaveChanges();
                ViewBag.message = "Successful payment";
                return View();
            }

            ViewBag.message = "Something wrong with your transaction, try later";
            return View();
        }
    }
}