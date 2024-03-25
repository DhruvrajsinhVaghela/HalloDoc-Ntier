using HalloDoc.DbEntity.Data;
using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Repositories.Implementation
{
    public class Patient : IPatient
    {
        private readonly ApplicationDbContext _logger;

        public Patient(ApplicationDbContext logger)
        {
            _logger = logger;
        }

        public RequestWiseFile? Download(int id)
        {
            var x = _logger.RequestWiseFiles.Find(id);
            if (x != null)
            {
                return x;
            }
            return default(RequestWiseFile);
        }

        public List<RequestWiseFile> DownloadAll(PatientDashboardVM vm, int id)
        {
            List<RequestWiseFile> files = new List<RequestWiseFile>();
            if (vm.FileId != null)
            {
                foreach (var i in vm.FileId)
                {
                    RequestWiseFile file = _logger.RequestWiseFiles.FirstOrDefault(x => x.RequestWiseFileId == i) ?? new RequestWiseFile();
                    files.Add(file);
                }
            }
            else
            {
                files = _logger.RequestWiseFiles.Where(x => x.RequestId == id).ToList();
            }
            return files;
        }




        //------------------------------------------
        public AspNetUser GetAspUserData(string email)
        {
            AspNetUser data = _logger.AspNetUsers.FirstOrDefault(u => u.Email == email) ?? new AspNetUser();
            return data;
        }
        public User GetUserUserData(string email)
        {
            User data = _logger.Users.FirstOrDefault(u => u.Email == email) ?? new User();
            return data;
        }
        public User GetUserAspIdData(int id)
        {
            User data = _logger.Users.FirstOrDefault(u => u.AspNetUserId == id) ?? new User();
            return data;
        }
        public User GetUserUserIdData(int id)
        {
            User data = _logger.Users.FirstOrDefault(u => u.UserId == id) ?? new User();
            return data;
        }



        public DbSet<User> GetUserData()
        {
            return _logger.Users;
        }
        public DbSet<Request> GetRequestData()
        {
            return _logger.Requests;
        }

        public DbSet<RequestWiseFile> GetReqWisFileData()
        {
            return _logger.RequestWiseFiles;
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

        public bool GetEmail(string? email)
        {
            bool var = _logger.AspNetUsers.Any(u => u.Email == email);
            return var;
        }
        public Region GetUniqueRegion(string State)
        {
            Region reg = _logger.Regions.FirstOrDefault(c => c.Name == State.ToUpper()) ?? new Region();
            return reg;
        }


        public void AddReqStatusLog(RequestStatusLog x)
        {
            _logger.RequestStatusLogs.Add(x);
            _logger.SaveChanges();
        }

        public void AddAspNetUser(AspNetUser aspnetuser1)
        {
            _logger.AspNetUsers.Add(aspnetuser1);
            _logger.SaveChanges();
        }

        public AspNetUser GetAspNetUser(int? aspNetUserId)
        {
            return _logger.AspNetUsers.FirstOrDefault(x => x.Id == aspNetUserId) ?? new AspNetUser();
        }

        public void UpdateAspNetUser(AspNetUser asp_net_u)
        {
            _logger.AspNetUsers.Update(asp_net_u);
            _logger.SaveChanges();
        }

        public void AddRegion(Region region)
        {
            _logger.Regions.Add(region);
            _logger.SaveChanges();
        }

        public void AddUser(User user1)
        {
            _logger.Users.Add(user1);
            _logger.SaveChanges();
        }

        public User GetUser(int patientAccountId)
        {
            return _logger.Users.FirstOrDefault(x => x.AspNetUserId == patientAccountId) ?? new User();
        }

        public void UpdateUser(User use)
        {
            _logger.Users.Update(use);
            _logger.SaveChanges();
        }

        public void AddRequest(Request request)
        {
            _logger.Requests.Add(request);
            _logger.SaveChanges();
        }

        public Request GetRequestByEmail(string email)
        {
            return _logger.Requests.FirstOrDefault(x => x.Email == email) ?? new Request();
        }

        public Request GetRequestById(int? id)
        {
            return _logger.Requests.FirstOrDefault(x => x.RequestId == id) ?? new Request();
        }

        public void UpdateRequest(Request req)
        {
            _logger.Requests.Update(req);
            _logger.SaveChanges();
        }

        public void AddReqClient(RequestClient requestClient)
        {
            _logger.RequestClients.Add(requestClient);
            _logger.SaveChanges();
        }

        public void UpdateReqClient(RequestClient reqclient)
        {
            _logger.RequestClients.Update(reqclient);
            _logger.SaveChanges();
        }

        public RequestClient GetReqClientById(int requestId)
        {
            return _logger.RequestClients.FirstOrDefault(x => x.RequestId == requestId) ?? new RequestClient();
        }

        public void AddReqWisFile(RequestWiseFile requestWiseFile)
        {
            _logger.RequestWiseFiles.Add(requestWiseFile);
            _logger.SaveChanges();
        }

        public void AddConcierge(Concierge concierge)
        {
            _logger.Concierges.Add(concierge);
            _logger.SaveChanges();
        }

        public void AddReqConcierge(RequestConcierge request1)
        {
            _logger.RequestConcierges.Add(request1);
            _logger.SaveChanges();
        }

        public void AddBusiness(Business business)
        {
            _logger.Businesses.Add(business);
            _logger.SaveChanges();
        }

        public void AddReqBusiness(RequestBusiness requestBusiness)
        {
            _logger.RequestBusinesses.Add(requestBusiness);
            _logger.SaveChanges();
        }

        public ICollection<AspNetRole> GetPatientRole()
        {
            return _logger.AspNetRoles.Where(x => x.Id == 3).ToList();
        }

        public RequestClient GetReqClientByEmail(string email)
        {
            return _logger.RequestClients.FirstOrDefault(x => x.Email == email);
        }
    }


}
