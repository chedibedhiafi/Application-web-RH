using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class Absence
    {
        [DisplayName("Abcense Id")]
        public int AbsenceId { get; set; }
        [DisplayName("Nombre Absence")]
        public int nbAbsence { get; set; }

        public virtual TypeJustificatif TypeJustificatif { get; set; }
        [ForeignKey("TypeJustificatif")]
        public int TypeJustificatifFk { get; set; }

        public virtual Employees Employees { get; set; }
        [ForeignKey("Employees")]

        public int EmplyeesFk { get; set; }


    }
}
