using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Projekt.Models
{
    public partial class Cd : Media
    {
        private string picture = "def000.jpg";
        private string cdsound = "error.mp3";
        private int moms = 0; // SVENSK moms;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nummer { get; set; }
        public string CD_ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Kategori { get; set; }
        public string Beskrivning { get; set; }
        public string Year { get; set; }
        public string HuvudTyp { get; set; }
        public int Pris { get; set; }
        public DateTime Datum { get; set; }

        public override string Typ { get { return "CD"; } }
        public override int Number { get { return Nummer; } }
        public override string ID_Nr { get { return CD_ID; } }
        public override string Titel { get { return Title; } }
        public override string Skapare { get { return Author; } }
        public override string Genre { get { return Kategori; } }
        public override string Description { get { return Beskrivning; } }
        public override string ProdYear { get { return Year; } }
        public override string MediaTyp { get { return HuvudTyp; } }
        public override int Price { get { return Pris; } }
        public override DateTime LogDate { get { return Datum; } }

        public override string Picture
        {
            get
            {
                if (Media.ImagePath(this.ID_Nr))
                {
                    picture = this.ID_Nr + ".jpg";
                }
                return picture;
            }
        }

        public override string Sound
        {
            get
            {
                if (Media.SoundPath(this.ID_Nr))
                {
                    cdsound = this.ID_Nr + ".mp3";
                }
                return cdsound;
            }
        }

        [NotMapped]
        // Moms på CD 25 %; 
        // Moms på ljudböcker och språkkurser 6 %;
        // Avrundar momsen till HELTAL;
        public override int Moms
        {
            get
            {
                if (this.Kategori == "Ljudbok" || this.Kategori == "Språkkurs")
                    moms = (int)Math.Round(Pris * 0.06);
                else
                {
                    moms = (int)Math.Round(Pris * 0.25);
                }
                return moms;
            }
        }
    }
}
