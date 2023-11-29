using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class Mission
    {
        [DisplayName("Mission Id")]
        public int MissionId { get; set; }

        public string Description { get; set; }
        [DisplayName("Date de début")]
        public DateTime DateDebut { get; set; }
        [DisplayName("Date de fin")]
        public DateTime DateFin { get; set; }

        


    }
}
