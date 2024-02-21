using HalloDoc.DbEntity.Models;
using HalloDoc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repositories.Interfaces
{
    public interface IPatient
    {
        //[FromForm]

        Task<int> PatientLogin(AspNetUser aspNetUser);

        List<PatientDashboardVM> PatientDashboard(int id);

        List<ViewDocumentVM> PatientViewDocuments(int id);

        Task<bool> validate_Email(System.String email);

        string PatientInfoForm(PatientInfo model);

        string PatientFamilyFriendForm(PatientFamilyFriendInfo model);
        
        string PatientConciergeForm(PatientConciergeInfo model);

        string PatientBusinessForm(PatientBusinessInfo model);

        public RequestWiseFile Download(int id);

        public List<RequestWiseFile> DownloadAll(int id);

        public string Update(int id,ViewDocumentVM vm);

        string PatientFileSave(int id, PatientDashboardVM model);

        public PatientInfo PatientMeRequest(int id,PatientInfo model);

        public PatientInfo PatientSomeOneElseRequest(int id,PatientInfo model);
    }
}
