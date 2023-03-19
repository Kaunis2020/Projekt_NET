using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    [NotMapped]
    public abstract class Media
    {
        private const string path = "wwwroot/images/";
        private const string soundpath = "wwwroot/audio/";

        public abstract int Number { get; }

        public abstract string Typ { get; }

        public abstract string ID_Nr { get; }

        public abstract string Titel { get; }

        public abstract string Skapare { get; }

        public abstract string Genre { get; }

        public abstract string Description { get; }

        public abstract string ProdYear { get; }

        public abstract string MediaTyp { get; }

        public abstract int Price { get; }

        public abstract DateTime LogDate { get; }

        public abstract int Moms { get; }

        public abstract string Picture
        {
            get;
        }

        public abstract string Sound
        {
            get;
        }

        /// <summary>
        /// Returnerar Bild-path. Om bilden INTE
        /// finns, returnerar en DEFAULTBILD med
        /// texten "Bild saknas";
        /// </summary>
        /// <param name="id_nr">ID-nummer</param>
        /// <returns>Bild-path som string</returns>
        public static bool ImagePath(string id_nr)
        {
            string fil = path + id_nr + ".jpg";
            string image = Path.Combine(Directory.GetCurrentDirectory(), fil);
            // Om bildfilen finns och om filen inte är tom
            // returnerar bildnamn, annars returnerar default;
            if (File.Exists(image) && !IsEmpty(image))
                return true;
            else
                return false;
        }

        public static bool SoundPath(string id_nr)
        {
            string fil = soundpath + id_nr + ".mp3";
            string cdljud = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fil);
            if (File.Exists(cdljud) && !IsEmpty(cdljud))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Kollar om den angivna filen är tom
        /// </summary>
        /// <param name="path">Filens path</param>
        /// <returns>bool</returns>
        private static bool IsEmpty(string path)
        {
            return new FileInfo(path).Length == 0;
        }
    }
}
