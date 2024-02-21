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


        public string Update(int id,ViewDocumentVM vm);

        public string PatientFileSave(int id, PatientDashboardVM model);

        public PatientInfo PatientMeRequest(int id,PatientInfo model);

        public PatientInfo PatientSomeOneElseRequest(int id,PatientInfo model);

        public AspNetUser GetAsp(string Email);

        public void SaveAspNetUser(AspNetUser asp);

        public void SaveRegion(Region reg);

        public User GetUser(string Email);
        
        public void SaveUser(User user);

        public void SaveRequest(Request request);

        public void SaveRequestClient(RequestClient requestClient);

        public void SaveRequestWiseFile(RequestWiseFile wiseFile);

        public void SaveConcierge(Concierge concierge);

        public void SaveRequestConcierge(RequestConcierge requestConcierge);

        public void SaveBusiness(Business business);

        public void SaveRequestBusiness(RequestBusiness requestBusiness);

        public List<RequestWiseFile> CountFile(int id);

        public List<Request> JoinedResult(int id);

        public List<Physician> Details(int id);

        public User GetUserAsp(int id);

        public List<User> use(int id);

        public RequestWiseFile ReqWiseFileId(int id);

        public List<RequestWiseFile> ReqWiseFileIdAll(int id);

        public AspNetUser GetAspNetUser(int id);
        //-----------------------------------------testing
        public List<ViewDocumentVM> GetInfo(int id);

    }
}
