using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductController : Controller
    {
        const string SessionName = "Admin";
        private MediaManager manager = null;

        public ProductController(CompanyDatabasContext _con)
        {
            manager = new MediaManager(_con);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/AllTheMedia")]
        public async Task<IActionResult> AllTheMedia(string sortOrder)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["KatSortParm"] = sortOrder == "Kategori" ? "kat_desc" : "Kategori";
            ViewData["TitSortParm"] = sortOrder == "Titel" ? "tit_desc" : "Titel";
            List<Media> medialista = await manager.GetAllMedia();
            switch (sortOrder)
            {
                case "Kategori":
                    medialista = medialista.OrderBy(s => s.Genre).ToList();
                    break;
                case "kat_desc":
                    medialista = medialista.OrderByDescending(s => s.Genre).ToList();
                    break;
                case "name_desc":
                    medialista = medialista.OrderByDescending(s => s.Skapare).ToList();
                    break;
                case "Titel":
                    medialista = medialista.OrderBy(s => s.Titel).ToList();
                    break;
                case "tit_desc":
                    medialista = medialista.OrderByDescending(s => s.Titel).ToList();
                    break;
                default:
                    medialista = medialista.OrderBy(s => s.Skapare).ToList();
                    break;
            }
            return PartialView("AllTheMedia", medialista);
        }

        [HttpGet]
        [Route("Admin/SearchMedia")]
        public IActionResult SearchMedia()
        {
            return View("SearchMedia");
        }

        [HttpPost]
        [Route("Admin/Glossary")]
        public async Task<JsonResult> Glossary()
        {
            List<string> ordlista = await manager.GetWordList();
            return Json(ordlista);
        }

        // Accepterar enbart POST;
        [HttpPost]
        [Route("Admin/Autocomplete")]
        public async Task<JsonResult> Autocomplete(string word)
        {
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrWhiteSpace(word))
            {
                List<Media> lista = await manager.GetAutoComplete(word);
                if (lista != null)
                    return Json(lista);
                else return Json(null);
            }
            else
                return Json(null);
        }

        [HttpPost]
        [Route("Admin/SearchWord")]
        public async Task<JsonResult> SearchWord(int choice, string word)
        {
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrWhiteSpace(word))
            {
                /* Splittrar med tomma mellanrum;  */
                string[] keywords = word.Split(null);

                List<Media> lista = await manager.GetSelectedMedia(choice, keywords);
                if (lista != null)
                    return Json(lista);
                else return Json(null);
            }
            else
                return Json(null);
        }

        [HttpPost]
        [Route("Admin/UpdateProduct")]
        public string UpdateProduct(string prodid, string price)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return "NOT AUTHORIZED";
            }
            int nypris;
            if (!int.TryParse(price, out nypris))
            {
                return "FEL";
            }
            else
            {
                string result;
                if (manager.UpdatePrice(prodid, nypris))
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

        [HttpPost]
        [Route("Admin/DeleteProduct")]
        public string DeleteProduct(string prodid)
        {
            // Kollar om en giltig temporär session är satt;
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                // Lämnar metoden med "Icke-auktoriserad";
                return "NOT AUTHORIZED";
            }

            if (string.IsNullOrEmpty(prodid) || string.IsNullOrWhiteSpace(prodid))
                return "FEL";
            string result;
            if (manager.DeleteProduct(prodid))
            {
                result = "OK";
            }
            else
            {
                result = "FEL";
            }
            return result;
        }

        [HttpGet]
        [Route("Admin/AddNewMedia")]
        public IActionResult AddNewMedia()
        {
            return View("AddNewMedia");
        }

        [HttpPost]
        [Route("Admin/MediaForm")]
        public IActionResult MediaForm()
        {
            string typ = Request.Form["mediatyp"];
            if (typ.Equals("bok"))
                return PartialView("BokForm");
            else if (typ.Equals("cd"))
                return PartialView("CDForm");
            else if (typ.Equals("dvd"))
                return PartialView("DVDForm");
            else
                return RedirectToAction("Adminsida", "Admin");
        }

        [HttpPost]
        [Route("Admin/RegisterCD")]
        public IActionResult RegisterCD(CD_DTO cddto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tomma fält är inte tillåtna!");
                return PartialView("CDForm", cddto);
            }
            if (ModelState.IsValid)
            {
                if (manager.RegisterCD(cddto))
                {
                    ViewBag.Information = "CD:n har sparats i databasen";
                    ModelState.Clear();
                    return PartialView("CDForm");
                }
                else
                {
                    ViewBag.Information = "Kunde inte spara CD:n i databasen";
                    return PartialView("CDForm", cddto);
                }
            }
            else
            {
                ViewBag.Information = "Tomma eller felaktiga fält finns!";
                return PartialView("CDForm", cddto);
            }
        }

        [HttpPost]
        [Route("Admin/RegisterBook")]
        public IActionResult RegisterBook(BokDTO dtobok)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tomma fält är inte tillåtna!");
                return PartialView("BokForm", dtobok);
            }
            if (ModelState.IsValid)
            {
                if (manager.RegisterBook(dtobok))
                {
                    ViewBag.Information = "Boken har sparats i databasen";
                    ModelState.Clear();
                    return PartialView("BokForm");
                }
                else
                {
                    ViewBag.Information = "Kunde inte spara boken i databasen";
                    return PartialView("BokForm", dtobok);
                }
            }
            else
            {
                ViewBag.Information = "Tomma eller felaktiga fält finns!";
                return PartialView("BokForm", dtobok);
            }
        }

        [HttpPost]
        [Route("Admin/RegisterDVD")]
        public IActionResult RegisterDVD(DVD_DTO dvddto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tomma fält är inte tillåtna!");
                return PartialView("DVDForm", dvddto);
            }
            if (ModelState.IsValid)
            {
                if (manager.RegisterDVD(dvddto))
                {
                    ViewBag.Information = "DVD:n har sparats i databasen";
                    ModelState.Clear();
                    return PartialView("DVDForm");
                }
                else
                {
                    ViewBag.Information = "Kunde inte spara DVD:n i databasen";
                    return PartialView("DVDForm", dvddto);
                }
            }
            else
            {
                ViewBag.Information = "Tomma eller felaktiga fält finns!";
                return PartialView("DVDForm", dvddto);
            }
        }
    }
}
