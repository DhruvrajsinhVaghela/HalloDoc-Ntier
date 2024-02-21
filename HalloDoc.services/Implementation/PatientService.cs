﻿using HalloDoc.DbEntity.Models;
using HalloDoc.Repositories.Interfaces;
using HalloDoc.services.Interface;
using HalloDoc.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Xml.Linq;

namespace HalloDoc.services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatient _repo;

        public PatientService(IPatient repo)
        {
            _repo = repo;
        }

        public string PatientInfoForm(PatientInfo model)
        {
            AspNetUser aspnetuser = _repo.GetAsp(model.Email); 

            if (aspnetuser == null)
            {
                AspNetUser aspnetuser1 = new AspNetUser
                {

                    UserName = model.FirstName + "_" + model.LastName,
                    Email = model.Email,
                    PasswordHash = model.FirstName,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now //here,modified Date,modified By coulmns-remaining to add    
                };
                aspnetuser = aspnetuser1;
                if (model.Password != null)
                {
                    aspnetuser.PasswordHash = Crypto.HashPassword(model.Password);
                }
                _repo.SaveAspNetUser(aspnetuser);

            }
            //region is first here because when we add region id foreign key in user table it will show error if object is not created 
            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.State.Substring(0, 3)
            };
            _repo.SaveRegion(region);

            User user1 =_repo.GetUser(model.Email);
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
                _repo.SaveUser(user1);

            }
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
            _repo.SaveRequest(request);


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
            _repo.SaveRequestClient(requestClient);

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
                    _repo.SaveRequestWiseFile(requestWiseFile);
                }
            }


            return "yes";
        }

        public int PatientLogin(AspNetUser aspNetUser)
        {
            AspNetUser user = _repo.GetAsp(aspNetUser.Email);

            if (user != null && Crypto.VerifyHashedPassword(user.PasswordHash, aspNetUser.PasswordHash))//in crypto. method (hash password,plain text)
            {
                return user.Id;
            }
            return 0;
        }

        public bool validate_Email(string email)
        {
            var user = _repo.GetAsp(email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public string PatientFamilyFriendForm(PatientFamilyFriendInfo model)
        {

            AspNetUser aspnetuser = _repo.GetAsp(model.Email);
            User user =_repo.GetUser(model.Email);

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
            _repo.SaveRequest(request);

            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.City
            };
            _repo.SaveRegion(region);

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
            _repo.SaveRequestClient(requestClient);

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
                    _repo.SaveRequestWiseFile(requestWiseFile);
                }
            };

            return "yes";
        }

        public string PatientConciergeForm(PatientConciergeInfo model)
        {
            AspNetUser aspnetuser = _repo.GetAsp(model.Email);
            User user = _repo.GetUser(model.Email);


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
            _repo.SaveRequest(request);

            Region region = new Region
            {
                Name = model.CState,
                Abbreviation = model.CCity
            };
            _repo.SaveRegion(region);

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
            _repo.SaveRequestClient(requestClient);

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
            _repo.SaveConcierge(concierge);

            RequestConcierge request1 = new RequestConcierge
            {
                RequestId = request.RequestId,
                ConciergeId = concierge.ConciergeId
            };
            _repo.SaveRequestConcierge(request1);

            return "yes";
        }

        public string PatientBusinessForm(PatientBusinessInfo model)
        {
            AspNetUser aspnetuser = _repo.GetAsp(model.Email);
            User user =_repo.GetUser(model.Email);


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
            _repo.SaveRequest(request);

            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.City
            };

            _repo.SaveRegion(region);

            Business business = new Business
            {
                Name = model.BFirstName,
                RegionId = region.RegionId,
                PhoneNumber = model.BPhoneNumber,
                CreatedDate = DateTime.Now

            };
            _repo.SaveBusiness(business);

            RequestBusiness requestBusiness = new RequestBusiness
            {
                RequestId = request.RequestId,
                BusinessId = business.BusinessId
            };
            _repo.SaveRequestBusiness(requestBusiness);

            return "yes";
        }

        public List<PatientDashboardVM> PatientDashboard(int id)
        {

            /*select request_id,count(file_name)
            from request_wise_file
            group by request_id;*/
            var count_file = _repo.CountFile(id)
                        .GroupBy(r => r.RequestId)
                        .Select(g => new
                        {
                            RequestId = g.Key,
                            FileName = g.First().FileName,
                            FileCount = g.Count()
                        })
                        .ToList();
            //join it with request table for getting other fields of request table
            var joinedResult = from r in _repo.JoinedResult(id)
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
                           join phytbl in _repo.Details(id)
                           on datatbl.r.PhysicianId equals phytbl.PhysicianId
                           into x
                           from phytbl in x.DefaultIfEmpty()
                           select new
                           {
                               datatbl,
                               phytbl,
                           }).ToList();

            var user2 = _repo.GetUserAsp(id);

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
            var data = (from reqtbl in _repo.JoinedResult(id)
                        join doctbl in _repo.CountFile(id)
                        on reqtbl.RequestId equals doctbl.RequestId
                        where reqtbl.RequestId == id
                        select new
                        {
                            reqtbl,
                            doctbl,
                        }).ToList();

            //var user2 = _logger.Users.FirstOrDefault(u => u.AspNetUserId == id);

            var query = from req in _repo.JoinedResult(id)
                        join usr in _repo.use(id) on req.UserId equals usr.UserId
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


            var user2 = _repo.use(id).FirstOrDefault(u => u.AspNetUserId == query.FirstOrDefault().PatientAccountId);

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

        public RequestWiseFile Download(int id)
        {
            return _repo.ReqWiseFileId(id);
        }

        public List<RequestWiseFile> DownloadAll(int id)
        {
            return _repo.ReqWiseFileIdAll(id).Where(x => x.RequestId == id).ToList();
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

                var reqclient =_logger.RequestClients.FirstOrDefault(m => m.RequestId == re.RequestId);
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
    }
}
