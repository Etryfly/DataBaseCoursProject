using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBKP.Controllers;
using DBKP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        
        public async Task<JsonResult> getId(string login, string passwordHash)
        {
            _logger.LogInformation("getId - Login: " + login + " password: " + passwordHash);
            UserModel user = await db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == passwordHash);
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
        
        public async Task<JsonResult> getUsers(string login, string passwordHash)
        {
            
            UserModel user = await db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == passwordHash && u.Role_id == 1);
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

        public async Task<JsonResult> getStatistics(string login, string passwordHash)
        {
            UserModel user = await db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == passwordHash && u.Role_id == 1);
            if (user == null)
            {
                return Json(new
                {
                    status = ReturnCode.InvalidLogin
                });
            }
           
            decimal totalPayed = await db.GameStats.SumAsync(g => g.payed);
            decimal totalWinned = await db.GameStats.SumAsync(g => g.chips_earned);
            return Json(new
            {
                status = ReturnCode.OK,
                payed = totalPayed,
                winned = totalWinned
            });
            
        }

        public async Task<JsonResult> addChipsToUser(string login, string passwordHash, int userId, int chips)
        {
            UserModel user = await db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == passwordHash && u.Role_id == 1);
            if (user == null)
            {
                return Json(new
                {
                    status = ReturnCode.InvalidLogin
                });
            }
            
            SqlParameter userIdParameter = new SqlParameter("@UserId", userId);
            SqlParameter chipsParameter = new SqlParameter("@ChipsAmount", chips);
            await db.Database.ExecuteSqlRawAsync("addChipsToUser @UserId, @ChipsAmount", 
                userIdParameter, chipsParameter);
             return Json(new
            {
                status = ReturnCode.OK
            });
        }
        
        public async Task<JsonResult> isAdmin(string login, string passwordHash)
        {
            UserModel user = await db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == passwordHash && u.Role_id == 1);

            if (user == null)
            {
                return Json(new
                {
                    status = ReturnCode.InvalidLogin
                });
            }
            return Json(new
            {
                status = ReturnCode.OK
            });
            
        }
        
    }
}