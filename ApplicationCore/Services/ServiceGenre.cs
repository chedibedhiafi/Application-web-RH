using ApplicationCore.Domain;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceGenre:Service<Genre>,IServiceGenres
    {
        public ServiceGenre(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
