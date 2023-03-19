using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Projekt.Models
{
    public partial class Book : Media
    {
        private string picture = "def000.jpg";
        private int moms = 0; // SVENSK moms;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nummer { get; set; }
        public string Bok_ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Kategori { get; set; }
        public string Beskrivning { get; set; }
        public string Year { get; set; }
        public string HuvudTyp { get; set; }
        public int Pris { get; set; }
        public DateTime Datum { get; set; }

        public override string Typ { get { return "BOK"; } }
        public override int Number { get { return Nummer; } }
        public override string ID_Nr { get { return Bok_ID; } }
        public override string Titel { get { return Title; } }
        public override string Skapare { get { return Author; } }
        public override string Genre { get { return Kategori; } }
        public override string Description { get { return Beskrivning; } }
        public override string ProdYear { get { return Year; } }
        public override string MediaTyp { get { return HuvudTyp; } }
        public override int Price { get { return Pris; } }
        public override DateTime LogDate { get { return Datum; } }

        public override string Sound
        {
            get { return null; }
        }

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

        public override int Moms
        {
            get
            {
                moms = (int)Math.Round(Pris * 0.06);
                return moms;
            }
        }
    }
}
