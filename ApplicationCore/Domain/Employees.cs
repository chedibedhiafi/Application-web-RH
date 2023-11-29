using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class Employees
    { 
        [Key]
        public int id { get; set; }
       
        [Required(ErrorMessage = "champs obligatoire")]

        public string Nom { get; set; }

        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Nom Arabe")]
        public string NomArabe { get; set; }
      
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Prénom Arabe")]
        public string PrenomArabe { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        public String Email { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Date De Naissance")]
        public DateTime DateDeNaissance { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Cin must be 8 digits")]
        public string Cin { get; set; }

        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("Solde de congés")]
        [Range(0, 365, ErrorMessage = "Le solde doit être positif.")]
        public decimal CreditConges { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        public int Matricule { get; set; }

        public string? Photo { get; set; }

       

      

        public virtual ICollection<Conges> Conges { get; set; }

        [DisplayName("Département")]
        public virtual Section Section { get; set; }
        [ForeignKey("Section")]
        [DisplayName("Département")]
        public int SectionFk { get; set; }
        [Required(ErrorMessage = "champs obligatoire")]
        [DisplayName("A commencé le")]
        public DateTime StartedAt { get; set; }
        [DisplayName("Situation")]
        public virtual Situation Situation { get; set; }
        [ForeignKey("Situation")]
        [DisplayName("Situation")]
        public int SituationFk { get; set; }



       
        [DisplayName("Genre")]
        public virtual Genre Genre { get; set; }
        [ForeignKey("Genre")]
        [DisplayName("Genre")]
        public int GenreFk { get; set; }
        [DisplayName("Fonction de l'employé")]
        public virtual FonctionEmployee FonctionEmployee { get; set; }
        [ForeignKey("FonctionEmployee")]
        [DisplayName("Fonction de l'employé")]
        public int FonctionEmployeeFk { get; set; }
   


        public virtual ICollection<ContreVisite> ContreVisites { get; set; }

       

        public virtual ICollection<Absence> Absences { get; set; }





    }
}
