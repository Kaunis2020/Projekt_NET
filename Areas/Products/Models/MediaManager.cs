using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class MediaManager
    {
        private DatabaseManager dbmanager = null;

        public MediaManager(CompanyDatabasContext _con)
        {
            dbmanager = new DatabaseManager(_con);
        }

        // Array med olika genrer
        readonly string[] medias = {
            "Action",
            "Animerad film",
            "Balettmusik",
            "Barnfilm",
            "Barnlitteratur",
            "Biografi",
            "Chanson",
            "Datateknik, datavetenskap",
            "Datorspel",
            "Drama",
            "Filmmusik",
            "Handarbete",
            "Historisk film",
            "Historisk roman",
            "Julmusik",
            "Katastroffilm",
            "Klassisk musik",
            "Kost, Matlagning",
            "Lingvistik och språk",
            "Ljudbok",
            "Modern musik",
            "Musikfilm, musikal",
            "Pop, populär musik",
            "Science fiction",
            "Skräckfilm",
            "Språkkurs",
            "Övrigt"
        };

        // Array med böcker;
        readonly string[] books = {
            "Barnlitteratur",
            "Biografi",
            "Datateknik, datavetenskap",
            "Drama",
            "Handarbete",
            "Historisk roman",
            "Kost, Matlagning",
            "Lingvistik och språk",
            "Turism",
            "Science fiction",
            "Övrigt"
        };
        // Array med CD;
        readonly string[] cds = {
            "Balettmusik",
            "Barnmusik",
            "Chanson",
            "Datorspel",
            "Filmmusik",
            "Folkmusik",
            "Julmusik",
            "Klassisk musik",
            "Ljudbok",
            "Modern musik",
            "Opera",
            "Orkestermusik",
            "Pop, populär musik",
            "Språkkurs",
            "Övrigt"
        };
        // Array med DVD;
        readonly string[] dvds = {
            "Action",
            "Animerad film",
            "Barnfilm",
            "Biografi",
            "Datorspel",
            "Deckare",
            "Drama",
            "Gangsterfilm",
            "Historisk film",
            "Katastroffilm",
            "Komedi",
            "Musikfilm, musikal",
            "Rysare",
            "Science fiction",
            "Skräckfilm",
            "Thriller",
            "Övrigt"
        };

        public async Task<List<Media>> GetAllMedia()
        {
            return await dbmanager.GetAllMedia();
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await dbmanager.GetAllBooks();
        }

        public async Task<List<Cd>> GetAllCDs()
        {
            return await dbmanager.GetAllCDs();
        }

        public async Task<List<Dvd>> GetAllDVDs()
        {
            return await dbmanager.GetAllDVDs();
        }

        /// <summary>
        /// Långa listor inhämtas med ASYNC;
        /// ASYNC för att utnyttja Threading;
        /// </summary>
        /// <returns></returns>
        public async Task<List<Media>> GetSelectedMedia(int alter, string[] keywords)
        {
            switch (alter)
            {
                case 0: return await GetChosenBooks(keywords);
                case 1: return await GetChosenCDs(keywords);
                case 2: return await GetChosenDVDs(keywords);
                case 3: return await GetChosenMedia(keywords);
                default: return null;
            }
        }
        public async Task<List<Media>> GetChosenMedia(string[] keywords)
        {
            List<Media> medialista = await dbmanager.GetAllMedia();
            List<Media> sendlista = new List<Media>();
            List<Media> templist = new List<Media>();
            medialista.Sort(new SortMedia());
            foreach (string wor in keywords)
            {
                // Jag använder metoden ToLower() för att mata in sökord
                // på vilket sätt som helst: t ex Astrid eller astrid;
                templist = new List<Media>(medialista.Where((Media bo) =>
                bo.Titel.ToLower().Contains(wor.ToLower()) ||
                bo.Genre.ToLower().Contains(wor.ToLower()) ||
                bo.Description.ToLower().Contains(wor.ToLower())
                || bo.Skapare.ToLower().Contains(wor.ToLower())));

                if (templist.Count > 0)
                {
                    foreach (Media ny in templist)
                    {
                        sendlista.Add(ny);
                    }
                }
            }

            if (sendlista.Count > 0)
            {
                /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */

                IEnumerable<Media> filteredList = sendlista.GroupBy(bo => bo.ID_Nr).Select(group => group.First());
                /*  KONVERTERAR IEnumerable till LIST;   */

                List<Media> nylista = new List<Media>(filteredList);
                return nylista;
            }
            else
                return null;
        }

        public async Task<List<Media>> GetChosenBooks(string[] keywords)
        {
            List<Book> bocker = await dbmanager.GetAllBooks();
            List<Media> sendlista = new List<Media>();
            List<Book> templist = new List<Book>();
            bocker.Sort(new SortBooks());
            foreach (string wor in keywords)
            {
                // Jag använder metoden ToLower() för att mata in sökord
                // på vilket sätt som helst: t ex Astrid eller astrid;
                templist = new List<Book>(bocker.Where((Book bo) =>
                bo.Titel.ToLower().Contains(wor.ToLower()) ||
                bo.Genre.ToLower().Contains(wor.ToLower()) ||
                bo.Description.ToLower().Contains(wor.ToLower())
                || bo.Skapare.ToLower().Contains(wor.ToLower())));

                if (templist.Count > 0)
                {
                    foreach (Media ny in templist)
                    {
                        sendlista.Add(ny);
                    }
                }
            }

            if (sendlista.Count > 0)
            {
                /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */

                IEnumerable<Media> filteredList = sendlista.GroupBy(bo => bo.ID_Nr).Select(group => group.First());
                /*  KONVERTERAR IEnumerable till LIST;   */

                List<Media> nylista = new List<Media>(filteredList);
                return nylista;
            }
            else
                return null;
        }

        public async Task<List<Media>> GetChosenCDs(string[] keywords)
        {
            List<Cd> _cds = await dbmanager.GetAllCDs();
            List<Media> sendlista = new List<Media>();
            List<Cd> templist = new List<Cd>();
            _cds.Sort(new SortCDs());
            foreach (string wor in keywords)
            {
                // Jag använder metoden ToLower() för att mata in sökord
                // på vilket sätt som helst: t ex Astrid eller astrid;
                templist = new List<Cd>(_cds.Where((Cd bo) =>
                bo.Titel.ToLower().Contains(wor.ToLower()) ||
                bo.Genre.ToLower().Contains(wor.ToLower()) ||
                bo.Description.ToLower().Contains(wor.ToLower())
                || bo.Skapare.ToLower().Contains(wor.ToLower())));

                if (templist.Count > 0)
                {
                    foreach (Media ny in templist)
                    {
                        sendlista.Add(ny);
                    }
                }
            }
            if (sendlista.Count > 0)
            {
                /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */

                IEnumerable<Media> filteredList = sendlista.GroupBy(bo => bo.ID_Nr).Select(group => group.First());
                /*  KONVERTERAR IEnumerable till LIST;   */

                List<Media> nylista = new List<Media>(filteredList);
                return nylista;
            }
            else
                return null;
        }

        public async Task<List<Media>> GetChosenDVDs(string[] keywords)
        {
            List<Dvd> dvds = await dbmanager.GetAllDVDs();
            List<Media> sendlista = new List<Media>();
            List<Dvd> templist = new List<Dvd>();
            dvds.Sort(new SortDVDs());
            foreach (string wor in keywords)
            {
                // Jag använder metoden ToLower() för att mata in sökord
                // på vilket sätt som helst: t ex Astrid eller astrid;
                templist = new List<Dvd>(dvds.Where((Dvd bo) =>
                bo.Titel.ToLower().Contains(wor.ToLower()) ||
                bo.Genre.ToLower().Contains(wor.ToLower()) ||
                bo.Description.ToLower().Contains(wor.ToLower())
                || bo.Skapare.ToLower().Contains(wor.ToLower())));

                if (templist.Count > 0)
                {
                    foreach (Media ny in templist)
                    {
                        sendlista.Add(ny);
                    }
                }
            }
            if (sendlista.Count > 0)
            {
                /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */

                IEnumerable<Media> filteredList = sendlista.GroupBy(bo => bo.ID_Nr).Select(group => group.First());
                /*  KONVERTERAR IEnumerable till LIST;   */

                List<Media> nylista = new List<Media>(filteredList);
                return nylista;
            }
            else
                return null;
        }

        /// <summary>
        /// Statistik för alla media;
        /// Statistiken visas för ADMIN;
        /// </summary>
        /// <returns></returns>
        public async Task<Object> MediaStatistics()
        {
            List<Media> medialist = await dbmanager.GetAllMedia();
            var counter = 0;
            var deps = new List<Object>();
            foreach (string row in medias)
            {
                foreach (Media me in medialist)
                {
                    if (me.Genre.Equals(row))
                    {
                        counter++;
                    }
                }
                /* Anonyma objekt för JSON; */
                var obj = new { Genre = row, Number = counter };
                deps.Add(obj);
                counter = 0;
            }
            var data = new { length = medialist.Count, genres = deps };
            return data;
        }

        /// <summary>
        /// Statistik för böcker;
        /// Statistiken visas för ADMIN;
        /// Metod är ASYNC för Threading;
        /// </summary>
        /// <returns></returns>
        public async Task<Object> BookStatistics()
        {
            List<Book> boklist = await dbmanager.GetAllBooks();
            var counter = 0;
            var poster = new List<Object>();
            var data = new List<Object>();
            foreach (string row in books)
            {
                foreach (Book me in boklist)
                {
                    if (me.Genre.Equals(row))
                    {
                        counter++;
                    }
                }
                /* Anonyma objekt för JSON; */
                var obj = new { Genre = row, Number = counter };
                poster.Add(obj);
                counter = 0;
            }
            var obj1 = new { Type = "Litteratur", Number = boklist.Count, genres = poster };
            data.Add(obj1);
            return data;
        }

        /// <summary>
        /// Statistik för CD: Audio och Musik;
        /// Statistiken visas för ADMIN;
        /// Metod är ASYNC för Threading;
        /// </summary>
        /// <returns></returns>
        public async Task<Object> CDStatistics()
        {
            List<Cd> cdlist = await dbmanager.GetAllCDs();
            var counter = 0;
            var counter1 = 0;
            var aud = "Audio";
            var mus = "Musik";
            var poster = new List<Object>();
            var poster2 = new List<Object>();
            var data = new List<Object>();
            int totcounter = cdlist.Count(me => me.HuvudTyp.Equals(aud));
            int totcounter2 = cdlist.Count(me => me.HuvudTyp.Equals(mus));
            foreach (string row in cds)
            {
                foreach (Cd me in cdlist)
                {
                    if (me.HuvudTyp.Equals(aud) && me.Genre.Equals(row))
                    {
                        counter++;
                    }
                }
                /* Anonyma objekt för JSON; */
                if (counter > 0)
                {
                    var obj = new { Genre = row, Number = counter };
                    poster.Add(obj);
                    counter = 0;
                }
            }

            var obj0 = new { Type = aud, Number = totcounter, genres = poster };
            data.Add(obj0);

            foreach (string row in cds)
            {
                foreach (Cd me in cdlist)
                {
                    if (me.HuvudTyp.Equals(mus) && me.Genre.Equals(row))
                    {
                        counter1++;
                    }
                }
                /* Anonyma objekt för JSON; */
                if (counter1 > 0)
                {
                    var obj = new { Genre = row, Number = counter1 };
                    poster2.Add(obj);
                    counter1 = 0;
                }
            }
            var obj1 = new { Type = mus, Number = totcounter2, genres = poster2 };
            data.Add(obj1);
            return data;
        }

        /// <summary>
        /// Statistik för DVD: Film och Video;
        /// Statistiken visas för ADMIN;
        /// Metod är ASYNC för Threading;
        /// </summary>
        /// <returns></returns>
        public async Task<Object> DVDStatistics()
        {
            List<Dvd> dvdlist = await dbmanager.GetAllDVDs();
            var counter = 0;
            var counter1 = 0;
            var vid = "Video";
            var film = "Film";
            var poster = new List<Object>();
            var poster2 = new List<Object>();
            var data = new List<Object>();
            int totcounter = dvdlist.Count(me => me.HuvudTyp.Equals(vid));
            int totcounter2 = dvdlist.Count(me => me.HuvudTyp.Equals(film));
            foreach (string row in dvds)
            {
                foreach (Dvd me in dvdlist)
                {
                    if (me.HuvudTyp.Equals(vid) && me.Genre.Equals(row))
                    {
                        counter++;
                    }
                }
                /* Anonyma objekt för JSON; */
                if (counter > 0)
                {
                    var obj = new { Genre = row, Number = counter };
                    poster.Add(obj);
                    counter = 0;
                }
            }

            var obj0 = new { Type = vid, Number = totcounter, genres = poster };
            data.Add(obj0);

            foreach (string row in dvds)
            {
                foreach (Dvd me in dvdlist)
                {
                    if (me.HuvudTyp.Equals(film) && me.Genre.Equals(row))
                    {
                        counter1++;
                    }
                }
                /* Anonyma objekt för JSON; */
                if (counter1 > 0)
                {
                    var obj = new { Genre = row, Number = counter1 };
                    poster2.Add(obj);
                    counter1 = 0;
                }
            }
            var obj1 = new { Type = film, Number = totcounter2, genres = poster2 };
            data.Add(obj1);
            return data;
        }

        public async Task<List<string>> GetWordList()
        {
            List<Media> medialist = await dbmanager.GetAllMedia();
            List<string> ordlista = new List<string>();
            foreach (var item in medialist)
            {
                ordlista.Add(item.ID_Nr);
                ordlista.Add(item.Titel);
                ordlista.Add(item.Skapare);
            }
            ordlista.Sort();
            List<string> nyordlist = new List<string>(ordlista.Distinct());
            ordlista = nyordlist;
            return ordlista;
        }

        public async Task<List<Media>> GetAutoComplete(string keywords)
        {
            List<Media> medialist = await dbmanager.GetAllMedia();
            List<Media> sendlista = new List<Media>();
            List<Media> templist = new List<Media>();
            templist = new List<Media>(medialist.Where((Media bo) =>
            bo.ID_Nr.Equals(keywords) ||
            bo.Titel.Equals(keywords) ||
            bo.Skapare.Equals(keywords)));

            if (templist.Count > 0)
            {
                foreach (Media ny in templist)
                {
                    sendlista.Add(ny);
                }
            }
            if (sendlista.Count > 0)
            {
                /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */

                IEnumerable<Media> filteredList = sendlista.GroupBy(bo => bo.ID_Nr).Select(group => group.First());
                /*  KONVERTERAR IEnumerable till LIST;   */

                List<Media> nylista = new List<Media>(filteredList);

                /* Sorterar efter NUMMERISKT LÖPNUMMER; */
                nylista.Sort(new SortMedia());
                return nylista;
            }
            else
                return null;
        }

        public bool UpdatePrice(string prod_id, int nyttpris)
        {
            return dbmanager.UpdatePrice(prod_id, nyttpris);
        }

        public bool RegisterCD(CD_DTO cddto)
        {
            Cd nytt = new();
            string _id = "CD" + Guid.NewGuid().ToString().Substring(0, 8);
            List<Cd> _cds = dbmanager.GetCds();
            bool finns = false;
            foreach (Cd cd_ in _cds)
            {
                if (cd_.CD_ID.Equals(_id))
                {
                    finns = true;
                    break;
                }
            }
            if (finns)
            {
                _id = "CD" + Guid.NewGuid().ToString().Substring(0, 8);
            }
            string newFileName = "";
            string imagePath;
            // Foto - Frivilligt;
            Stream photo = null;

            if (cddto.Image != null)
            {
                photo = cddto.Image.OpenReadStream();
            }
            if (photo != null)
            {
                string temp = _id;
                string delstr = Path.GetFileName(cddto.Image.FileName);
                string filnamn = delstr.Substring(delstr.IndexOf("."));
                newFileName = temp + filnamn;
                imagePath = @"wwwroot\images\" + newFileName;
                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                    stream.Close();
                }
            }
            DateTime idag = DateTime.Today;
            nytt.CD_ID = _id;
            nytt.Title = cddto.Title;
            nytt.Author = cddto.Author;
            nytt.Kategori = cddto.Kategori;
            nytt.Beskrivning = cddto.Beskrivning;
            nytt.Year = cddto.Year;
            nytt.HuvudTyp = cddto.HuvudTyp;
            nytt.Pris = cddto.Pris;
            nytt.Datum = idag;
            return dbmanager.RegisterCD(nytt);
        }

        public bool RegisterBook(BokDTO dtobok)
        {
            Book nytt = new Book();
            string _id = "BO" + Guid.NewGuid().ToString().Substring(0, 8);
            List<Book> books = dbmanager.GetBooks();
            bool finns = false;
            foreach (Book bo in books)
            {
                if (bo.Bok_ID.Equals(_id))
                {
                    finns = true;
                    break;
                }
            }
            if (finns)
            {
                _id = "BO" + Guid.NewGuid().ToString().Substring(0, 8);
            }
            string newFileName = "";
            string imagePath;
            // Foto - Frivilligt;
            Stream photo = null;

            if (dtobok.Image != null)
            {
                photo = dtobok.Image.OpenReadStream();
            }

            if (photo != null)
            {
                string temp = _id;
                string delstr = Path.GetFileName(dtobok.Image.FileName);
                string filnamn = delstr.Substring(delstr.IndexOf("."));
                newFileName = temp + filnamn;
                imagePath = @"wwwroot\images\" + newFileName;
                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                    stream.Close();
                }
            }
            DateTime idag = DateTime.Today;
            nytt.Bok_ID = _id;
            nytt.Title = dtobok.Title;
            nytt.Author = dtobok.Author;
            nytt.Kategori = dtobok.Kategori;
            nytt.Beskrivning = dtobok.Beskrivning;
            nytt.Year = dtobok.Year;
            nytt.HuvudTyp = dtobok.HuvudTyp;
            nytt.Pris = dtobok.Pris;
            nytt.Datum = idag;
            return dbmanager.RegisterBook(nytt);
        }

        public bool RegisterDVD(DVD_DTO dvddto)
        {
            Dvd nytt = new Dvd();
            string _id = "DVD" + Guid.NewGuid().ToString().Substring(0, 8);
            List<Dvd> dvds = dbmanager.GetDvds();
            bool finns = false;
            foreach (Dvd dv_ in dvds)
            {
                if (dv_.DVD_ID.Equals(_id))
                {
                    finns = true;
                    break;
                }
            }
            if (finns)
            {
                _id = "DVD" + Guid.NewGuid().ToString().Substring(0, 8);
            }
            string newFileName = "";
            string imagePath;
            // Foto - Frivilligt;
            Stream photo = null;

            if (dvddto.Image != null)
            {
                photo = dvddto.Image.OpenReadStream();
            }

            if (photo != null)
            {
                string temp = _id;
                string delstr = Path.GetFileName(dvddto.Image.FileName);
                string filnamn = delstr.Substring(delstr.IndexOf("."));
                newFileName = temp + filnamn;
                imagePath = @"wwwroot\images\" + newFileName;
                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                    stream.Close();
                }
            }
            DateTime idag = DateTime.Today;
            nytt.DVD_ID = _id;
            nytt.Title = dvddto.Title;
            nytt.Author = dvddto.Author;
            nytt.Kategori = dvddto.Kategori;
            nytt.Beskrivning = dvddto.Beskrivning;
            nytt.Year = dvddto.Year;
            nytt.HuvudTyp = dvddto.HuvudTyp;
            nytt.Pris = dvddto.Pris;
            nytt.Datum = idag;
            return dbmanager.RegisterDVD(nytt);
        }

        public bool DeleteProduct(string prodid)
        {
            return dbmanager.DeleteProduct(prodid);
        }
    }
}
