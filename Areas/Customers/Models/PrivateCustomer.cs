using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Projekt.Models
{
    [NotMapped]
    public class PrivateCustomer
    {
        private const string kundtyp = "Privat";
        private const string prefix = "PR"; //Privat;

        public string Prefix
        {
            get { return prefix; }
        }

        public string KundTyp
        {
            get { return kundtyp; }
        }

        [Required(ErrorMessage = "Fullständigt namn skall anges")]
        [FullName(ErrorMessage = "Fullständigt namn")]
        [Display(Name = "Kundens namn")]
        public string PersonNamn
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
