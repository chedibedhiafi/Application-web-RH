using ApplicationCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IServiceEmployees:IService<Employees>
    {
        /*
        Employees GetByMatriculeAndNom(string matricule, string nom);
        */
    }
}
