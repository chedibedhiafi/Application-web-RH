using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class TypeAttestation
    {
        [Key]
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string Contenu { get; set; }
        public virtual ICollection<Attestation> Attestations { get; set; }
    }
}
