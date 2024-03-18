using HalloDoc.DbEntity.Models;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Repositories.Interfaces
{
    public interface IAAdmin
    {
        public List<Request> GetAdminDashboardData();
        
        public List<Request> GetPatientData(int id);
        //public StatusWiseCount GetCount();
        public RequestNote GetNotes(int id);
        //public RequestNote GetUpdatedNotes(int id);
        public void SaveChanges();
        public void GetAddRequest(Request x);
        public void DbUpdatedNotes(RequestNote x);
        public Request GetRequestStatus(int id);
        public void DbReqStatusUpdate(Request x);
        public RequestNote checkNotes(int id, RequestNote vm);
        public List<Request> GetNotes2(int id);
        public void GetReqNotesData(RequestNote x);
        public void DbAddReqClient(Blockrequest x);
        public void GetUpDateReqNote(RequestNote x);
        public List<CaseTag> GetCaseTags();
        public RequestClient GetDataReqClient(int id);
        public RequestNote GetRequestNotesData(int id);
        public void GetAddStatusLog(RequestStatusLog vm);
        public List<Physician> GetPhysicianData();
        public List<Region> GetAllRegions();
        public List<Physician> GetPhysicianByReg(int id);
        public List<RequestStatusLog> GetStatusLogs(int id);
        public List<RequestStatusLog> GetStatusLogs1(int id);
        Physician GetPhysicianDataByID(int? transToPhysicianId);
        public DbSet<Request> GetRequestData();
        public List<RequestWiseFile> GetReqWiseFile(int id);
        public List<User> GetUserData();
        public void GetReqWisFileData(RequestWiseFile requestWiseFile);
        public List<RequestWiseFile> DownloadAll(int id);
        public RequestWiseFile Download(int id);
        public void GetDbUpRequestWise(RequestWiseFile model);
        public AspNetUser GetAspUserData(string? email);
        public Request GetEmail(int id);
        public List<HealthProfessionalType> GetProfessionList(int id);
        public List<HealthProfessional> GetVendorByProfId(int id);
        public HealthProfessional GetVendor(int vendorId);
        public void UpdateOrders(OrderDetail x);
        public RequestWiseFile GetReqWiseFileById(int id);
        public void GetUpAspUser(AspNetUser asp);
        public void UpReqClient(RequestClient reqcl);
        public void GetUpUser(User use);
        public List<string> GetRole(int id);
        public Admin GetAdminData(int? adminId);
        public AspNetUser GetAspNetUserData(int? adminId);
        /*public RequestWiseFile GetReqWiseFileById(int id);*/
        public void updateadmintbl(Admin a);
    }
}
