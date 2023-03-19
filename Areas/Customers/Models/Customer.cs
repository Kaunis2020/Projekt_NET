using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Projekt.Models
{
    public partial class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nummer { get; set; }
        public string Kund_ID { get; set; }
        public string KundTyp { get; set; }
        public string FirmaNamn { get; set; }
        public string KontaktPerson { get; set; }
        public string GatuAdress { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Bild { get; set; }
    }
}
