using HalloDoc.DbEntity.Data;
using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModels;
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
using HalloDoc.DbEntity.ViewModel;

namespace HalloDoc.Repositories.Implementation
{
    public class Patient : IPatient
    {
        private readonly ApplicationDbContext _logger;

        public Patient(ApplicationDbContext logger)
        {
            _logger = logger;
        }
        public string PatientInfoForm(PatientInfo model)
        {

            //AspNetUser aspnetuser = _logger.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);

            //if (aspnetuser == null)
            //{
            //    AspNetUser aspnetuser1 = new AspNetUser
            //    {

            //        UserName = model.FirstName + "_" + model.LastName,
            //        Email = model.Email,
            //        PasswordHash = model.FirstName,
            //        PhoneNumber = model.PhoneNumber,
            //        CreatedDate = DateTime.Now //here,modified Date,modified By coulmns-remaining to add    
            //    };
            //    aspnetuser = aspnetuser1;
            //    if (model.Password != null)
            //    {
            //        aspnetuser.PasswordHash = Crypto.HashPassword(model.Password);
            //    }
            //    _logger.AspNetUsers.Add(aspnetuser1);
            //    _logger.SaveChanges();

            //}
            //region is first here because when we add region id foreign key in user table it will show error if object is not created 
            /* Region region = new Region
             {
                 Name = model.State,
                 Abbreviation = model.State.Substring(0, 3)
             };*/
            /*_logger.Regions.Add(region);
            _logger.SaveChanges();*/
/*
            User user1 = _logger.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user1 == null)
            {
                User user = new User
                {
                    AspNetUserId = aspnetuser.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.PhoneNumber,
                    ZipCode = model.ZipCode,
                    State = model.State,
                    City = model.City,
                    Street = model.Street,
                    IntDate = model.BirthDate.Day,
                    IntYear = model.BirthDate.Year,
                    StrMonth = model.BirthDate.ToString("MMMM"),
                    CreatedDate = DateTime.Now,
                    RegionId = region.RegionId,
                    CreatedBy = model.FirstName,
                    AspNetUser = aspnetuser //modified_by,modified_date,status,ip,is_request_with_email,is_deleted remaining to add
                };
                user1 = user;
                _logger.Users.Add(user);
                _logger.SaveChanges();

            }*/
            Request request = new Request
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CreatedDate = DateTime.Now,
                Status = 1,
                PatientAccountId = aspnetuser.Id,
                UserId = user1.UserId
            };
            _logger.Requests.Add(request);
            _logger.SaveChanges();


            RequestClient requestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.City,
                Address = model.Room,
                RegionId = region.RegionId,
                Email = model.Email,
                Notes = model.Notes,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                StrMonth = model.BirthDate.ToString("MMMM"),
                IntYear = model.BirthDate.Year,
                IntDate = model.BirthDate.Day

            };
            _logger.RequestClients.Add(requestClient);
            _logger.SaveChanges();
            if (model.Files != null)
            {
                foreach (Microsoft.AspNetCore.Http.IFormFile files in model.Files)
                {
                    string filename = model.FirstName + model.LastName + Path.GetExtension(files.FileName);
                    //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
                    string path = Path.Combine("D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\", filename);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new RequestWiseFile();
                    requestWiseFile.FileName = filename;
                    requestWiseFile.RequestId = request.RequestId;
                    requestWiseFile.DocType = 1;
                    _logger.RequestWiseFiles.Add(requestWiseFile);
                    _logger.SaveChanges();
                }
            }


            return "yes";
        }


