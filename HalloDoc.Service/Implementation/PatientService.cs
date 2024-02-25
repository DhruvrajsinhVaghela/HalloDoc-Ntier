using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using HalloDoc.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace HalloDoc.Service.Implementation
{
    public class PatientService:IPatientService
    {
        private readonly IPatient _repo;

        public PatientService(IPatient repo)
        {
            _repo = repo;
        }

        public int PatientLogin(AspNetUser aspNetUser)
        {
            //AspNetUser user = _logger.AspNetUsers.FirstOrDefault(u => u.Email == aspNetUser.Email);
            AspNetUser user = _repo.GetAspUserData(aspNetUser.Email);
            if (user != null && Crypto.VerifyHashedPassword(user.PasswordHash, aspNetUser.PasswordHash))//in crypto. method (hash password,plain text)
            {
                return user.Id;
            }
            return 0;
        }

        public Task<bool> validate_Email(string email)
        {
            //var user = await _repo.AspNetUsers.FirstOrDefaultAsync(u => u.Email == email);
            var user = _repo.GetAspUserData(email);
            if (user == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public string PatientInfoForm(PatientInfo model)
        {
            AspNetUser aspnetuser=_repo.GetAspUserData(model.Email);

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
                _repo.GetAspData().Add(aspnetuser1);
                _repo.GetSaveChanges();

            }
            //region is first here because when we add region id foreign key in user table it will show error if object is not created 
            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.State.Substring(0, 3)
            };
            _repo.GetRegionData().Add(region);
            _repo.GetSaveChanges();

            User user1 = _repo.GetUserUserData(model.Email);
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
                    AspNetUser = aspnetuser ,
                    Status=1//modified_by,modified_date,status,ip,is_request_with_email,is_deleted remaining to add
                };
                user1 = user;
                _repo.GetUserData().Add(user1);
                _repo.GetSaveChanges();
            }
            

            Request request = new Request
            {
                RequestType = 1,//1 for patient 
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CreatedDate = DateTime.Now,
                Status = 1,
                PatientAccountId = aspnetuser.Id,
                UserId = user1.UserId,
                ConfirmationNumber=region.Name.Substring(0,2)+DateTime.Now.ToString().Substring(0,4)
                                    +model.LastName.Substring(0,2)+model.FirstName.Substring(0,2)
                                    +_repo.GetConfirmationNo(),
                //case number is here------
                //physician is here------
            };
            _repo.GetRequestData().Add(request);
            _repo.GetSaveChanges();

           
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
                IntDate = model.BirthDate.Day,
                
            };
            _repo.GetReqClientData().Add(requestClient);
            _repo.GetSaveChanges();
            if (model.Files != null)
            {
                foreach (Microsoft.AspNetCore.Http.IFormFile files in model.Files)
                {
                    string filename = model.FirstName + model.LastName + Path.GetExtension(files.FileName);
                    //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
                    string path = Path.Combine("D:\\project\\HalloDoc-Ntier\\HalloDoc-Ntier\\HalloDoc\\UploadFiles\\", filename);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new RequestWiseFile();
                    requestWiseFile.FileName = filename;
                    requestWiseFile.RequestId = request.RequestId;
                    requestWiseFile.DocType = 1;
                    _repo.GetReqWisFileData().Add(requestWiseFile);
                    _repo.GetSaveChanges();
                }
            }

            return "yes";
        }

        public string PatientFamilyFriendForm(PatientFamilyFriendInfo model)
        {
            AspNetUser aspnetuser = _repo.GetAspUserData(model.Email);
            User user = _repo.GetUserUserData(model.Email);

            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.City
            };
            _repo.GetRegionData().Add(region);
            _repo.GetSaveChanges();

            Request request = new Request
            {
                RequestType=2,
                FirstName = model.FFFirstName,
                LastName = model.FFLastName,
                PhoneNumber = model.FFPhoneNumber,
                Email = model.FFEmail,
                RelationName = model.FFRelation,
                CreatedDate = DateTime.Now,
                Status = 1,
                User = user,
                ConfirmationNumber = region.Name.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 4)
                                    + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2)
                                    + _repo.GetConfirmationNo(),
            };
            if (user != null)
            {
                request.UserId = user.UserId;
            }
            _repo.GetRequestData().Add(request);
            _repo.GetSaveChanges();

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
            _repo.GetReqClientData().Add(requestClient);
            _repo.GetSaveChanges();

            if (model.Files != null)
            {
                foreach (IFormFile files in model.Files)
                {
                    string filename = model.FirstName + model.LastName + Path.GetExtension(files.FileName);
                    //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
                    string path = Path.Combine("D:\\project\\HalloDoc-Ntier\\HalloDoc-Ntier\\HalloDoc\\UploadFiles\\", filename);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new RequestWiseFile();
                    requestWiseFile.FileName = filename;
                    requestWiseFile.RequestId = request.RequestId;
                    requestWiseFile.DocType = 1;
                    _repo.GetReqWisFileData().Add(requestWiseFile);
                    _repo.GetSaveChanges();
                }
            };

            return "yes";
        }

        public string PatientConciergeForm(PatientConciergeInfo model)
        {
            AspNetUser aspnetuser = _repo.GetAspUserData(model.Email);
            User user = _repo.GetUserUserData(model.Email);

            Region region = new Region
            {
                Name = model.CState,
                Abbreviation = model.CCity
            };
            _repo.GetRegionData().Add(region);
            _repo.GetSaveChanges();

            Request request = new Request
            {
                RequestType=3,
                FirstName = model.CFirstName,
                LastName = model.CLastName,
                PhoneNumber = model.CPhoneNumber,
                Email = model.CEmail,
                RelationName = model.CHotelName,
                CreatedDate = DateTime.Now,
                Status = 1,
                User = user,
                ConfirmationNumber = region.Name.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 4)
                                    + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2)
                                    + _repo.GetConfirmationNo(),

            };
            _repo.GetRequestData().Add(request);
            _repo.GetSaveChanges();

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
            _repo.GetReqClientData().Add(requestClient);
            _repo.GetSaveChanges();

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
            _repo.GetConciergeData().Add(concierge);
            _repo.GetSaveChanges();
            RequestConcierge request1 = new RequestConcierge
            {
                RequestId = request.RequestId,
                ConciergeId = concierge.ConciergeId
            };
            _repo.GetReqConciergeData().Add(request1);
            _repo.GetSaveChanges();
            return "yes";
        }

        public string PatientBusinessForm(PatientBusinessInfo model)
        {
            AspNetUser aspnetuser = _repo.GetAspUserData(model.Email);
            User user = _repo.GetUserUserData(model.Email);

            Region region = new Region
            {
                Name = model.State,
                Abbreviation = model.City
            };

            _repo.GetRegionData().Add(region);
            _repo.GetSaveChanges();

            Request request = new Request
            {
                RequestType=4,
                FirstName = model.BFirstName,
                LastName = model.BLastName,
                PhoneNumber = model.BPhoneNumber,
                Email = model.BEmail,
                RelationName = model.BBusinessName,
                CreatedDate = DateTime.Now,
                Status = 1,
                User = user,
                ConfirmationNumber = region.Name.Substring(0, 2) + DateTime.Now.ToString("ddmm").Substring(0, 4)
                    + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2)
                    + _repo.GetConfirmationNo(),
            };
            _repo.GetRequestData().Add(request);
            _repo.GetSaveChanges();

            Business business = new Business
            {
                Name = model.BFirstName,
                RegionId = region.RegionId,
                PhoneNumber = model.BPhoneNumber,
                CreatedDate = DateTime.Now

            };
            _repo.GetBusinessData().Add(business);
            _repo.GetSaveChanges();

            RequestBusiness requestBusiness = new RequestBusiness
            {
                RequestId = request.RequestId,
                BusinessId = business.BusinessId
            };
            _repo.GetReqBusinessData().Add(requestBusiness);
            _repo.GetSaveChanges();

            return "yes";
        }

        public List<PatientDashboardVM> PatientDashboard(int id)
        {


            /*select request_id,count(file_name)
            from request_wise_file
            group by request_id;*/
            var count_file = _repo.GetReqWisFileData()
                        .GroupBy(r => r.RequestId)
                        .Select(g => new
                        {
                            RequestId = g.Key,
                            FileName = g.First().FileName,
                            FileCount = g.Count()
                        })
                        .ToList();
            //join it with request table for getting other fields of request table
            var joinedResult = from r in _repo.GetRequestData().ToList()
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
                           join phytbl in _repo.GetPhysicianData().ToList()
                           on datatbl.r.PhysicianId equals phytbl.PhysicianId
                           into x
                           from phytbl in x.DefaultIfEmpty()
                           select new
                           {
                               datatbl,
                               phytbl,
                           }).ToList();

            var user2 = _repo.GetUserAspIdData(id);

            DateOnly? date = new DateOnly(user2.IntYear.Value, DateOnly.ParseExact(user2.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, user2.IntDate.Value);


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
            var data = (from reqtbl in _repo.GetRequestData().ToList()
                        join doctbl in _repo.GetReqWisFileData().ToList()
                        on reqtbl.RequestId equals doctbl.RequestId
                        where reqtbl.RequestId == id
                        select new
                        {
                            reqtbl,
                            doctbl,
                        }).ToList();

            //var user2 = _logger.Users.FirstOrDefault(u => u.AspNetUserId == id);

            var query = from req in _repo.GetRequestData()
                        join usr in _repo.GetUserData() on req.UserId equals usr.UserId
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


            var user2 = _repo.GetUserData().FirstOrDefault(u => u.AspNetUserId == query.FirstOrDefault().PatientAccountId);

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
                    user = user2,
                    ConfirmationNo=d.reqtbl.ConfirmationNumber

                });
            }
            return viewdoc;
        }
        public RequestWiseFile Download(int id)
        {
            return _repo.Download(id);
        }

        public List<RequestWiseFile> DownloadAll(int id)
        {
            return _repo.DownloadAll(id);
        }

        public string Update(int id, ViewDocumentVM vm)
        {
            var use = _repo.GetUserAspIdData(id);
            var asp_net_u = _repo.GetAspData().FirstOrDefault(asp => asp.Id == use.AspNetUserId);
            var req = _repo.GetRequestData().Where(u => u.User.AspNetUserId == id).ToList();

            //return View(dashboard);

            use.FirstName = vm.user.FirstName;
            use.LastName = vm.user.LastName;
            use.Email = vm.user.Email;
            use.Mobile = vm.user.Mobile;
            use.Street = vm.user.Street;
            use.City = vm.user.City;
            use.State = vm.user.State;
            use.ZipCode = vm.user.ZipCode;


            _repo.GetUserData().Update(use);
            _repo.GetSaveChanges();

            asp_net_u.UserName = vm.user.FirstName + vm.user.LastName;
            asp_net_u.Email = vm.user.Email;
            asp_net_u.PhoneNumber = vm.user.Mobile;
            _repo.GetAspData().Update(asp_net_u);
            _repo.GetSaveChanges();

            foreach (var re in req)
            {
                re.FirstName = vm.user.FirstName;
                re.LastName = vm.user.LastName;
                re.PhoneNumber = vm.user.Mobile;
                re.Email = vm.user.Email;
                _repo.GetRequestData().Update(re);
                _repo.GetSaveChanges();

                var reqclient = _repo.GetReqClientData().FirstOrDefault(m => m.RequestId == re.RequestId);
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
                    _repo.GetReqClientData().Update(reqclient);
                    _repo.GetSaveChanges();
                }
            }

            return "yes";
        }

        public string PatientFileSave(int id, PatientDashboardVM model)
        {
            Request user1 =_repo.GetRequestData().FirstOrDefault(u => u.RequestId == id);
            Request request = new Request { };
            if (model.View.UploadFile != null)
            {
                foreach (IFormFile files in model.View.UploadFile)
                {
                    string filename = user1.FirstName + user1.LastName + Path.GetExtension(files.FileName);
                    string path = Path.Combine("D:\\project\\HalloDoc-Ntier\\HalloDoc-Ntier\\HalloDoc\\UploadFiles\\", filename);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new RequestWiseFile();
                    requestWiseFile.FileName = filename;
                    requestWiseFile.RequestId = user1.RequestId;
                    requestWiseFile.DocType = 1;
                    _repo.GetReqWisFileData().Add(requestWiseFile);
                    _repo.GetSaveChanges();
                }
            }

            return "yes";
        }

        public PatientInfo PatientMeRequest(int id, PatientInfo model)
        {
            //var userid = HttpContext.Session.GetInt32("userId");
            var userobj = _repo.GetUserUserIdData(id);
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
            var userobj = _repo.GetUserUserIdData(id);

            var request =_repo.GetRequestData().FirstOrDefault(m => m.Email == model.Email);


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

            _repo.GetRequestData().Add(req);
            _repo.GetSaveChanges();


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
                _repo.GetReqWisFileData().Add(requestWiseFile);
                _repo.GetSaveChanges();
            }

            Region reg = new Region()
            {
                Name = model.State,
                Abbreviation = model.State
            };
            _repo.GetRegionData().Add(reg);
            _repo.GetSaveChanges();

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
            _repo.GetReqClientData().Add(reqClient);
            _repo.GetSaveChanges();


            return model;
        }


    }
}
