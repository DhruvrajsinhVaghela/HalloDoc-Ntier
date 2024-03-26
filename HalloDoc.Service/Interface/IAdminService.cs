using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.services.Interface
{
    public interface IAdminService
    {
        AdminDashboardVM PatientStatus(AdminDashboardVM swc);

        List<AdminDashboardVM> GetNewStateData(int status,int id);
        ViewCaseVM ViewPatientData(int id);
        RequestNote ViewNotes(int id, ViewNotesVM vm);
        ViewNotesVM ViewNotes2(int id);
       /* public void UpNotes(int id,ViewNotesVM vm);*/
        void UpStatus(int id,CancelCaseVM vm);
        CancelCaseVM CancelCaseData(int id);
        AssignCaseVM GetPhysician(int id, AssignCaseVM vm);
        List<Physician> GetPhysiciansByRegionId(int id);
        void UpAssignStatus(int id, AssignCaseVM vm);
        BlockCaseVM BlockCaseData(int id,BlockCaseVM vm);
        void UpBlockCase(int id, BlockCaseVM vm);
        ViewUploadVM PatientViewDocuments(int id);
        string PatientFileSave(int id, PatientDashboardVM model);
        List<RequestWiseFile> DownloadAll(int id);
        RequestWiseFile Download(int id);
        RequestWiseFile Delete(int id);
        object DeleteAll(int id);
        string SMail(int id);
        AspNetUser AdmintLogin(AspNetUser aspNetUser);
        SendMailVM SendAgreement(int id);
        SendMailVM GetReqType(int id,SendMailVM vm);
        SendOrderVM GetProfessions(int id, SendOrderVM vm);
        List<HealthProfessional> GetVendorNames(int id);
        object GetVendorData(int vendorId);
        bool AddOrderData(int id, SendOrderVM vm);
        object GetClearCase(int id);
        bool UpStatusClear(int id);
        object GetCloseCase(int id);
        bool UpCloseCase(int id, CloseCaseVM vm);
        object AdminProfileData(int? aspId,int? adminId);
        AspNetUser AspUserData(string email);
        List<AdminDashboardVM> GetDataPagination(int status,int pn,int item);
        List<AdminDashboardVM> GetFilteredData(string keywrd, int regId, int status, int reqType, int pn, int item);
        void EditAdminProfile(AdminProfileVM model, int id);
        Admin GetAdminDataById(int id);
        List<string> GetUserRoleById(int? id);
        List<Region> GetAllRegions();
        object GetProviderInfo(int regionId);
        List<ProviderInformation> GetProviderRegion();
        List<string> GetUserRoleByEmail(string value);
        void EditAdminProfile1(AdminProfileVM model, int adminId);
        void EditAdminProfilePw(AdminProfileVM model, int? AspId);
        void ChangeStopNotificaiton(ProviderInformation vm);

        //public StatusWiseCount PatientCount(StatusWiseCount swc);
    }
}
