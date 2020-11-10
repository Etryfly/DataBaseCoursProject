using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DBKP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DBKP.Controllers
{
    public class AuthenticationController : Controller
    {
        private ApplicationContext db;

        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Loginin(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Loginin(UserModel user)
        {
            string passwordHash = getHash( user.Login + user.Password);
            var userFromDb = db.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == passwordHash);
            if (userFromDb != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };
                // создаем объект ClaimsIdentity
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(id));
                    
                return RedirectToAction("Index", "Home");

            }
            ModelState.AddModelError("Login", "Invalid login or password");
            return View(user);


        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        private string getHash(string password)
        {
            byte[] hash;
           
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            _logger.LogDebug("MD5 hash: password to bytes " + inputBytes);
            hash = new MD5CryptoServiceProvider().ComputeHash(inputBytes);
            _logger.LogDebug("MD5 hash: bytes to MD5 " + hash);

            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            _logger.LogDebug("MD5 hash: hash to hex" + sb.ToString());


            return sb.ToString();
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(UserModel model, string returnUrl)
        {
            if (db.Users.FirstOrDefault(u => u.Login == model.Login)!=null)
            {
                ModelState.AddModelError("Login", "Login already claimed");
                return View(model);
                
            }

            string salt = model.Login;
            
            UserModel user = new UserModel();
            MoneyModel money = new MoneyModel();
            user.Money = money;
            
            
            user.Login = model.Login;
            
            user.Password = getHash(salt + model.Password);
            _logger.LogInformation("Auth controller - Register method. Login: " + user.Login + " Password hash: " +
                                   user.Password);
        
            db.Money.Add(money);
            db.SaveChanges();
            user.Id = money.Id;
            _logger.LogInformation(db.SaveChanges().ToString());
            
            user.GameStats = new GameStatsModel(){chips_earned = 0, Loses = 0, Wins = 0, Id = money.Id};
            db.GameStats.Add(user.GameStats);
            db.Users.Add(user);
                
            db.SaveChanges();
           
            
            TempData["login"] = user.Login;
            return RedirectToAction("SuccessRegister");
        }
        
        

        public IActionResult SuccessRegister()
        {
            return View();
        }
        
        
    }
}