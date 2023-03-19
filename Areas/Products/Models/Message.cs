using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Projekt.Models
{
    public partial class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        [Required(ErrorMessage = "Namnet måste anges")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Epostadress måste anges")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Land måste anges")]
        [DataType(DataType.Text)]
        [StringLength(50)]
        [Display(Name = "Land")]
        public string Land { get; set; }
        
        [Required(ErrorMessage = "Rubrik måste anges")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Display(Name = "Rubrik")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Meddelandet måste anges")]
        [DataType(DataType.Text)]
        [StringLength(1000)]
        [Display(Name = "Meddelande", Description = "Meddelande")]
        public string Meddelande { get; set; }

        [Required]
        [ReadOnly(true)]
        public DateTime Datum { get; set; } = DateTime.Today;

        [NotMapped]
        [ReadOnly(true)]
        public static string Postat { get; set; } = DateTime.Today.ToShortDateString();
    }
}
