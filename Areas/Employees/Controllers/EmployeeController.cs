using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.Models;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    [Area("Employees")]
    public class EmployeeController : Controller
    {
        const string SessionName = "Admin";
        private EmployeeManager emplmanager = null;

        public EmployeeController(CompanyDatabasContext _con)
        {
            emplmanager = new EmployeeManager(_con);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/Dictionary")]
        public JsonResult Dictionary()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return Json("NOT AUTHORIZED");
            }

            List<string> ordlista = emplmanager.GetWordList();
            return Json(ordlista);
        }

        [HttpGet]
        [Route("Admin/EmplOnLoad")]
        public async Task<JsonResult> EmplOnLoad()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return Json("NOT AUTHORIZED");
            }
            List<Employee> lista = await emplmanager.GetEmployees();
            lista.Sort(new SortEmpl());
            if (lista != null)
            {
                return Json(lista);
            }
            else
                return Json("null");
        }

        [HttpPost]
        [Route("Admin/NewDepartment")]
        public string NewDepartment(string newdepart)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return "NOT AUTHORIZED";
            }
            else
                return emplmanager.NewDepartment(newdepart);
        }

        [HttpGet]
        [Route("Admin/SearchEmpl")]
        public IActionResult SearchEmpl()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) != null)
                return PartialView("SearchEmpl");
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("Admin/AutoEmployee/{word}")]
        public JsonResult AutoEmployee(string word)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return Json("NOT AUTHORIZED");
            }
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrWhiteSpace(word))
            {
                List<Employee> lista = emplmanager.GetAutoComplete(word);
                if (lista != null)
                    return Json(lista);
                else return Json(null);
            }
            else
                return Json(null);
        }

        [HttpGet]
        [Route("Admin/SearchAnyEmpl/{word}")]
        public JsonResult SearchAnyEmpl(string word)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return Json("NOT AUTHORIZED");
            }
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrWhiteSpace(word))
            {
                /* Splittrar med tomma mellanrum;  */
                string[] keywords = word.Split(null);
                List<Employee> lista = emplmanager.GetChosenEmpl(keywords);
                if (lista != null)
                    return Json(lista);
                else return Json(null);
            }
            else
                return Json(null);
        }

        [HttpGet]
        [Route("Admin/ListEmployees")]
        public async Task<ActionResult> ListEmployees(string sortOrder)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["NumSortParm"] = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            ViewData["AvdSortParm"] = sortOrder == "Avdelning" ? "avd_desc" : "Avdelning";
            List<Employee> medarbetare = await emplmanager.GetEmployees();

            if (medarbetare == null || medarbetare.Count == 0)
            {
                ViewBag.Information = "Inga medarbetare finns i databasen!";
                return PartialView("Adminsida", ViewBag.Information);
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
                return PartialView("Employeelist", medarbetare);
            }
        }

        [HttpGet]
        [Route("Admin/GetEmploList")]
        public FileResult GetEmploList()
        {
            List<Employee> medarbetare = emplmanager.GetAllEmployees();
            if (medarbetare != null && medarbetare.Count > 0)
            {
                // Sorterar NUMMERISKT enligt löpnummer;
                medarbetare.Sort(new SortEmpl());
                if (FileHandler.SaveEmployees(medarbetare))
                {
                    byte[] fileBytes = FileHandler.GetFile("App_Data/json/employees.json");
                    string contentType = "application/json";
                    return File(fileBytes, contentType, "employees.json");
                }
                else
                    return null;
            }
            return null;
        }

        [HttpGet]
        [Route("Admin/NewEmployee")]
        public IActionResult NewEmployee()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) != null)
                return PartialView("NewEmployee");
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Admin/RegisterEmpl")]
        public IActionResult RegisterEmpl(Employee medarb)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var newFileName = "";
                string imagePath;
                string prefix = "EM";
                // Foto - Frivilligt;
                Stream photo = null;
                if (medarb.Image != null)
                {
                    photo = medarb.Image.OpenReadStream();
                }
                List<Employee> medarbetare = emplmanager.GetAllEmployees();
                Employee nyemp = new Employee();

                string auto_id = Guid.NewGuid().ToString();
                string this_id = prefix + auto_id.Substring(0, 8);
                bool idfinns = false;

                if (medarbetare != null && medarbetare.Count > 0)
                {
                    foreach (Employee emp in medarbetare)
                    {
                        if (this_id.Equals(emp.ID))
                        {
                            idfinns = true;
                            break;
                        }
                    }
                    if (idfinns == true)
                    {
                        auto_id = Guid.NewGuid().ToString();
                        this_id = prefix + auto_id.Substring(0, 8);
                    }
                }

                if (photo != null)
                {
                    string temp = auto_id.Substring(0, 10);
                    string delstr = Path.GetFileName(medarb.Image.FileName);
                    string filnamn = delstr.Substring(delstr.IndexOf(".") - 1);
                    newFileName = temp + filnamn;
                    imagePath = @"wwwroot\images\" + newFileName;
                    using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                        stream.Close();
                    }
                }
                if (photo == null)
                {
                    newFileName = "photo.png";
                }
                nyemp.ID = this_id;
                nyemp.First_name = medarb.First_name;
                nyemp.Last_name = medarb.Last_name;
                nyemp.Email = medarb.Email;
                nyemp.Phone = medarb.Phone;
                nyemp.Department = medarb.Department;
                nyemp.Bild = newFileName;
                if (emplmanager.InsertNewEmployee(nyemp))
                {
                    ViewBag.Information = nyemp.First_name + " " + nyemp.Last_name + " har sparats";
                    /* Model.Clear tömmer inmatningsrutorna 
                     * för nästa inmatning av en annan person; */
                    ModelState.Clear();
                    return PartialView("NewEmployee");
                }
                else
                {
                    ViewBag.Information = "Tekniskt fel. Försök igen.";
                    return PartialView("NewEmployee");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tomma fält är inte tillåtna!");
                ViewBag.Information = "Tomma fält är inte tillåtna!";
                return PartialView("NewEmployee", medarb);
            }
        }

        [HttpGet]
        [Route("Admin/SendEmplForm/{emplid}")]
        public IActionResult SendEmplForm(string emplid)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Employee temp = emplmanager.GetEmployee(emplid);
            return PartialView("ChangeForm", temp);
        }

        [HttpPost]
        [Route("Admin/ChangeEmplData")]
        public IActionResult ChangeEmplData(Employee empl)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                @ViewBag.Message = "Tomma fält är inte tillåtna!";
                //  return "Tomma fält är inte tillåtna!";
                return PartialView("ChangeForm", empl);
            }
            else
            {
                if (emplmanager.SaveEmplChanges(empl))
                {
                    ModelState.Clear();
                    @ViewBag.Message = "OK -- Ändringarna har sparats!";
                    //   return "OK -- Ändringarna har sparats!";
                    return PartialView("EditedEmpl", empl);
                }
                else
                {
                    @ViewBag.Message = "FEL -- Tekniskt fel. Försök igen!";
                    // return "FEL -- Tekniskt fel. Försök igen!";
                    return RedirectToAction("Index", "Admin");
                }
            }
        }

        [HttpPost]
        [Route("Admin/DeleteEmpl")]
        public string DeleteEmpl(string emploid)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return "NOT AUTHORIZED";
            }
            if (string.IsNullOrEmpty(emploid) || string.IsNullOrWhiteSpace(emploid))
                return "FEL";
            string result;
            if (emplmanager.DeleteEmployee(emploid))
            {
                result = "OK";
            }
            else
            {
                result = "FEL";
            }
            return result;
        }
    }
}


