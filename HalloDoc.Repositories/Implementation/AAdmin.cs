using HalloDoc.DbEntity.Data;
using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            var abc = JoinAll.Where(x => x.RequestClients.FirstOrDefault().RequestId != null).Include(x => x.Physician).ToList();

            return abc;
        }

        public List<Request> GetPatientData(int id)
        {
            // var data=_logger.RequestClients.FirstOrDefault(x => x.RequestId == id);
            var JoinAll = _logger.Requests.Include(x => x.RequestClients).Where(x => x.RequestClients.FirstOrDefault().RequestId != null);
            var abc = JoinAll.Where(x => x.RequestClients.FirstOrDefault().RequestId == id).ToList();
            return abc;
        }

        public RequestNote GetNotes(int id)
        {
            var notes = _logger.RequestNotes.FirstOrDefault(x => x.RequestId == id) ?? new RequestNote();
            return notes;
        }

        public void GetReqNotesData(RequestNote x)
        {
            _logger.RequestNotes.Add(x);
            _logger.SaveChanges();
        }
        public void GetUpDateReqNote(RequestNote x)
        {
            _logger.RequestNotes.Update(x);
            _logger.SaveChanges();
        }
        public void SaveChanges()
        {
            _logger.SaveChanges();
        }
        public void GetAddRequest(Request x)
        {
            _logger.Requests.Update(x);
            _logger.SaveChanges();
        }
        public void DbUpdatedNotes(RequestNote x)
        {
            _logger.RequestNotes.Update(x);
            _logger.SaveChanges();
        }
        public void DbAddReqClient(Blockrequest x)
        {
            _logger.Blockrequests.Add(x);
            _logger.SaveChanges();
        }
        public Request GetRequestStatus(int id)
        {
            var req = _logger.Requests.FirstOrDefault(x => x.RequestId == id) ?? new Request();
            return req;
        }

        public void DbReqStatusUpdate(Request x)
        {
            _logger.Update(x);
            _logger.SaveChanges();
        }

        public RequestNote checkNotes(int id)
        {
            RequestNote ReqNote = _logger.RequestNotes.FirstOrDefault(x => x.RequestId == id) ?? new RequestNote();
            return ReqNote;

        }

        public List<Request> GetNotes2(int id)
        {

            var JoinAll = _logger.Requests.Include(x => x.RequestNotes).Include(x => x.Physician);
            //var abc = JoinAll.Where(x => x.RequestNotes.FirstOrDefault().RequestId != null).Include(x=>x.Physician);
            var data = JoinAll.Where(x => x.RequestId == id).ToList();
            return data;
        }

        public List<CaseTag> GetCaseTags()
        {
            return _logger.CaseTags.ToList();
        }

        public RequestClient GetDataReqClient(int id)
        {
            RequestClient reqcl = _logger.RequestClients.FirstOrDefault(c => c.RequestId == id) ?? new RequestClient();
            return reqcl;
        }

        public RequestNote GetRequestNotesData(int id)
        {
            RequestNote reqNote = _logger.RequestNotes.FirstOrDefault(c => c.RequestId == id) ?? new RequestNote();
            return reqNote;
        }
        public void GetAddStatusLog(RequestStatusLog vm)
        {
            _logger.RequestStatusLogs.Add(vm);
            _logger.SaveChanges();
        }

        public List<Physician> GetPhysicianData()
        {
            return _logger.Physicians.Include(x => x.PhysicianNotifications).ToList();
        }
        public List<Region> GetAllRegions()
        {
            return _logger.Regions.ToList() ?? new List<Region>();
        }
        public List<Physician> GetPhysicianByReg(int id)
        {
            return _logger.Physicians.Where(x => x.RegionId == id).ToList() ?? new List<Physician>();
        }

        public RequestStatusLog GetStatusLogs(int id)
        {
            RequestStatusLog RSL = _logger.RequestStatusLogs.FirstOrDefault(x => x.RequestId == id) ?? new RequestStatusLog();
            return RSL;
        }
        public List<RequestStatusLog> GetStatusLogs1(int id)
        {
            return _logger.RequestStatusLogs.Where(r => r.RequestId == id).ToList();
        }

        public Physician GetPhysicianDataByID(int? transToPhysicianId)
        {
            Physician phy = _logger.Physicians.FirstOrDefault(p => p.PhysicianId == transToPhysicianId) ?? new Physician();
            return phy;
        }

        public DbSet<Request> GetRequestData()
        {
            return _logger.Requests;
        }
        public List<RequestWiseFile> GetReqWiseFile(int id)
        {
            return _logger.RequestWiseFiles.Where(x => x.RequestId == id).ToList();
        }

        public List<User> GetUserData()
        {
            return _logger.Users.ToList();
        }
        public void GetReqWisFileData(RequestWiseFile requestWiseFile)
        {
            _logger.RequestWiseFiles.Add(requestWiseFile);
            _logger.SaveChanges();
        }
        public List<RequestWiseFile> DownloadAll(int id)
        {
            return _logger.RequestWiseFiles.Where(x => x.RequestId == id).ToList();
        }
        public RequestWiseFile Download(int id)
        {
            RequestWiseFile RWF = _logger.RequestWiseFiles.Find(id) ?? new RequestWiseFile();
            return RWF;
        }

        public void GetDbUpRequestWise(RequestWiseFile model)
        {
            _logger.RequestWiseFiles.Update(model);
            _logger.SaveChanges();
        }

        public AspNetUser GetAspUserData(string? email)
        {
            AspNetUser data = _logger.AspNetUsers.FirstOrDefault(u => u.Email == email) ?? new AspNetUser();
            return data;
        }

        public Request GetEmail(int id)
        {
            Request var = _logger.Requests.FirstOrDefault(u => u.RequestId == id) ?? new Request();
            return var;
        }

        public List<HealthProfessionalType> GetProfessionList(int id)
        {
            return _logger.HealthProfessionalTypes.ToList();
        }
        public RequestWiseFile GetReqWiseFileById(int id)
        {
            RequestWiseFile RWF = _logger.RequestWiseFiles.FirstOrDefault(x => x.RequestId == id) ?? new RequestWiseFile();
            return RWF;
        }
        public List<HealthProfessional> GetVendorByProfId(int id)
        {
            return _logger.HealthProfessionals.Where(x => x.Profession == id).ToList();
        }

        public HealthProfessional GetVendor(int vendorId)
        {
            HealthProfessional Hp = _logger.HealthProfessionals.FirstOrDefault(x => x.VendorId == vendorId) ?? new HealthProfessional();
            return Hp;
        }
        public void UpdateOrders(OrderDetail x)
        {
            _logger.OrderDetails.Add(x);
            _logger.SaveChanges();
        }

        public void GetUpAspUser(AspNetUser asp)
        {
            _logger.AspNetUsers.Update(asp);
            _logger.SaveChanges();
        }

        public void UpReqClient(RequestClient reqcl)
        {
            _logger.RequestClients.Update(reqcl);
            _logger.SaveChanges();
        }

        public void GetUpUser(User use)
        {
            _logger.Users.Update(use);
            _logger.SaveChanges();
        }
        public List<string> GetRole(int? id)
        {
            List<string> str = _logger.AspNetUsers.Include(x => x.Roles).FirstOrDefault(x => x.Id == id)?.Roles.Select(x => x.Name).ToList() ?? new List<string>();
            return str;
        }

        public Admin GetAdminData(int? adminId)
        {
            Admin Adm = _logger.Admins.FirstOrDefault(x => x.AspNetUserId == adminId) ?? new Admin();
            return Adm;
        }

        public AspNetUser GetAspNetUserData(int? adminId)
        {
            AspNetUser Asp = _logger.AspNetUsers.FirstOrDefault(x => x.Id == adminId) ?? new AspNetUser();
            return Asp;
        }

        List<RequestStatusLog> IAAdmin.GetStatusLogs(int id)
        {
            return _logger.RequestStatusLogs.Where(x => x.RequestId == id).ToList();
        }

        public void updateadmintbl(Admin a)
        {
            _logger.Admins.Update(a);
            _logger.SaveChanges();
        }
        public List<AdminRegion> GetAdminRegion(int? adminId)
        {
            return _logger.AdminRegions.Where(x => x.AdminId == adminId).ToList();
        }
        public Region GetRegionById(int? regionId)
        {
            return _logger.Regions.FirstOrDefault(x => x.RegionId == regionId) ?? new Region();
        }

        public Admin GetAdminDataById(int id)
        {
            return _logger.Admins.FirstOrDefault(x => x.AspNetUserId == id) ?? new Admin();
        }

        public void AddAdminRegion(AdminRegion adds)
        {
            _logger.AdminRegions.Add(adds);
            _logger.SaveChanges();
        }

        public void RemoveAdminRegion(AdminRegion removes)
        {
            var data = _logger.AdminRegions.FirstOrDefault(x => x.AdminId == removes.AdminId && x.RegionId == removes.RegionId);
            _logger.AdminRegions.Remove(data);
            _logger.SaveChanges();
        }

        public void UpAdmin(Admin adminData)
        {
            _logger.Admins.Update(adminData);
            _logger.SaveChanges();
        }

        public List<Physician> GetPhysicianDataByRegion(int regionId)
        {
            try
            {
                // Assuming _logger.Physicians is a valid data source
                var physicians = _logger.Physicians
                    .Where(x => x.RegionId == regionId)
                    .Include(x => x.PhysicianNotifications)
                    .ToList();

                return physicians;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error retrieving physician data: {ex.Message}");
                return new List<Physician>(); // Return an empty list or handle the error case
            }
        }

        public List<string> GetRoleEmail(string value)
        {
            List<string> str = _logger.AspNetUsers.Include(x => x.Roles).FirstOrDefault(x => x.Email == value)?.Roles.Select(x => x.Name).ToList() ?? new List<string>();
            return str;
        }

        public Admin GetAdminDataByAdminId(int id)
        {
            return _logger.Admins.FirstOrDefault(x => x.AdminId == id) ?? new Admin();

        }

        public List<PhysicianNotification> GetPhysicianListById(ProviderInformation vm)
        {
            List<PhysicianNotification> physiciansList = new();
            foreach (var item in vm.IsChecked)
            {
                physiciansList.Add(_logger.PhysicianNotifications.FirstOrDefault(x => x.PhysicianId == item)??new());
            }
            return physiciansList;
        }

        public PhysicianNotification GetPhysicianNotification(int phy)
        {
            return _logger.PhysicianNotifications.FirstOrDefault(x => x.PhysicianId == phy)??new();
        }

        public void UpdatePhysicianNotification(PhysicianNotification phy)
        {
            _logger.PhysicianNotifications.Update(phy);
            _logger.SaveChanges();
        }

        public List<PhysicianNotification> GetAllPhysicianNotification()
        {
            return _logger.PhysicianNotifications.ToList();
        }

        public List<Role> GetPhysicianRoles()
        {
            return _logger.Roles.Where(x=>x.AccountType==2).ToList();
        }

        public void AddAspNetUser(AspNetUser aspUser)
        {
            _logger.AspNetUsers.Add(aspUser);
            _logger.SaveChanges();
        }

        public int GetRegionIdByName(string? v)
        {
            return _logger.Regions.FirstOrDefault(x => x.Name == v)?.RegionId??0;
        }

        public void AddPhysician(Physician phy)
        {
            _logger.Physicians.Add(phy);
            _logger.SaveChanges();
        }

        public void AddPhysicianRegion(PhysicianRegion pr)
        {
            _logger.PhysicianRegions.Add(pr);
            _logger.SaveChanges();
        }
        
        public Physician GetPhysicianByEmail(string email)
        {
            return _logger.Physicians.FirstOrDefault(x=>x.Email == email)??new();
        }
    }
}
