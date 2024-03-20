using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;

namespace HalloDoc.Service.Interface
{
     public interface IPatientService
    {
          AspNetUser PatientLogin(AspNetUser aspNetUser);
          Task<bool> validate_Email(string email);
          string PatientInfoForm(PatientInfoVM model);
          string PatientFamilyFriendForm(PatientFamilyFriendInfoVM model);
          string PatientConciergeForm(PatientConciergeInfoVM model);
          string PatientBusinessForm(PatientBusinessInfoVM model);
          List<PatientDashboardVM> PatientDashboard(int id);
          List<ViewDocumentVM> PatientViewDocuments(int id);
          RequestWiseFile Download(int id);
          List<RequestWiseFile> DownloadAll(PatientDashboardVM vm,int id);
          string Update(int id, ViewDocumentVM vm);
          string PatientFileSave(int id, PatientDashboardVM model);
          PatientInfoVM PatientMeRequest(int id, PatientInfoVM model);
          PatientInfoVM PatientSomeOneElseRequest(int id, PatientInfoVM model);
          bool ValidateUserByEmail(string? email);
          SendAgreementVM GetRequest(int id);
          bool GetAgree(int id);
          bool CancelAgreement(int id, SendAgreementVM VM);
          bool UpdateAspUser(ResetPwVM RP,int id);
        bool CreateUser(ResetPwVM rP);
    }
}
