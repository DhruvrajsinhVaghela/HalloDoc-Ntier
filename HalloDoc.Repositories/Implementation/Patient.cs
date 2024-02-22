using HalloDoc.DbEntity.Data;
using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using System.Reflection.Emit;

namespace HalloDoc.Repositories.Implementation
{
    public class Patient : IPatient
    {
        private readonly ApplicationDbContext _logger;

        public Patient(ApplicationDbContext logger)
        {
            _logger = logger;
        }


       
        //public string Update(int id, ViewDocumentVM vm)
        //{

            
        //}
        public string PatientFileSave(int id, PatientDashboardVM model)
        {
            Request user1 = _logger.Requests.FirstOrDefault(u => u.RequestId == id);
            Request request = new Request { };
            if (model.View.UploadFile != null)
            {
                foreach (IFormFile files in model.View.UploadFile)
                {
                    string filename = user1.FirstName + user1.LastName + Path.GetExtension(files.FileName);
                    string path = Path.Combine("D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\", filename);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new RequestWiseFile();
                    requestWiseFile.FileName = filename;
                    requestWiseFile.RequestId = user1.RequestId;
                    requestWiseFile.DocType = 1;
                    _logger.RequestWiseFiles.Add(requestWiseFile);
                    _logger.SaveChanges();
                }
            }
            return "yes";
        }
        public PatientInfo PatientMeRequest(int id,PatientInfo model) 
        {
            //var userid = HttpContext.Session.GetInt32("userId");
            var userobj = _logger.Users.FirstOrDefault(u => u.UserId == id);
            DateOnly dateofbirth = new DateOnly(userobj.IntYear.Value, DateOnly.ParseExact(userobj.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, userobj.IntDate.Value);
            ViewDocumentVM dvm = new ViewDocumentVM()
            {
                user = userobj,
                Date = dateofbirth

            };
            PatientInfo pr = new PatientInfo()
            {
                Email = dvm.user.Email,
                FirstName = dvm.user.FirstName,
                LastName = dvm.user.LastName,
                PhoneNumber = dvm.user.Mobile,
                ZipCode = dvm.user.ZipCode,
                State = dvm.user.State,
                City = dvm.user.City,
                Street = dvm.user.Street,
                Notes = dvm.Notes,
                BirthDate = dvm.Date,
                Room = dvm.Room
                //BirthDate=dvm.dob

            };

            return pr;
        }

        public PatientInfo PatientSomeOneElseRequest(int id, PatientInfo model)
        {

            //var userid = HttpContext.Session.GetInt32("userId");
            var userobj = _logger.Users.FirstOrDefault(u => u.UserId == id);

            var request = _logger.Requests.FirstOrDefault(m => m.Email == model.Email);


            Request req = new Request()
            {
                RequestType = 2,
                UserId = id,
                CreatedUserId = id,
                FirstName = userobj.FirstName,
                LastName = userobj.LastName,
                Email = userobj.Email,
                PhoneNumber = userobj.Mobile,
                Status = 1,
                CreatedDate = DateTime.Now,
                CallType = 1,
                RelationName = model.RelationName
            };
            if (request != null)
            {
                req.UserId = request.UserId;

                req.PatientAccountId = request.PatientAccountId;
            }

            _logger.Requests.Add(req);
            _logger.SaveChanges();


            foreach (IFormFile files in model.Files)
            {
                string filename = files.FileName;
                string path = Path.Combine("D:\\Project\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\", filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    files.CopyToAsync(stream).Wait();
                }

                RequestWiseFile requestWiseFile = new RequestWiseFile();
                requestWiseFile.FileName = filename;
                requestWiseFile.RequestId = req.RequestId;
                requestWiseFile.DocType = 1;
                _logger.RequestWiseFiles.Add(requestWiseFile);
                _logger.SaveChanges();
            }

            Region reg = new Region()
            {
                Name = model.State,
                Abbreviation = model.State
            };
            _logger.Regions.Add(reg);
            _logger.SaveChanges();

            RequestClient reqClient = new RequestClient
            {
                RequestId = req.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.Room,
                Address = model.Street + " , " + model.City + " , " + model.State,
                RegionId = reg.RegionId,
                Email = model.Email,
                Notes = model.Notes,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
            };
            reqClient.StrMonth = DateTime.Now.ToString("MMMM");
            reqClient.IntYear = DateTime.Now.Year;
            reqClient.IntDate = DateTime.Now.Day;
            _logger.RequestClients.Add(reqClient);
            _logger.SaveChanges();


            return model;
        }

        public AspNetUser GetAsp(string email)
        {
            return _logger.AspNetUsers.FirstOrDefault(m => m.Email == email); 
        }

        public void SaveAspNetUser(AspNetUser aspuser)
        {
            _logger.AspNetUsers.Add(aspuser);
            _logger.SaveChanges();
        }

        public void SaveRegion(Region reg)
        {
            _logger.Regions.Add(reg);
            _logger.SaveChanges();
        }

        public User GetUser(string Email) 
        { 
           return _logger.Users.FirstOrDefault(m => m.Email == Email);
        }

        public void SaveUser(User user)
        {
            _logger.Users.Add(user);
            _logger.SaveChanges();
        }

        public void SaveRequest(Request request)
        {
            _logger.Requests.Add(request);
            _logger.SaveChanges();
        }

        public void SaveRequestClient(RequestClient requestClient)
        {
            _logger.RequestClients.Add(requestClient);
            _logger.SaveChanges();
        }

        public void SaveRequestWiseFile(RequestWiseFile requestWise)
        {
            _logger.RequestWiseFiles.Add(requestWise);
            _logger.SaveChanges();
        }

        public void SaveConcierge(Concierge concierge)
        {
            _logger.Concierges.Add(concierge);
            _logger.SaveChanges();
        }

        public void SaveRequestConcierge(RequestConcierge requestConcierge)
        {
            _logger.RequestConcierges.Add(requestConcierge);
            _logger.SaveChanges();
        }

        public void SaveBusiness(Business business)
        {
            _logger.Businesses.Add(business);
            _logger.SaveChanges();
        }

        public void SaveRequestBusiness(RequestBusiness requestBusiness)
        {
            _logger.RequestBusinesses.Add(requestBusiness); 
            _logger.SaveChanges();
        }

        public List<RequestWiseFile> CountFile(int id)
        {
            var x = _logger.RequestWiseFiles.ToList();
            return x;
        }
        public List<Request> JoinedResult(int id)
        {
            var x=_logger.Requests.ToList();
            return x;
        }

        public List<Physician> Details(int id)
        {
            var x = _logger.Physicians.ToList();
            return x;
        }

        public User GetUserAsp(int id)
        {
            return _logger.Users.FirstOrDefault(m=>m.AspNetUserId==id);
        }

        public List<User> use(int id)
        {
            return _logger.Users.ToList();
        }

        public RequestWiseFile ReqWiseFileId(int id)
        {
            return _logger.RequestWiseFiles.Find(id);
        }

        public List<RequestWiseFile> ReqWiseFileIdAll(int id)
        {
            return _logger.RequestWiseFiles.ToList();
        }

        public AspNetUser GetAspNetUser(int id)
        {
            return _logger.AspNetUsers.FirstOrDefault(asp => asp.Id == id);
        }
        //--------------------testing
        //public List<ViewDocumentVM> GetInfo(int id,ViewDocumentVM vm)
        //{
        //    var use = _logger.Users.FirstOrDefault(u => u.AspNetUserId == id);
        //    var asp_net_u = _logger.AspNetUsers.FirstOrDefault(asp => asp.Id == use.AspNetUserId);
        //    var req = _logger.Requests.Where(u => u.User.AspNetUserId == id).ToList();

        //    use.FirstName = vm.user.FirstName;
        //    use.LastName = vm.user.LastName;
        //    use.Email = vm.user.Email;
        //    use.Mobile = vm.user.Mobile;
        //    use.Street = vm.user.Street;
        //    use.City = vm.user.City;
        //    use.State = vm.user.State;
        //    use.ZipCode = vm.user.ZipCode;

        //    _logger.Users.Update(use);
        //    _logger.SaveChanges();

        //    asp_net_u.UserName = vm.user.FirstName + vm.user.LastName;
        //    asp_net_u.Email = vm.user.Email;
        //    asp_net_u.PhoneNumber = vm.user.Mobile;
        //    _logger.AspNetUsers.Update(asp_net_u);
        //    _logger.SaveChanges();

        //    foreach (var re in req)
        //    {
        //        re.FirstName = vm.user.FirstName;
        //        re.LastName = vm.user.LastName;
        //        re.PhoneNumber = vm.user.Mobile;
        //        re.Email = vm.user.Email;
        //        _logger.Requests.Update(re);
        //        _logger.SaveChanges();

        //        var reqclient = _logger.RequestClients.FirstOrDefault(m => m.RequestId == re.RequestId);
        //        if (reqclient != null)
        //        {
        //            reqclient.FirstName = vm.user.FirstName;
        //            reqclient.LastName = vm.user.LastName;
        //            reqclient.PhoneNumber = vm.user.Mobile;

        //            reqclient.Address = vm.user.Street + " , " + vm.user.City + " , " + vm.user.State;
        //            reqclient.Email = vm.user.Email;
        //            reqclient.Street = vm.user.Street;
        //            reqclient.City = vm.user.City;
        //            reqclient.State = vm.user.State;
        //            reqclient.ZipCode = vm.user.ZipCode;
        //            _logger.RequestClients.Update(reqclient);
        //            _logger.SaveChanges();
        //        }
        //    }
        //    return ;
        //}

    }
}
