using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    [NotMapped]
    public class CD_DTO
    {
        [Required(ErrorMessage = "Titel måste anges")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Titel måste anges")]
        [Display(Name = "Titel")]
        public string Title { set; get; }

        [Required(ErrorMessage = "Regissör eller Producent måste anges")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Regissör eller Producent måste anges")]
        [FullName(ErrorMessage = "Fullständigt namn")]
        [Display(Name = "Regissör eller Producent")]
        public string Author { set; get; }

        [Required(ErrorMessage = "Kategori måste anges")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "Kategori måste anges")]
        [Display(Name = "Kategori")]
        public string Kategori { set; get; }

        [Required(ErrorMessage = "Beskrivning måste anges")]
        [StringLength(750, MinimumLength = 10, ErrorMessage = "Beskrivning måste anges")]
        [Display(Name = "Beskrivning")]
        public string Beskrivning { set; get; }

        [Required(ErrorMessage = "Utgivningsår måste anges")]
        [StringLength(5, MinimumLength = 4, ErrorMessage = "Utgivningsår måste anges")]
        [Display(Name = "Utgivningsår 4 siffror")]
        public string Year { set; get; }
        public string HuvudTyp { set; get; }

        [Required(ErrorMessage = "Pris måste anges")]
        [PrisInteger(ErrorMessage = "Priset skall vara heltal")]
        [Display(Name = "Pris i hela kronor")]
        public int Pris { get; set; }

        [NotMapped]
        [Display(Name = "Foto")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}