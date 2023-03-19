using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Projekt.Models
{
    [NotMapped]
    public class FirmCustomer
    {
        private const string kundtyp = "Företag";
        private const string prefix = "FI"; //Firma;

        public string Prefix
        {
            get { return prefix; }
        }

        public string KundTyp
        {
            get { return kundtyp; }
        }

        // För Företagskunder Företagsnamn obligatoriskt;
        [Required(ErrorMessage = "Företaget skall anges")]
        [Display(Name = "Företagsnamn")]
        public string FirmaNamn
        {
            set; get;
        }

        [Required(ErrorMessage = "Kontaktperson skall anges")]
        [FullName(ErrorMessage = "Ditt fullständiga namn")]
        [Display(Name = "Kontaktperson")]
        public string KontaktPerson
        {
            set; get;
        }

        [Required(ErrorMessage = "Gatuadress skall anges")]
        [Display(Name = "Gatuadress")]
        public string GatuAdress
        {
            set; get;
        }

        [Required(ErrorMessage = "Ort skall anges")]
        [Display(Name = "Ort, stad")]
        public string Ort
        {
            set; get;
        }

        [Required(ErrorMessage = "Land skall anges")]
        [Display(Name = "Välj Land")]
        public string Land
        {
            set; get;
        }

        [NotMapped]
        [Display(Name = "Foto")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}