        public List<PatientDashboardVM> PatientDashboard(int id)
        {


            /*select request_id,count(file_name)
            from request_wise_file
            group by request_id;*/
            var count_file = _logger.RequestWiseFiles
                        .GroupBy(r => r.RequestId)
                        .Select(g => new
                        {
                            RequestId = g.Key,
                            FileName = g.First().FileName,
                            FileCount = g.Count()
                        })
                        .ToList();
            //join it with request table for getting other fields of request table
            var joinedResult = from r in _logger.Requests.ToList()
                               join coun in count_file
                               on r.RequestId equals coun.RequestId
                               into x
                               from coun in x.DefaultIfEmpty()
                               where r.PatientAccountId == id
                               select new
                               {
                                   r,
                                   coun
                               };

            var finalResult = joinedResult.ToList();


            var details = (from datatbl in finalResult
                           join phytbl in _logger.Physicians.ToList()
                           on datatbl.r.PhysicianId equals phytbl.PhysicianId
                           into x
                           from phytbl in x.DefaultIfEmpty()
                           select new
                           {
                               datatbl,
                               phytbl,
                           }).ToList();

            var user2 = _logger.Users.FirstOrDefault(u => u.AspNetUserId == id);

            DateOnly date = new DateOnly(user2.IntYear.Value, DateOnly.ParseExact(user2.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, user2.IntDate.Value);


            List<PatientDashboardVM> dashboard = new List<PatientDashboardVM>();
            foreach (var vm in details)

            {
                var Phy_name = "no data";
                if (vm.phytbl != null)
                {
                    Phy_name = vm.phytbl.FirstName;
                }
                if (vm.datatbl.coun == null)
                {
                    dashboard.Add(new PatientDashboardVM
                    {
                        requestID = vm.datatbl.r.RequestId,
                        CreatedDate = vm.datatbl.r.CreatedDate,
                        Status = vm.datatbl.r.Status,
                        FirstName = vm.datatbl.r.FirstName,
                        count_file = 0,
                        FileName = "",
                        user = user2,
                        PhysicianName = Phy_name,
                        Date = date
                    });
                }
                else
                {
                    dashboard.Add(new PatientDashboardVM
                    {
                        requestID = vm.datatbl.r.RequestId,
                        CreatedDate = vm.datatbl.r.CreatedDate,
                        Status = vm.datatbl.r.Status,
                        FirstName = vm.datatbl.r.FirstName,
                        count_file = vm.datatbl.coun.FileCount,
                        FileName = vm.datatbl.coun.FileName,
                        user = user2,
                        PhysicianName = Phy_name,
                        Date = date
                    });
                }
            };
            return dashboard;
        }

        public List<ViewDocumentVM> PatientViewDocuments(int id)
        {
            var data = (from reqtbl in _logger.Requests.ToList()
                        join doctbl in _logger.RequestWiseFiles.ToList()
                        on reqtbl.RequestId equals doctbl.RequestId
                        where reqtbl.RequestId == id
                        select new
                        {
                            reqtbl,
                            doctbl,
                        }).ToList();

            //var user2 = _logger.Users.FirstOrDefault(u => u.AspNetUserId == id);

            var query = from req in _logger.Requests
                        join usr in _logger.Users on req.UserId equals usr.UserId
                        where req.RequestId == id
                        select new
                        {
                            req.RequestId,
                            req.UserId,
                            usr.FirstName,
                            usr.LastName,
                            usr.Email,
                            usr.Mobile,
                            usr.City,
                            usr.State,
                            usr.ZipCode,
                            usr.Street,
                            usr.StrMonth,
                            usr.IntYear,
                            req.PatientAccountId,
                            usr.IntDate
                        };

            var all_detail = (from da in data
                              join qu in query
                              on da.reqtbl.RequestId equals qu.RequestId
                              select new
                              {
                                  da.reqtbl,
                                  da.doctbl,
                                  qu
                              }).ToList();


            var user2 = _logger.Users.FirstOrDefault(u => u.AspNetUserId == query.FirstOrDefault().PatientAccountId);

            DateOnly date = new DateOnly(user2.IntYear.Value, DateOnly.ParseExact(user2.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, user2.IntDate.Value);

            List<ViewDocumentVM> viewdoc = new List<ViewDocumentVM>();
            foreach (var d in all_detail)
            {
                viewdoc.Add(new ViewDocumentVM
                {
                    FileId = d.doctbl.RequestWiseFileId,
                    AspNetUserId = d.reqtbl.PatientAccountId,
                    RequestId = d.reqtbl.RequestId,
                    PatientName = d.reqtbl.FirstName,
                    File = d.doctbl.FileName,
                    FirstName = d.qu.FirstName,
                    LastName = d.qu.LastName,
                    Email = d.qu.Email,
                    City = d.qu.City,
                    mobile = d.qu.Mobile,
                    State = d.qu.State,
                    ZipCode = d.qu.ZipCode,
                    Street = d.qu.Street,
                    UploadDate = d.doctbl.CreatedDate,
                    Date = date,
                    user = user2

                });
            }
            return viewdoc;
        }


        public string PatientFamilyFriendForm(PatientFamilyFriendInfo model)
        {
            AspNetUser aspnetuser = _logger.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);
            User user = _logger.Users.FirstOrDefault(u => u.Email == model.Email);

            Request request = new Request
            {

                FirstName = model.FFFirstName,
                LastName = model.FFLastName,
                PhoneNumber = model.FFPhoneNumber,
                Email = model.FFEmail,
                RelationName = model.FFRelation,
                CreatedDate = DateTime.Now,
                Status = 1,
                User = user,
            };
            if (user != null)
            {
                request.UserId = user.UserId;
            }
            _logger.Requests.Add(request);
            _logger.SaveChanges();

            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.City
            };
            _logger.Regions.Add(region);
            _logger.SaveChanges();

            RequestClient requestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.City,
                Address = model.Room,
                RegionId = region.RegionId,
                Email = model.Email,
                IntDate = model.BirthDate.Day,
                IntYear = model.BirthDate.Year,
                StrMonth = model.BirthDate.ToString("MMMM"),
                Notes = model.Notes,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,

            };
            _logger.RequestClients.Add(requestClient);
            _logger.SaveChanges();

