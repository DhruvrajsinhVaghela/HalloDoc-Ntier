using HalloDoc.DbEntity.Data;
using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using System.Reflection.Emit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HalloDoc.Repositories.Implementation
{
    public class Patient : IPatient
    {
        private readonly ApplicationDbContext _logger;

        public Patient(ApplicationDbContext logger)
        {
            _logger = logger;
        }
       

        /*public List<PatientDashboardVM> PatientDashboard(int id)
        {
           

            select request_id,count(file_name)
            from request_wise_file
            group by request_id;*//*
            var count_file = _logger.RequestWiseFiles
                        .GroupBy(r => r.RequestId)
                        .Select(g => new
                        {
                            RequestId = g.Key,
                            FileName = g.First().FileName,
                            FileCount = g.Count()
                        })
                        .ToList();
            //join it with request table for getting other fields of request table
            var joinedResult = from r in _logger.Requests.ToList()
                               join coun in count_file
                               on r.RequestId equals coun.RequestId
                               into x
                               from coun in x.DefaultIfEmpty()
                               where r.PatientAccountId == id
                               select new
                               {
                                   r,
                                   coun
                               };

            var finalResult = joinedResult.ToList();


            var details = (from datatbl in finalResult
                           join phytbl in _logger.Physicians.ToList()
                           on datatbl.r.PhysicianId equals phytbl.PhysicianId
                           into x
                           from phytbl in x.DefaultIfEmpty()
                           select new
                           {
                               datatbl,
                               phytbl,
                           }).ToList();

            var user2 = _logger.Users.FirstOrDefault(u => u.AspNetUserId == id);

            DateOnly date = new DateOnly(user2.IntYear.Value, DateOnly.ParseExact(user2.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, user2.IntDate.Value);


            List<PatientDashboardVM> dashboard = new List<PatientDashboardVM>();
            foreach (var vm in details)

            {
                var Phy_name = "no data";
                if (vm.phytbl != null)
                {
                    Phy_name = vm.phytbl.FirstName;
                }
                if (vm.datatbl.coun == null)
                {
                    dashboard.Add(new PatientDashboardVM
                    {
                        requestID = vm.datatbl.r.RequestId,
                        CreatedDate = vm.datatbl.r.CreatedDate,
                        Status = vm.datatbl.r.Status,
                        FirstName = vm.datatbl.r.FirstName,
                        count_file = 0,
                        FileName = "",
                        user = user2,
                        PhysicianName = Phy_name,
                        Date = date
                    });
                }
                else
                {
                    dashboard.Add(new PatientDashboardVM
                    {
                        requestID = vm.datatbl.r.RequestId,
                        CreatedDate = vm.datatbl.r.CreatedDate,
                        Status = vm.datatbl.r.Status,
                        FirstName = vm.datatbl.r.FirstName,
                        count_file = vm.datatbl.coun.FileCount,
                        FileName = vm.datatbl.coun.FileName,
                        user = user2,
                        PhysicianName = Phy_name,
                        Date = date
                    });
                }
            };
            return dashboard;
        }*/

       
        public RequestWiseFile Download(int id)
        {
            return _logger.RequestWiseFiles.Find(id);
        }

        public List<RequestWiseFile> DownloadAll(int id)
        {
            return _logger.RequestWiseFiles.Where(x => x.RequestId == id).ToList();
        }
       
      
        

        //------------------------------------------
        public AspNetUser GetAspUserData(string email)
        {
            var data = _logger.AspNetUsers.FirstOrDefault(u => u.Email == email);
            return data;
        }
        public User GetUserUserData(string email)
        {
            var data = _logger.Users.FirstOrDefault(u => u.Email == email);
            return data;
        }
        public User GetUserAspIdData(int id)
        {
            var data= _logger.Users.FirstOrDefault(u => u.AspNetUserId == id);
            return data;
        }
        public User GetUserUserIdData(int id)
        {
            var data=_logger.Users.FirstOrDefault(u => u.UserId == id);
            return data;
        }
        public void GetSaveChanges()
        {
            _logger.SaveChanges();
        }

        public DbSet<AspNetUser> GetAspData()
        {
            return _logger.AspNetUsers;

        }
        public DbSet<Region> GetRegionData()
        {
            return _logger.Regions;
        }
        public DbSet<User> GetUserData()
        {
            return _logger.Users;
        }
        public DbSet<Request> GetRequestData()
        {
            return _logger.Requests;
        }
        public DbSet<RequestClient> GetReqClientData()
        {
            return _logger.RequestClients;
        }
        public DbSet<RequestWiseFile> GetReqWisFileData()
        {
            return _logger.RequestWiseFiles;
        }

        public DbSet<Concierge> GetConciergeData()
        {
            return _logger.Concierges;
        }
        public DbSet<RequestConcierge> GetReqConciergeData()
        {
            return _logger.RequestConcierges;
        }

        public DbSet<Business> GetBusinessData()
        {
            return _logger.Businesses;
        }
        public DbSet<RequestBusiness> GetReqBusinessData()
        {
            return _logger.RequestBusinesses;
        }
        public DbSet<Physician> GetPhysicianData()
        {
            return _logger.Physicians;

        }
        public string GetConfirmationNo()
        {
            var today = DateTime.Today;
            var requestCount = _logger.Requests
            .Where(r => r.CreatedDate >= today)
            .Count().ToString("D4");
            return requestCount;
        }

        
    }
}
