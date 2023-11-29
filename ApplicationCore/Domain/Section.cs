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
    public class Section
    {
        [DisplayName("Departement Id")]
        public int SectionId { get; set; }

        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Nom du département")]
        public string NomSection { get; set; }
        //new
        // Nullable foreign key to the primary key of the same class
        [DisplayName("Département Parent")]
        public int? ParentSectionId { get; set; }

        // Navigation property to the parent section
        [DisplayName("Département Parent")]
        public virtual Section ParentSection { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        
    }
}
