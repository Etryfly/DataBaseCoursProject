using System.Collections.Generic;
using System.Linq;
using DBKP.Controllers;
using DBKP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DBKP.admin
{
    public class AdminController : Controller
    {
        private ApplicationContext db;

        private readonly ILogger<AdminController> _logger;

        private struct ReturnCode
        {
            public static readonly string OK = "success";
            public static readonly string InvalidLogin = "InvalidLogin";
        }
        

        public AdminController(ILogger<AdminController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }
        
        
        
        [HttpGet]
        
        public JsonResult getId(string login, string passwordHash)
        {
            _logger.LogInformation("getId - Login: " + login + " password: " + passwordHash);
            UserModel user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == passwordHash);
            if (user == null)
            {
                return Json(new
                {
                    code = "error"
                });
            }
            _logger.LogInformation("Id = " + user.Id);
            return Json(new
            {
                code = ReturnCode.OK,
                id = user.Id
            });
        }
        
        public JsonResult getUsers(string login, string passwordHash)
        {
            
            UserModel user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == passwordHash);
            if (user == null)
            {
                return Json(new
                {
                    status = ReturnCode.InvalidLogin
                });
            }
            
            var users = db.Users.Join(db.Money, user => user.Id, money => money.Id, (user, money) => new
            {
                id = user.Id,
                money = money.Chips,
                login = user.Login,
                password = user.Password
            });
            
            _logger.LogInformation("Get users");
            return Json(new
            {
                status = ReturnCode.OK,
                UsersList = users
            });

        }

        public JsonResult getStatistics(string login, string passwordHash)
        {
            UserModel user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == passwordHash);
            if (user == null)
            {
                return Json(new
                {
                    status = ReturnCode.InvalidLogin
                });
            }
           
            decimal totalPayed = db.GameStats.Sum(g => g.payed);
            decimal totalWinned = db.GameStats.Sum(g => g.chips_earned);
            return Json(new
            {
                status = ReturnCode.OK,
                payed = totalPayed,
                winned = totalWinned
            });
            
        }
    }
}