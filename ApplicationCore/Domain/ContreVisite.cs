using Microsoft.VisualBasic;
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
    public class ContreVisite
    {
        [DisplayName("ContreVisite Id")]
        public int ContreVisiteId { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        public DateTime Date { get; set; }
       
        public virtual Employees Employees { get; set; }
        [ForeignKey("Employees")]
        [DisplayName("Effectuer Par")]
        public int EffectuerPar { get; set; }

        public virtual Conges Conge { get; set; }
        [ForeignKey("Conge")]
        [DisplayName("Congé associé")]
        public int CongesFk { get; set; }
        [DisplayName("Nom Du medecin")]
        public string NomDoc { get; set; }
        public string Avis { get; set; }
        public string Details { get; set; }
        [DisplayName("Encore valide à réaliser")]
        public bool Etat { get; set; }
        public bool DoneOrNot { get; set; }

        //prop etat
        public bool HasDatePassed
        {
            get { return DateTime.Now > Date; }
        }

        // Constructor to set Etat based on Date
        public ContreVisite()
        {
            Etat = DateTime.Now <= Date;
        }

    }
}
