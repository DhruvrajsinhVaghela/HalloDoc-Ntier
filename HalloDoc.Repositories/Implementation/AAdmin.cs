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
            
        public List<Request> GetAdminDashboardData()
        {

            var JoinAll = _logger.Requests.Include(x => x.RequestClients);
             var abc=   JoinAll.Where(x => x.RequestClients.FirstOrDefault().RequestId != null).ToList();

            return abc;
        }

        /*public List<Request> GetCount()
        {
            var count = _logger.Requests.Include(x => x.RequestClients).Include(x => x.Physician).GroupBy(x => x.Status).ToList();

            *//*var CountNew = count.FirstOrDefault().Count(x => x.Status == 1);
            var CountPen = count.FirstOrDefault().Count(x => x.Status == 2);
            var CountAct = count.FirstOrDefault().Count(x => x.Status == 4) + count.FirstOrDefault().Count(x => x.Status == 5);
            var CountCon = count.FirstOrDefault().Count(x => x.Status == 3) + count.FirstOrDefault().Count(x => x.Status == 7) + count.FirstOrDefault().Count(x => x.Status == 8);
            var CountClo = count.FirstOrDefault().Count(x => x.Status == 9);
            var CountUnp = count.FirstOrDefault().Count(x => x.Status == 10);*//*
            //swc.Countdata = count;
            return count;

        }*/
    }
}
