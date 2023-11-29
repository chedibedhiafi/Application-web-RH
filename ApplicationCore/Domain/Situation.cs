using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class Situation
    {
        [DisplayName("Situation Id")]
        public int SituationId { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Situation")]
        public string nomSituation { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
