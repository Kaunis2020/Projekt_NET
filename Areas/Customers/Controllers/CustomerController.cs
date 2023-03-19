using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    [Area("Customers")]
    public class CustomerController : Controller
    {
        const string SessionName = "Admin";
        private CustomerManager manager = null;

        public CustomerController(CompanyDatabasContext _con)
        {
            manager = new CustomerManager(_con);
            manager.FillList();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/Vocabulary")]
        public JsonResult Vocabulary()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return Json("NOT AUTHORIZED");
            }

            List<string> ordlista = manager.GetWordList();
            return Json(ordlista);
        }

        [HttpGet]
        [Route("Admin/AllCustomers")]
        public async Task<ActionResult> AllCustomers()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Customer> kundlista = await manager.GetCustomers();
            if (kundlista == null || kundlista.Count == 0)
            {
                ViewBag.Information = "Inga kunder finns i databasen!";
                return PartialView("Adminsida", ViewBag.Information);
            }
            else
            {
                // Sorterar NUMMERISKT;
                kundlista.Sort(new SortCustomer());
                return PartialView("CustomerList", kundlista);
            }
        }

        [HttpGet]
        [Route("Admin/CustomerOnLoad")]
        public async Task<JsonResult> CustomerOnLoad()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return Json("NOT AUTHORIZED");
            }
            List<Customer> lista = await manager.GetCustomers();
            lista.Sort(new SortCustomer());
            if (lista != null)
            {
                return Json(lista);
            }
            else
                return Json("null");
        }

        [HttpGet]
        [Route("Admin/GetCustomerList")]
        public FileResult GetCustomerList()
        {
            List<Customer> customers = manager.GetAllCustomers();
            if (customers != null && customers.Count > 0)
            {
                // Sorterar NUMMERISKT enligt löpnummer;
                customers.Sort(new SortCustomer());
                if (FileHandler.SaveCustomers(customers))
                {
                    byte[] fileBytes = FileHandler.GetFile("App_Data/json/customers.json");
                    string contentType = "application/json";
                    return File(fileBytes, contentType, "customers.json");
                }
                else
                    return null;
            }
            return null;
        }

        [HttpGet]
        [Route("Admin/NewCustomer")]
        public IActionResult NewCustomer()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) != null)
                return PartialView("NewCustomer");
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("Admin/CustomerForm")]
        public ActionResult CustomerForm()
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string typ = Request.Form["kundtyp"];
            if (typ.Equals("firma"))
                return PartialView("FirmForm");
            else if (typ.Equals("privat"))
                return PartialView("PrivateForm");
            else
                return RedirectToAction("Index", "Home");
        }

        // Accepterar enbart POST;
        [HttpGet]
        [Route("Admin/AutoComplete/{word}")]
        public JsonResult AutoComplete(string word)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return Json("NOT AUTHORIZED");
            }
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrWhiteSpace(word))
            {
                List<Customer> lista = manager.GetAutoComplete(word);
                if (lista != null)
                    return Json(lista);
                else return null;
            }
            else
                return null;
        }

        [HttpGet]
        [Route("Admin/SearchCustomer")]
        public ActionResult SearchCustomer()
        {
            if (HttpContext.Session.GetString(SessionName) != null)
                return PartialView("SearchCustomer");
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("Admin/AnyCustomer/{word}")]
        public JsonResult AnyCustomer(string word)
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

                List<Customer> lista = manager.GetChosenCustomer(keywords);
                if (lista != null)
                    return Json(lista);
                else return Json(null);
            }
            else
                return Json(null);
        }
        [HttpGet]
        [Route("Admin/AllMessages")]
        public async Task<ActionResult> AllMessages()
        {
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Message> messages = await manager.AllMessages();
                return PartialView("AllMessages", messages);
            }
        }

        [HttpGet]
        [Route("Admin/Delete/{id:int}")]
        public IActionResult DeleteMessage(int id)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (manager.DeleteMessage(id))
            {
                return Ok(HttpStatusCode.OK);
            }
            else
            {
                return BadRequest((HttpStatusCode.BadRequest, "TEKNISKT FEL"));
            }
        }

        [HttpPost]
        [Route("Admin/CustNewAddr")]
        public string CustNewAddr(string kundid, string gata, string ort, string land)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return "NOT AUTHORIZED";
            }

            if (string.IsNullOrEmpty(kundid) || string.IsNullOrWhiteSpace(kundid)
                || string.IsNullOrEmpty(gata) || string.IsNullOrWhiteSpace(gata)
                || string.IsNullOrEmpty(ort) || string.IsNullOrWhiteSpace(ort)
                || string.IsNullOrEmpty(land) || string.IsNullOrWhiteSpace(land))
            {
                return "FEL";
            }
            string result;
            if (manager.ChangeAddress(kundid, gata, ort, land))
            {
                result = "OK";
            }
            else
            {
                result = "FEL";
            }
            return result;
        }

        [HttpPost]
        [Route("Admin/RegisterFirm")]
        public IActionResult RegisterFirm(FirmCustomer fikund)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                string newFileName = "";
                string imagePath;
                // Foto - Frivilligt;
                Stream photo = null;
                if (fikund.Image != null)
                {
                    photo = fikund.Image.OpenReadStream();
                }
                List<Customer> kundlista = manager.GetAllCustomers();
                Customer dbkund = new Customer();

                string auto_id = Guid.NewGuid().ToString();
                // ID-nummer skapas med FI + nummer (FI = Företagskund);
                string kund_id = fikund.Prefix + auto_id.Substring(0, 8);
                bool idfinns = false;

                if (kundlista != null && kundlista.Count > 0)
                {
                    foreach (Customer ku in kundlista)
                    {
                        if (kund_id.Equals(ku.Kund_ID))
                        {
                            idfinns = true;
                            break;
                        }
                    }

                    if (idfinns == true)
                    {
                        auto_id = Guid.NewGuid().ToString();
                        kund_id = fikund.Prefix + auto_id.Substring(0, 8);
                    }
                }
                if (photo != null)
                {
                    string temp = auto_id.Substring(0, 10);
                    string delstr = Path.GetFileName(fikund.Image.FileName);
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

                dbkund.Kund_ID = kund_id;
                dbkund.KundTyp = fikund.KundTyp;
                // Företagsnamn OBLIGATORISKT;
                dbkund.FirmaNamn = fikund.FirmaNamn;
                dbkund.KontaktPerson = fikund.KontaktPerson;
                dbkund.GatuAdress = fikund.GatuAdress;
                dbkund.Ort = fikund.Ort;
                dbkund.Land = fikund.Land;
                dbkund.Bild = newFileName;
                if (manager.InsertNewCustomer(dbkund))
                {
                    ViewBag.Information = "Kunden " + dbkund.KontaktPerson + " har sparats";
                    /* Model.Clear tömmer inmatningsrutorna 
                     * för nästa inmatning av en annan person; */
                    ModelState.Clear();
                    return PartialView("NewCustomer");
                }
                else
                {
                    ViewBag.Information = "Tekniskt fel. Försök igen.";
                    return PartialView("FirmForm");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tomma fält är inte tillåtna!");
                ViewBag.Information = "Tomma fält är inte tillåtna!";
                return PartialView("FirmForm", fikund);
            }
        }

        [HttpPost]
        [Route("Admin/RegisterPrivate")]
        public IActionResult RegisterPrivate(PrivateCustomer kund)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                string newFileName = "";
                string imagePath;
                // Foto - Frivilligt;
                Stream photo = null;
                if (kund.Image != null)
                {
                    photo = kund.Image.OpenReadStream();
                }
                List<Customer> kundlista = manager.GetAllCustomers();
                Customer dbkund = new Customer();

                string auto_id = Guid.NewGuid().ToString();
                // ID-nummer skapas med PR + nummer (PR = privat kund);
                string kund_id = kund.Prefix + auto_id.Substring(0, 8);
                bool idfinns = false;

                if (kundlista != null && kundlista.Count > 0)
                {
                    foreach (Customer ku in kundlista)
                    {
                        if (kund_id.Equals(ku.Kund_ID))
                        {
                            idfinns = true;
                            break;
                        }
                    }
                    if (idfinns == true)
                    {
                        auto_id = Guid.NewGuid().ToString();
                        kund_id = kund.Prefix + auto_id.Substring(0, 8);
                    }
                }
                if (photo != null)
                {
                    string temp = auto_id.Substring(0, 10);
                    string delstr = Path.GetFileName(kund.Image.FileName);
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
                dbkund.Kund_ID = kund_id;
                dbkund.KundTyp = kund.KundTyp;
                // För PRIVATPERSONER FirmaNamn == '';
                dbkund.FirmaNamn = " ";
                dbkund.KontaktPerson = kund.PersonNamn;
                dbkund.GatuAdress = kund.GatuAdress;
                dbkund.Ort = kund.Ort;
                dbkund.Land = kund.Land;
                dbkund.Bild = newFileName;
                if (manager.InsertNewCustomer(dbkund))
                {
                    ViewBag.Information = "Kunden " + dbkund.KontaktPerson + " har sparats";
                    /* Model.Clear tömmer inmatningsrutorna 
                     * för nästa inmatning av en annan person; */
                    ModelState.Clear();
                    return PartialView("NewCustomer");
                }
                else
                {
                    ViewBag.Information = " Tekniskt fel. Försök igen.";
                    return PartialView("PrivateForm", kund);
                }
            }
            else
            {
                ModelState.AddModelError("", "Tomma fält är inte tillåtna!");
                ViewBag.Information = "Tomma fält är inte tillåtna!";
                return PartialView("PrivateForm", kund);
            }
        }

        [HttpPost]
        [Route("Admin/DeleteCustomer")]
        public string DeleteCustomer(string kundid)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return "NOT AUTHORIZED";
            }
            if (string.IsNullOrEmpty(kundid) || string.IsNullOrWhiteSpace(kundid))
                return "FEL";
            string result;
            if (manager.DeleteCustomer(kundid))
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
