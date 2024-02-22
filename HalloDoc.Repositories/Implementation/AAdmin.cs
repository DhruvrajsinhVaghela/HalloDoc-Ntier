using HalloDoc.DbEntity.Data;
using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc.Repositories.Implementation
{
    public class AAdmin : IAAdmin
    {
        private readonly ApplicationDbContext _logger;

        public AAdmin(ApplicationDbContext logger)
        {
            _logger = logger;
        }

        public List<Request> GetDetails()
        {
            
            
            var ab = _logger.Requests.Include(x=>x.RequestClients).Include(x => x.Physician).ToList();

            return ab;
        }
    }
}
