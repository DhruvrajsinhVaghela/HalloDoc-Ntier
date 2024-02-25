using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Service.Interface
{
    public interface IPatientService
    {
        public int PatientLogin(AspNetUser aspNetUser);
        public Task<bool> validate_Email(string email);
        public string PatientInfoForm(PatientInfo model);
        public string PatientFamilyFriendForm(PatientFamilyFriendInfo model);
        public string PatientConciergeForm(PatientConciergeInfo model);
        public string PatientBusinessForm(PatientBusinessInfo model);
        public List<PatientDashboardVM> PatientDashboard(int id);
        public List<ViewDocumentVM> PatientViewDocuments(int id);
        public RequestWiseFile Download(int id);
        public List<RequestWiseFile> DownloadAll(int id);
        public string Update(int id, ViewDocumentVM vm);
        public string PatientFileSave(int id, PatientDashboardVM model);
        public PatientInfo PatientMeRequest(int id, PatientInfo model);
        public PatientInfo PatientSomeOneElseRequest(int id, PatientInfo model);
    }
}
