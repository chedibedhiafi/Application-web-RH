using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationCore.Domain
{
    public class Conges
    {
        [DisplayName("Conges Id")]
        public int CongesId { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Date Debut")]
        public DateTime DateDebut { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Date Fin")]
        public DateTime DateFin { get; set; }
        [DisplayName("Commentaire")]
        public string? Raison { get; set; }
        
        [ForeignKey("Employees")]
        [DisplayName("Employé associé")]
        public int IdEmployees { get; set; }
        public virtual Employees Employees { get; set; }

        [ForeignKey("ConfirmeParEmployee")]
        [DisplayName("Confirmé Par")]
        public int? ConfirmePar { get; set; }
        public virtual Employees? ConfirmeParEmployee { get; set; }

        [ForeignKey("RemplasseParEmployee")]
        [DisplayName("Remplacé Par")]
        public int? RemplassePar { get; set; }
        [DisplayName("Remplacé Par")]
        public virtual Employees? RemplasseParEmployee { get; set; }

    


        [DisplayName("Type du congé")]
        public virtual TypeConges TypeConges { get; set; }
        [ForeignKey("TypeConges")]
        [DisplayName("Type du congé")]

        public int TypeCongesFk { get; set; }
        [DisplayName("Type de confirmation")]
        public virtual TypeConfirmation TypeConfirmation { get; set; }
        [ForeignKey("TypeConfirmation")]
        [DisplayName("Type de confirmation")]

        public int TypeConfirmationFk { get; set; }

        public virtual ICollection<ContreVisite> ContreVisites { get; set; }
        //displayed
        [DisplayName("Formatted Date Debut")]
        public string FormattedDateDebut => DateDebut.ToString("dd/MM/yyyy");

        [DisplayName("Formatted Date Fin")]
        public string FormattedDateFin => DateFin.ToString("dd/MM/yyyy");
        public string DisplayAttributes
        {
            get
            {
                string nom = Employees?.Nom ?? "N/A";
                string prenom = Employees?.Prenom ?? "N/A";
                string dateDebut = DateDebut.ToString("dd/MM");
                string dateFin = DateFin.ToString("dd/MM");

                return $"{nom} - {prenom} - {dateDebut} - {dateFin}";
            }
        }


    }
}
