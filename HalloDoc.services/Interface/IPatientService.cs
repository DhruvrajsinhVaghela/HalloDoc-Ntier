using HalloDoc.DbEntity.ViewModel;
using HalloDoc.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.services.Interface
{
    public interface IPatientService
    {
        
        public string PatientInfoForm(PatientInfo model);

        public int PatientLogin(AspNetUser aspNetUser);
        
        public bool validate_Email(string email);

        public string PatientFamilyFriendForm(PatientFamilyFriendInfo model);
        public string PatientConciergeForm(PatientConciergeInfo model);
        public string PatientBusinessForm(PatientBusinessInfo model);
        public List<PatientDashboardVM> PatientDashboard(int id);
        public List<ViewDocumentVM> PatientViewDocuments(int id);
        public RequestWiseFile Download(int id);
        public List<RequestWiseFile> DownloadAll(int id);
        //public string Update(int id, ViewDocumentVM vm);
    }
}
