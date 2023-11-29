using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceSoldeConges:Service<SoldeConges>,IServiceSoldeConges
    {
        public ServiceSoldeConges(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        //new
            
        }
    }