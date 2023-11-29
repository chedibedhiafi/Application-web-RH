using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class TypeJustificatif
    {
        [DisplayName("TypeJustificatif Id")]
        public int TypeJustificatifId { get; set; }
        [DisplayName("Type")]
        public string  type { get; set; }

        public string Document { get; set; }


        public virtual ICollection<Absence> Absences { get; set; }

        public virtual ICollection<GestionDocument> GestionDocuments { get; set; }
    }
}
