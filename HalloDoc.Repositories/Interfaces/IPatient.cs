
using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repositories.Interfaces
{
    public interface IPatient
    {

        public RequestWiseFile Download(int id);

        public List<RequestWiseFile> DownloadAll(int id);
        
        //--------------------------------------
        public AspNetUser GetAspUserData(string email);
        public User GetUserUserData(string email);
        public User GetUserAspIdData(int id);
        public void GetSaveChanges();
        public DbSet<AspNetUser> GetAspData();
        public DbSet<Region> GetRegionData();
        public DbSet<User> GetUserData();
        public DbSet<Request> GetRequestData();
        public DbSet<RequestClient> GetReqClientData();
        public DbSet<RequestWiseFile> GetReqWisFileData();
        public DbSet<Concierge> GetConciergeData();
        public DbSet<RequestConcierge> GetReqConciergeData();
        public DbSet<Business> GetBusinessData();
        public DbSet<RequestBusiness> GetReqBusinessData();
        public DbSet<Physician> GetPhysicianData();
        public string GetConfirmationNo();
        public User GetUserUserIdData(int id);
    }
}
