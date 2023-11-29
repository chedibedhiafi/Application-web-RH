using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class TypeConfirmation
    {
        [DisplayName("TypeConfirmation Id")]
        public int TypeConfirmationId { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Type")]
        public string type  { get; set; }

        public virtual ICollection<Conges> Conges { get; set; }
    }
}
