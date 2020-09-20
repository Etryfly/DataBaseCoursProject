using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DB_KP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DB_KP.Controllers
{
    public class AuthenticationController : Controller
    {
        private ApplicationContext db;

        private readonly ILogger<HomeController> _logger;

        public AuthenticationController(ILogger<HomeController> logger, ApplicationContext context)
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
        public IActionResult Loginin(UserModel user)
        {
            return View();
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

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(UserModel model, string returnUrl)
        {
           
            string salt = model.Login;
            
            UserModel user = new UserModel();
            user.Login = model.Login;
            
            user.Password = getHash(salt + model.Password);
            _logger.LogInformation("Auth controller - Register method. Login: " + user.Login + " Password hash: " +
                                   user.Password);

            db.Add(user);
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