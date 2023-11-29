using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceConges : Service<Conges>, IServiceConges
    {
        public ServiceConges(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
           
        }
        
    }
}
