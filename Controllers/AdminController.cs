using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.Models;
using System;
using System.Collections.Generic;

namespace Projekt.Controllers
{
    /// <summary>
    /// Admin-Controller för ADMIN
    /// </summary>
    public class AdminController : Controller
    {
        const string SessionName = "Admin";

        public IActionResult Start()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionName) != null)
            {
                HttpContext.Session.GetString(SessionName);
                return View("Adminsida");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Admin/Adminsida")]
        public ActionResult Adminsida()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View("AdminLogin");
        }

        [Route("Admin/Services")]
        // KONTROLLERAR OM SESSIONEN FINNS;
        public IActionResult Services()
        {
            if (HttpContext.Session.GetString(SessionName) != null)
            {
                return View("Services");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear(); // RADERAR SESSIONEN;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AdminLogin(LoginData logindata)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home");
            else
            {
                KeyValuePair<string, string> adminlogin = FileHandler.GetLogInfo();
                if (adminlogin.Key.Equals(logindata.UserName) &&
                    adminlogin.Value.Equals(logindata.Password))
                {
                    // SKAPAR EN GILTIG TEMPORÄR SESSION;
                    // SESSIONEN KONTROLLERAS VID radering och uppdatering av data;
                    HttpContext.Session.SetString(SessionName, adminlogin.Key);
                    return View("Adminsida");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}


