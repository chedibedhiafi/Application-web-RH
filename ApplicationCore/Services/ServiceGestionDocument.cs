using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceGestionDocument:Service<GestionDocument>,IServiceGestionDocument
    {
        public ServiceGestionDocument(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
