using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;

namespace HalloDoc.Service.Interface
{
     public interface IPatientService
    {
          AspNetUser PatientLogin(AspNetUser aspNetUser);
          Task<bool> validate_Email(string email);
          string PatientInfoForm(PatientInfo model);
          string PatientFamilyFriendForm(PatientFamilyFriendInfo model);
          string PatientConciergeForm(PatientConciergeInfo model);
          string PatientBusinessForm(PatientBusinessInfo model);
          List<PatientDashboardVM> PatientDashboard(int id);
          List<ViewDocumentVM> PatientViewDocuments(int id);
          RequestWiseFile Download(int id);
          List<RequestWiseFile> DownloadAll(int id);
          string Update(int id, ViewDocumentVM vm);
          string PatientFileSave(int id, PatientDashboardVM model);
          PatientInfo PatientMeRequest(int id, PatientInfo model);
          PatientInfo PatientSomeOneElseRequest(int id, PatientInfo model);
          bool ValidateUserByEmail(string? email);
          SendAgreementVM GetRequest(int id);
          bool GetAgree(int id);
          bool CancelAgreement(int id, SendAgreementVM VM);
    }
}
