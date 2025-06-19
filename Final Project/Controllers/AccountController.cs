using Final_Project.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Final_Project.Data;

namespace Final_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext dbcontext;

        public AccountController(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public IActionResult Login() => View();
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = dbcontext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            var exists = dbcontext.Users.Any(u => u.Username == username);
            if (exists)
            {
                ViewBag.Error = "Username already exists";
                return View();
            }

            dbcontext.Users.Add(new UserLR { ID = Guid.NewGuid(), Username = username, Password = password });
            dbcontext.SaveChanges();

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
