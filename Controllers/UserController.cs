using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebNaN.Models;
using System.Text;
using System.Security.Cryptography;
using System;

namespace WebNaN.Controllers
{
    public class UserController : Controller
    {
        private readonly WebNaNContext _sql;


        public UserController(WebNaNContext sql)
        {
            _sql = sql;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User u)
        {

            StringBuilder sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {

                byte[] inputBytes = Encoding.ASCII.GetBytes("WEB" + u.UserPassword + "NAN");
                byte[] outputBytes = md5.ComputeHash(inputBytes);

                for (int i = 0; i < outputBytes.Length; i++)
                {
                    sb.Append(outputBytes[i].ToString("X2"));
                }


            }

            var sifre = sb.ToString();

            var getUser = _sql.Users.Include(x => x.UserStatus).SingleOrDefault(x => x.UserName == u.UserName && x.UserPassword == sifre);
            if (getUser != null)
            {
                var claims = new List<Claim>

                {
                    new Claim(ClaimTypes.Name, getUser.UserName),
                    new Claim("Id",getUser.UserId.ToString()),
                    new Claim(ClaimTypes.Role, getUser.UserStatus.StatusName),
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var princitial = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princitial, props).Wait();
                return RedirectToAction("Index", "Home");

            }






            return View();
        }




        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");

        }




        public IActionResult SignUp()
        {
            return View();
        }




        [HttpPost]
        public IActionResult SignUp(User u)
        {
            var test = _sql.Users.Any(x => x.UserName == u.UserName);
            if (test)
            {
                ModelState.AddModelError("UserName", "Istifadeci adi istifade olunur");
                return View();
            }
            StringBuilder sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {

                byte[] inputBytes = Encoding.ASCII.GetBytes("WEB" + u.UserPassword + "NAN");
                byte[] outputBytes = md5.ComputeHash(inputBytes);

                for (int i = 0; i < outputBytes.Length; i++)
                {
                    sb.Append(outputBytes[i].ToString("X2"));
                }


            }

            var sifre = sb.ToString();

            u.UserStatusId = 3;
            u.UserPassword = sifre;
            _sql.Users.Add(u);
            _sql.SaveChanges();
            return RedirectToAction("SignUp");




         
        }
    }
}
