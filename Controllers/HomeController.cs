using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomerManager manager = null;
        private readonly EmployeeManager empmanager = null;
        public HomeController(ILogger<HomeController> logger, CompanyDatabasContext con)
        {
            _logger = logger;
            manager = new CustomerManager(con);
            empmanager = new EmployeeManager(con);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            string path = Directory.GetCurrentDirectory();
            string fullpath = path + @"\App_Data\binaries\chiefboard.bin";
            List<Member> members = BinSerializerUtility.BinaryFileDeserialize<List<Member>>(fullpath);
            return View(members);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewBag.Message = "På denna sida kan Du skicka ett brev till oss";
            return View();
        }

        [HttpPost]
        public IActionResult PostMessage(Message medd)
        {
            if (ModelState.IsValid)
            {
                if (manager.InsertMessage(medd))
                {
                    /* Model.Clear tömmer inmatningsrutorna 
                     * för nästa inmatning av en annan person; */
                    ModelState.Clear();
                    ViewBag.Message = "Tack så mycket för Ditt brev. Vi besvarar det inom tre arbetsdagar.";
                    return PartialView("Contact");
                }
                else
                {
                    ViewBag.Message = "Ett tekniskt fel har inträffat! Försök senare!!!";
                    return PartialView("Contact", medd);
                }
            }
            else
            {
                ModelState.AddModelError("", "Tomma fält är inte tillåtna");
                return PartialView("Contact", medd);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> allOurEmployees(string sortOrder)
        {
            ViewData["NumSortParm"] = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            ViewData["AvdSortParm"] = sortOrder == "Avdelning" ? "avd_desc" : "Avdelning";
            List<Employee> medarbetare = await empmanager.GetEmployees();
            if (medarbetare == null || medarbetare.Count == 0)
            {
                ViewBag.Information = "Inga medarbetare finns i databasen!";
                return PartialView("allOurEmployees", null);
            }
            else
            {
                switch (sortOrder)
                {
                    case "Avdelning":
                        medarbetare = medarbetare.OrderBy(med => med.Department).ToList();
                        break;
                    case "avd_desc":
                        medarbetare = medarbetare.OrderByDescending(med => med.Department).ToList();
                        break;
                    case "num_desc":
                        medarbetare = medarbetare.OrderByDescending(med => med.Nummer).ToList();
                        break;
                    default: // Sorterar NUMMERISKT;
                        medarbetare.Sort(new SortEmpl());
                        break;
                }
                return PartialView("allOurEmployees", medarbetare);
            }
        }
    }
}
