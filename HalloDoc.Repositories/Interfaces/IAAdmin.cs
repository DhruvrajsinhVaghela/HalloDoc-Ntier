using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Repositories.Interfaces
{
    public interface IAAdmin
    {
        List<Request> GetAdminDashboardData();
        
        List<Request> GetPatientData(int id);
        //public StatusWiseCount GetCount();
        RequestNote GetNotes(int id);
        //public RequestNote GetUpdatedNotes(int id);
        void SaveChanges();
        void GetAddRequest(Request x);
        void DbUpdatedNotes(RequestNote x);
        Request GetRequestStatus(int id);
        void DbReqStatusUpdate(Request x);
        RequestNote checkNotes(int id);
        List<Request> GetNotes2(int id);
        void GetReqNotesData(RequestNote x);
        void DbAddReqClient(Blockrequest x);
        void GetUpDateReqNote(RequestNote x);
        List<CaseTag> GetCaseTags();
        RequestClient GetDataReqClient(int id);
        RequestNote GetRequestNotesData(int id);
        void GetAddStatusLog(RequestStatusLog vm);
        List<Physician> GetPhysicianData();
        List<Region> GetAllRegions();
        List<Physician> GetPhysicianByReg(int id);
        List<RequestStatusLog> GetStatusLogs(int id);
        List<RequestStatusLog> GetStatusLogs1(int id);
        Physician GetPhysicianDataByID(int? transToPhysicianId);
        DbSet<Request> GetRequestData();
        List<RequestWiseFile> GetReqWiseFile(int id);
        List<User> GetUserData();
        void GetReqWisFileData(RequestWiseFile requestWiseFile);
        List<RequestWiseFile> DownloadAll(int id);
        RequestWiseFile Download(int id);
        void GetDbUpRequestWise(RequestWiseFile model);
        AspNetUser GetAspUserData(string? email);
        Request GetEmail(int id);
        List<HealthProfessionalType> GetProfessionList(int id);
        List<HealthProfessional> GetVendorByProfId(int id);
        HealthProfessional GetVendor(int vendorId);
        void UpdateOrders(OrderDetail x);
        RequestWiseFile GetReqWiseFileById(int id);
        void GetUpAspUser(AspNetUser asp);
        void UpReqClient(RequestClient reqcl);
        void GetUpUser(User use);
        List<string> GetRole(int? id);
        Admin GetAdminData(int? adminId);
        AspNetUser GetAspNetUserData(int? adminId);
        /*public RequestWiseFile GetReqWiseFileById(int id);*/
        void updateadmintbl(Admin a);
        List<AdminRegion> GetAdminRegion(int? adminId);
        Region GetRegionById(int? regionId);
        Admin GetAdminDataById(int id);
        void AddAdminRegion(AdminRegion adds);
        void RemoveAdminRegion(AdminRegion removes);
        void UpAdmin(Admin adminData);
        List<Physician> GetPhysicianDataByRegion(int regionId);
        List<string> GetRoleEmail(string value);
        Admin GetAdminDataByAdminId(int id);
        List<PhysicianNotification> GetPhysicianListById(ProviderInformation vm);
        PhysicianNotification GetPhysicianNotification(int phy);
        void UpdatePhysicianNotification(PhysicianNotification phy);
        List<PhysicianNotification> GetAllPhysicianNotification();
        List<Role> GetPhysicianRoles();
        void AddAspNetUser(AspNetUser aspUser);
        int GetRegionIdByName(string? v);
        void AddPhysician(Physician phy);
        void AddPhysicianRegion(PhysicianRegion pr);
        Physician GetPhysicianByEmail(string email);
    }
}
