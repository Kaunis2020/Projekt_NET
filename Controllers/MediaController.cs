using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt.Models;
using Microsoft.EntityFrameworkCore;

namespace Projekt.Controllers
{
    public class MediaController : Controller
    {
        private MediaManager mediamanager = null;

        public MediaController(CompanyDatabasContext _con)
        {
            mediamanager = new MediaManager(_con);
        }

        /// <summary>
        /// GET: Media, Returnerar Alla Media i sorterad form
        /// Man kan sortera genom att klicka på kolumnen
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <returns></returns>        
        [HttpGet]
        [Route("Media/OurMedia")]
        public async Task<ActionResult> OurMedia(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["KatSortParm"] = sortOrder == "Kategori" ? "kat_desc" : "Kategori";
            ViewData["TitSortParm"] = sortOrder == "Titel" ? "tit_desc" : "Titel";
            var medialist = await mediamanager.GetAllMedia();
            switch (sortOrder)
            {
                case "Kategori":
                    medialist = medialist.OrderBy(s => s.Genre).ToList();
                    break;
                case "kat_desc":
                    medialist = medialist.OrderByDescending(s => s.Genre).ToList();
                    break;
                case "name_desc":
                    medialist = medialist.OrderByDescending(s => s.Skapare).ToList();
                    break;
                case "Titel":
                    medialist = medialist.OrderBy(s => s.Titel).ToList();
                    break;
                case "tit_desc":
                    medialist = medialist.OrderByDescending(s => s.Titel).ToList();
                    break;
                default:
                    medialist = medialist.OrderBy(s => s.Skapare).ToList();
                    break;
            }
            return PartialView("OurMedia", medialist);
        }

        /// <summary>
        /// GET: Media: Returnerar PAGINERING;
        /// Visar 10 element per varje sida;
        /// Bekvämt att använda vid långa listor;
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Media/OurPagMedia")]
        public async Task<ActionResult> OurPagMedia(int? pageNumber)
        {
            // Innehåller 10 element per varje sida;
            int pageSize = 10;
            var medialist = await mediamanager.GetAllMedia();
            var nylist = medialist.AsQueryable();
            var pagineted = PaginatedList<Media>.CreateAsync(nylist, pageNumber ?? 1, pageSize);
            return View("OurPagMedia", pagineted);
        }

        [HttpGet]
        [Route("Media/OurBooks")]
        public async Task<ActionResult> OurBooks()
        {
            List<Book> books = await mediamanager.GetAllBooks();
            books.Sort(new SortBooks());
            return PartialView("OurBooks", books);
        }

        [HttpGet]
        [Route("Media/OurAudio")]
        public async Task<ActionResult> OurAudio()
        {
            List<Cd> cds = await mediamanager.GetAllCDs();
            cds.Sort(new SortCDs());
            return PartialView("OurAudio", cds);
        }

        [HttpGet]
        [Route("Media/OurVideo")]
        public async Task<ActionResult> OurVideo()
        {
            List<Dvd> dvds = await mediamanager.GetAllDVDs();
            dvds.Sort(new SortDVDs());
            return PartialView("OurVideo", dvds);
        }

        [HttpGet]
        [Route("Media/MediaOnLoad/{alter:int}")]
        public async Task<JsonResult> MediaOnLoad(int alter)
        {
            var media = new List<Media>();
            switch (alter)
            {
                case 0:
                    List<Book> books = await mediamanager.GetAllBooks();
                    books.Sort(new SortBooks());                  
                    media.AddRange(books);
                    return Json(media);
                case 1:
                    List<Cd> cds = await mediamanager.GetAllCDs();
                    cds.Sort(new SortCDs());
                    media.AddRange(cds);
                    return Json(media);
                case 2:
                    List<Dvd> dvds = await mediamanager.GetAllDVDs();
                    dvds.Sort(new SortDVDs());
                    media.AddRange(dvds);
                    return Json(media);
                case 3:
                    media = await mediamanager.GetAllMedia();
                    media.Sort(new SortMedia());
                    return Json(media);
                default: return Json(null);
            }
        }

        [HttpPost]
        [Route("Media/SearchWord")]
        public async Task<JsonResult> SearchWord(int choice, string word)
        {
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrWhiteSpace(word))
            {
                /* Splittrar med tomma mellanrum;  */
                string[] keywords = word.Split(null);

                List<Media> lista = await mediamanager.GetSelectedMedia(choice, keywords);
                if (lista != null)
                    return Json(lista);
                else return Json(null);
            }
            else
                return Json(null);
        }

        [HttpGet]
        [Route("Media/SearchMedia")]
        public IActionResult SearchMedia()
        {
            return View("SearchMedia");
        }

        [HttpPost]
        [Route("Media/Glossary")]
        public async Task<JsonResult> Glossary()
        {
            List<string> ordlista = await mediamanager.GetWordList();
            return Json(ordlista);
        }

        // Accepterar enbart POST;
        [HttpPost]
        [Route("Media/Autocomplete")]
        public async Task<JsonResult> Autocomplete(string word)
        {
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrWhiteSpace(word))
            {
                List<Media> lista = await mediamanager.GetAutoComplete(word);
                if (lista != null)
                    return Json(lista);
                else return Json(null);
            }
            else
                return Json(null);
        }
    }
}
