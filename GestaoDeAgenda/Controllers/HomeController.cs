using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GestaoDeAgenda.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestaoDeAgenda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectContext _context;

        public HomeController(ProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //var id = 3;
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(m => m.UserId == id);

            //HttpContext.Session.SetString("Test", "Eber Ortiz");
            //var user = await _context.Users.ToListAsync();
            //return View(user);

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                ViewBag.Name = HttpContext.Session.GetString("Name");
                ViewBag.Email = HttpContext.Session.GetString("Email");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                var userId = HttpContext.Session.GetInt32("UserId");

                // var data = _context.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                // var data = await _context.Events.ToListAsync();
                // var events = _context.Events.Include(e => e.UserEvent).ToList();
                var events = (from e in _context.Events
                              join ue in _context.UserEvents on e.EventId equals ue.EventId
                              where ue.UserId == userId
                              select new Event
                              {
                                  EventId = e.EventId,
                                  Type = e.Type,
                                  Name = e.Name,
                                  Description = e.Description,
                                  Date = e.Date,
                                  Local = e.Local,
                              });

                // Console.WriteLine(events);
                // return Json(new { events });
                return View(events.ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public IActionResult About()
        {
            ViewBag.Message = HttpContext.Session.GetString("Test");

            return View();
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // METHOD POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = _context.Users.FirstOrDefault(s => s.Email == _user.Email);

                if (check == null)
                {
                    
                    _user.Password = GetMD5(_user.Password);
                    _context.Add(_user);
                    _context.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {

                    ViewBag.error = "O e-mail já existe";
                    return View();

                }

            }

            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        // Recebe dados do formulario login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.error = "Prencha todos os campos";
                return View();
            }

            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = _context.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                
                if (data.Count() > 0)
                {
                    // return Json(new { data.FirstOrDefault().UserId });

                    // Add session
                    HttpContext.Session.SetString("Name", data.FirstOrDefault().Name);
                    HttpContext.Session.SetString("Email", data.FirstOrDefault().Email);
                    HttpContext.Session.SetInt32("UserId", data.FirstOrDefault().UserId);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Erro ao fazer login";
                    return View();
                }
            }

            return View();
        }

        // Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); // Remove session
            return RedirectToAction("Login");
        }

        // Criamos o string MD5 para a senha
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
