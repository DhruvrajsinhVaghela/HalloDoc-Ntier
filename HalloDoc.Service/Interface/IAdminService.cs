using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;

namespace HalloDoc.services.Interface
{
    public interface IAdminService
    {
        public AdminDashboardVM PatientStatus(AdminDashboardVM swc);

        public List<AdminDashboardVM> GetNewStateData(int status,int id);
        public ViewCaseVM ViewPatientData(int id);
        public RequestNote ViewNotes(int id, RequestNote vm);
        public ViewNotesVM ViewNotes2(int id);
       /* public void UpNotes(int id,ViewNotesVM vm);*/
        public void UpStatus(int id,CancelCaseVM vm);
        public CancelCaseVM CancelCaseData(int id);
        public AssignCaseVM GetPhysician(int id, AssignCaseVM vm);
        public object GetPhysiciansByRegionId(int id);
        public void UpAssignStatus(int id, AssignCaseVM vm);
        public BlockCaseVM BlockCaseData(int id,BlockCaseVM vm);
        public void UpBlockCase(int id, BlockCaseVM vm);
        public ViewUploadVM PatientViewDocuments(int id);
        public string PatientFileSave(int id, PatientDashboardVM model);
        public List<RequestWiseFile> DownloadAll(int id);
        public RequestWiseFile Download(int id);
        public RequestWiseFile Delete(int id);
        public object DeleteAll(int id);
        public string SMail(int id);
        public AspNetUser AdmintLogin(AspNetUser aspNetUser);
        public SendMailVM SendAgreement(int id);
        public SendMailVM GetReqType(int id,SendMailVM vm);
        public SendOrderVM GetProfessions(int id, SendOrderVM vm);
        public List<HealthProfessional> GetVendorNames(int id);
        public object GetVendorData(int vendorId);
        public bool AddOrderData(int id, SendOrderVM vm);
        public object GetClearCase(int id);
        public bool UpStatusClear(int id);
        public object GetCloseCase(int id);
        public bool UpCloseCase(int id, CloseCaseVM vm);
        public object AdminProfileData(int? adminId);
        public AspNetUser AspUserData(string email);
        public List<AdminDashboardVM> GetDataPagination(int status,int pn,int item);
        public List<AdminDashboardVM> GetFilteredData(string keywrd, int regId, int status, int reqType);

        //public StatusWiseCount PatientCount(StatusWiseCount swc);
    }
}
