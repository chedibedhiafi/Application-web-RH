using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class Genre
    {
        [DisplayName("Genre Id")]
        public int GenreId { get; set; }
        [DisplayName("Genre")]
        public string NomGenre { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
