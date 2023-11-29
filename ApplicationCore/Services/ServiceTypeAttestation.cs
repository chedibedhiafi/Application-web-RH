using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceTypeAttestation:Service<TypeAttestation>,IServiceTypeAttestation
    {
        public ServiceTypeAttestation(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
