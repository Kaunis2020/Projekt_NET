using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    [NotMapped]
    public class LoginData
    {
        [Required(ErrorMessage = "Användarnamn måste anges")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Felaktigt användarnamn")]
        [Display(Name = "Användarnamn")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lösenord måste anges")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Felaktigt lösenord")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
    }
}
