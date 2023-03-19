using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

#nullable disable

namespace Projekt.Models
{
    public partial class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nummer { get; set; }

        public string ID { get; set; }

        [Required(ErrorMessage = "Föramnet ska anges")]
        [DataType(DataType.Text)]
        [Display(Name = "Förnamn")]
        public string First_name { get; set; }

        [Required(ErrorMessage = "Efternamnet ska anges")]
        [DataType(DataType.Text)]
        [Display(Name = "Efternamn")]
        public string Last_name { get; set; }

        [Required(ErrorMessage = "Epost ska anges")]
        [DataType(DataType.Text)]
        [Display(Name = "Epost")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon ska anges")]
        [DataType(DataType.Text)]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Avdelning ska anges")]
        [DataType(DataType.Text)]
        [Display(Name = "Avdelning")]
        public string Department { get; set; }

        public string Bild { get; set; }

        [NotMapped]
        [Display(Name = "Foto")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
