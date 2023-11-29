using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class TypeConges
    {
        [DisplayName("TypeConges Id")]
        public int TypeCongesId { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Type")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Nombre Maximum De Jour")]
        public int nbMaximumdeJour { get; set; }

        [DisplayName("Nécessite une contre visite ou pas")]
        public bool NeedsCv { get; set; }
        public virtual ICollection<Conges> Conges { get; set; }
       

    }
}
