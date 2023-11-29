using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class GestionDocument
    {
        [DisplayName("GestionDocument Id")]
        public int GestionDocumentId { get; set; }

        public virtual TypeJustificatif TypeJustificatif { get; set; }
        [ForeignKey("TypeJustificatif")]
        [DisplayName("Type de justificatif")]
        public int TypeJustificatifFk { get; set; }


        public virtual Attestation Attestation { get; set; }
        [ForeignKey("Attestation")]
        [DisplayName("Attestation")]
        public int AttestationFk { get; set; }

    }
}
