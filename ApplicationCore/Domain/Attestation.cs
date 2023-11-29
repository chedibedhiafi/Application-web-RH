using Microsoft.AspNetCore.Http;
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
    public class Attestation
    {
        [DisplayName("Attestation Id")]
        public int AttestationId { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        public string Description { get; set; }


        [DisplayName("Signature")]
        public string DocumentAttestation { get; set; }



        [DisplayName("Type d'attestation")]
        public virtual TypeAttestation TypeAttestation { get; set; }
        [ForeignKey("TypeAttestation")]
        [DisplayName("Type d'attestation")]
        public int TypeAttestationFk { get; set; }

        public virtual ICollection<GestionDocument> GestionDocuments { get; set; }
    }
}
