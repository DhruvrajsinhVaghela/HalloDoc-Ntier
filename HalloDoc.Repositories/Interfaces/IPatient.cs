
using HalloDoc.DbEntity.Models;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Repositories.Interfaces
{
    public interface IPatient
    {

        RequestWiseFile? Download(int id);

        List<RequestWiseFile> DownloadAll(int id);
        
        //--------------------------------------
        AspNetUser GetAspUserData(string email);
        User GetUserUserData(string email);
        User GetUserAspIdData(int id);
        
        DbSet<User> GetUserData();
        DbSet<Request> GetRequestData();
        
        DbSet<RequestWiseFile> GetReqWisFileData();
        DbSet<Physician> GetPhysicianData();
        string GetConfirmationNo();
        User GetUserUserIdData(int id);
        bool GetEmail(string? email);
        Region GetUniqueRegion(string State);
        RequestClient GetRequestClientinfo(int id);
        void AddReqStatusLog(RequestStatusLog x);
        void AddAspNetUser(AspNetUser aspnetuser1);
        AspNetUser GetAspNetUser(int? aspNetUserId);
        void UpdateAspNetUser(AspNetUser asp_net_u);
        void AddRegion(Region region);
        void AddUser(User user1);
        User GetUser(int patientAccountId);
        void UpdateUser(User use);
        void AddRequest(Request request);
        Request GetRequestByEmail(string email);
        Request GetRequestById(int id);
        void UpdateRequest(Request req);
        void AddReqClient(RequestClient requestClient);
        void UpdateReqClient(RequestClient reqclient);
        RequestClient GetReqClientById(int requestId);
        void AddReqWisFile(RequestWiseFile requestWiseFile);
        void AddConcierge(Concierge concierge);
        void AddReqConcierge(RequestConcierge request1);
        void AddBusiness(Business business);
        void AddReqBusiness(RequestBusiness requestBusiness);
    }
}
