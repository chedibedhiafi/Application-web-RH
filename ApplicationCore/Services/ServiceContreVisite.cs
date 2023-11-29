using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceContreVisite:Service<ContreVisite>,IServiceContreVisite
    {
        public ServiceContreVisite(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