            if (model.Files != null)
            {
                foreach (IFormFile files in model.Files)
                {
                    string filename = model.FirstName + model.LastName + Path.GetExtension(files.FileName);
                    //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
                    string path = Path.Combine("D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\", filename);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new RequestWiseFile();
                    requestWiseFile.FileName = filename;
                    requestWiseFile.RequestId = request.RequestId;
                    requestWiseFile.DocType = 1;
                    _logger.RequestWiseFiles.Add(requestWiseFile);
                    _logger.SaveChanges();
                }
            };

            return "yes";
        }

        public string PatientConciergeForm(PatientConciergeInfo model)
        {
            AspNetUser aspnetuser = _logger.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);
            User user = _logger.Users.FirstOrDefault(u => u.Email == model.Email);


            Request request = new Request
            {

                FirstName = model.CFirstName,
                LastName = model.CLastName,
                PhoneNumber = model.CPhoneNumber,
                Email = model.CEmail,
                RelationName = model.CHotelName,
                CreatedDate = DateTime.Now,
                Status = 1,
                User = user,
            };
            _logger.Requests.Add(request);
            _logger.SaveChanges();

            Region region = new Region
            {
                Name = model.CState,
                Abbreviation = model.CCity
            };
            _logger.Regions.Add(region);
            _logger.SaveChanges();
            RequestClient requestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.CCity,
                Address = model.Room,
                RegionId = region.RegionId,
                Email = model.Email,
                IntDate = model.BirthDate.Day,
                IntYear = model.BirthDate.Year,
                StrMonth = model.BirthDate.ToString("MMMM"),
                Notes = model.Notes,
                State = model.CState,
                City = model.CCity,
                Street = model.CStreet,
                ZipCode = model.CZipCode,

            };
            _logger.RequestClients.Add(requestClient);
            _logger.SaveChanges();

            Concierge concierge = new Concierge
            {
                ConciergeName = model.CFirstName + " " + model.CLastName,
                Address = model.CCity,
                Street = model.CStreet,
                City = model.CCity,
                State = model.CState,
                ZipCode = model.CZipCode,
                CreatedDate = DateTime.Now,
                RegionId = region.RegionId

            };
            _logger.Concierges.Add(concierge);
            _logger.SaveChanges();
            RequestConcierge request1 = new RequestConcierge
            {
                RequestId = request.RequestId,
                ConciergeId = concierge.ConciergeId
            };
            _logger.RequestConcierges.Add(request1);
            _logger.SaveChanges();
            return "yes";
        }

        public string PatientBusinessForm(PatientBusinessInfo model)
        {

            AspNetUser aspnetuser = _logger.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);
            User user = _logger.Users.FirstOrDefault(u => u.Email == model.Email);


            Request request = new Request
            {

                FirstName = model.BFirstName,
                LastName = model.BLastName,
                PhoneNumber = model.BPhoneNumber,
                Email = model.BEmail,
                RelationName = model.BBusinessName,
                CreatedDate = DateTime.Now,
                Status = 1,
                User = user,
            };
            _logger.Requests.Add(request);
            _logger.SaveChanges();

            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.City
            };

            _logger.Regions.Add(region);
            _logger.SaveChanges();

            Business business = new Business
            {
                Name = model.BFirstName,
                RegionId = region.RegionId,
                PhoneNumber = model.BPhoneNumber,
                CreatedDate = DateTime.Now

            };
            _logger.Businesses.Add(business);
            _logger.SaveChanges();

            RequestBusiness requestBusiness = new RequestBusiness
            {
                RequestId = request.RequestId,
                BusinessId = business.BusinessId
            };
            _logger.RequestBusinesses.Add(requestBusiness);
            _logger.SaveChanges();

            return "yes";
        }
        public RequestWiseFile Download(int id)
        {
            return _logger.RequestWiseFiles.Find(id);
        }

        public List<RequestWiseFile> DownloadAll(int id)
        {
            return _logger.RequestWiseFiles.Where(x => x.RequestId == id).ToList();
        }
        public string Update(int id, ViewDocumentVM vm)
        {

            var use = _logger.Users.FirstOrDefault(u => u.AspNetUserId == id);
            var asp_net_u = _logger.AspNetUsers.FirstOrDefault(asp => asp.Id == use.AspNetUserId);
            var req = _logger.Requests.Where(u => u.User.AspNetUserId == id).ToList();

            //return View(dashboard);

            use.FirstName = vm.user.FirstName;
            use.LastName = vm.user.LastName;
            use.Email = vm.user.Email;
            use.Mobile = vm.user.Mobile;
            use.Street = vm.user.Street;
            use.City = vm.user.City;
            use.State = vm.user.State;
            use.ZipCode = vm.user.ZipCode;


            _logger.Users.Update(use);
            _logger.SaveChanges();

            asp_net_u.UserName = vm.user.FirstName + vm.user.LastName;
            asp_net_u.Email = vm.user.Email;
            asp_net_u.PhoneNumber = vm.user.Mobile;
            _logger.AspNetUsers.Update(asp_net_u);
            _logger.SaveChanges();

            foreach (var re in req)
            {
                re.FirstName = vm.user.FirstName;
                re.LastName = vm.user.LastName;
                re.PhoneNumber = vm.user.Mobile;
                re.Email = vm.user.Email;
                _logger.Requests.Update(re);
                _logger.SaveChanges();

                var reqclient = _logger.RequestClients.FirstOrDefault(m => m.RequestId == re.RequestId);
                if (reqclient != null)
                {
                    reqclient.FirstName = vm.user.FirstName;
                    reqclient.LastName = vm.user.LastName;
                    reqclient.PhoneNumber = vm.user.Mobile;

                    reqclient.Address = vm.user.Street + " , " + vm.user.City + " , " + vm.user.State;
                    reqclient.Email = vm.user.Email;
                    reqclient.Street = vm.user.Street;
                    reqclient.City = vm.user.City;
                    reqclient.State = vm.user.State;
                    reqclient.ZipCode = vm.user.ZipCode;
                    _logger.RequestClients.Update(reqclient);
                    _logger.SaveChanges();
                }
            }

            return "yes";
        }
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
        public PatientInfo PatientMeRequest(int id, PatientInfo model)
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

        //------------------------------------------------------

        public GetAuthenticationVM AuthenticateUser(GetAuthenticationVM test)
        {
            var user = _logger.AspNetUsers.First(u => u.Email == test.Email);
            test.Passwordhashed = user.PasswordHash;
            test.Id = user.Id;
            return test;
        }

        //public GetDashboardVM GetPatientDashboard(GetDashboardVM model)
        //{
        //    var count_file= _logger.RequestWiseFiles
        //                .GroupBy(r => r.RequestId)
        //                .Select(g => new
        //                {
        //                    RequestId = g.Key,
        //                    FileName = g.First().FileName,
        //                    FileCount = g.Count()
        //                })
        //                .ToList();
        //    var joinedResult = from r in _logger.Requests.ToList()
        //                       join coun in count_file
        //                       on r.RequestId equals coun.RequestId
        //                       into x
        //                       from coun in x.DefaultIfEmpty()
        //                       where r.PatientAccountId == model.user_details.AspNetUserId
        //                       select new
        //                       {
        //                           r,
        //                           coun
        //                       };

        //    var finalResult = joinedResult.ToList();


        //    var details = (from datatbl in finalResult
        //                   join phytbl in _logger.Physicians.ToList()
        //                   on datatbl.r.PhysicianId equals phytbl.PhysicianId
        //                   into x
        //                   from phytbl in x.DefaultIfEmpty()
        //                   select new
        //                   {
        //                       datatbl,
        //                       phytbl,
        //                   }).ToList();

        //    var user2 = _logger.Users.FirstOrDefault(u => u.AspNetUserId == model.user_details.AspNetUserId);

        //    DateOnly date = new DateOnly(user2.IntYear.Value, DateOnly.ParseExact(user2.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, user2.IntDate.Value);
        //    model.user_details = user2;
        //    model.details = details;
        //    model.date = date;

        //    return  model;

        //}
        public GetAuthenticationVM ValidateEmail(GetAuthenticationVM test)
        {
            var user = _logger.AspNetUsers.FirstOrDefault(u => u.Email == test.Email);
            test.Id = user.Id;
            return test;
        }

        public GetAllFormsVM GetAllForms(GetAllFormsVM form)
        {
            var aspuser = _logger.AspNetUsers.FirstOrDefault(u => u.Email == form.Email);
            var user1 = _logger.Users.FirstOrDefault(u => u.AspNetUserId == form.aspnetuser.Id);
            form.aspnetuser = aspuser;
            form.user = user1;
            return form;
        }
        public void ADDAspNetUser(AspNetUser aspNetUser)
        {
            _logger.AspNetUsers.Add(aspNetUser);
        }
        public void ADDRegion(Region region)
        {
            _logger.Regions.Add(region);
        }
        public void ADDUser(User user)
        {
            _logger.Users.Add(user);
        }
        public void SaveChanges()
        {
            _logger.SaveChanges();
        }

    }
}
