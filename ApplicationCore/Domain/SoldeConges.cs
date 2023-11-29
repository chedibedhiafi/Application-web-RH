using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    
    public class SoldeConges
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [Range(1, 365, ErrorMessage = "Nombre de jour doit être entre 1 et 365")]
        [DisplayName("Nombre de jour")]
        public decimal Nombre { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Nom du Solde")]
        public string Nom { get; set; }
        [DisplayName("Activé ou non")]
        public bool IsActivated { get; set; }
    }
}
