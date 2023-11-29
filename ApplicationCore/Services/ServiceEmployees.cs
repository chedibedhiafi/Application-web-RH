using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class ServiceEmployees : Service<Employees>, IServiceEmployees
    {

        public ServiceEmployees(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        /*
        private readonly DbContext _context;
        
                public ServiceEmployees(DbContext context)
                {
                    _context = context;
                }

                public Employees GetByMatriculeAndNom(string matricule, string nom)
                {
                    return _context.Set<Employees>()
                        .FirstOrDefault(e => e.Matricule == matricule && e.Nom == nom);
                }
            }
        */
       

    }
}
