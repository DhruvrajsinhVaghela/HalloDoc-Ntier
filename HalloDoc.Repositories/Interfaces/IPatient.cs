using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.DbEntity.ViewModels;
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

        Task<bool> validate_Email(System.String email);

        string PatientInfoForm(PatientInfo model);

        string PatientFamilyFriendForm(PatientFamilyFriendInfo model);

        string PatientConciergeForm(PatientConciergeInfo model);

        string PatientBusinessForm(PatientBusinessInfo model);

        List<PatientDashboardVM> PatientDashboard(int id);

        public PatientInfo PatientMeRequest(int id, PatientInfo model);

        public PatientInfo PatientSomeOneElseRequest(int id, PatientInfo model);

        List<ViewDocumentVM> PatientViewDocuments(int id);

        public RequestWiseFile Download(int id);

        public List<RequestWiseFile> DownloadAll(int id);

        public string Update(int id,ViewDocumentVM vm);

        string PatientFileSave(int id, PatientDashboardVM model);

        //---------------------------------------------------------

        public GetAuthenticationVM AuthenticateUser(GetAuthenticationVM test);

        //public GetDashboardVM GetPatientDashboard(GetDashboardVM model);


        public GetAuthenticationVM ValidateEmail(GetAuthenticationVM test);

        public GetAllFormsVM GetAllForms(GetAllFormsVM form);

        public void ADDAspNetUser(AspNetUser aspNetUser);
        public void ADDRegion(Region region);

        public void ADDUser(User user);
        public void SaveChanges();

    }
}
