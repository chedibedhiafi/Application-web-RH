using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class FonctionEmployee
    {
        [DisplayName("FonctionEmployee Id")]
        public int FonctionEmployeeId { get; set; }
        public string Fonction { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
