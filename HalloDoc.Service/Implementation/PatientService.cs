using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using HalloDoc.Service.Interface;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using HalloDoc.Services.Interfaces;
using System.Data;

namespace HalloDoc.Service.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatient _repo;
        //private readonly IJwtToken _token;

        public PatientService(IPatient repo, IJwtToken token)
        {
            _repo = repo;
            //_token = token;
        }
        readonly Dictionary<string, string> stateAbbreviations = new()
            {
            {"ANDHRA PRADESH", "AP"},
            {"ARUNACHAL PRADESH", "AR"},
            {"ASSAM", "AS"},
            {"BIHAR", "BR"},
            {"CHHATTISGARH", "CG"},
            {"GOA", "GA"},
            {"GUJARAT", "GJ"},
            {"HARYANA", "HR"},
            {"HIMACHAL PRADESH", "HP"},
            {"JHARKHAND", "JH"},
            {"KARNATAKA", "KA"},
            {"KERALA", "KL"},
            {"MADHYA PRADESH", "MP"},
            {"MAHARASHTRA", "MH"},
            {"MANIPUR", "MN"},
            {"MEGHALAYA", "ML"},
            {"MIZORAM", "MZ"},
            {"NAGALAND", "NL"},
            {"ODISHA", "OD"},
            {"PUNJAB", "PB"},
            {"RAJASTHAN", "RJ"},
            {"SIKKIM", "SK"},
            {"TAMIL NADU", "TN"},
            {"TELANGANA", "TG"},
            {"TRIPURA", "TR"},
            {"UTTAR PRADESH", "UP"},
            {"UTTARAKHAND", "UK"},
            {"WEST BENGAL", "WB"},
            {"ANDAMAN AND NICOBAR ISLANDS", "AN"},
            {"CHANDIGARH", "CH"},
            {"DADRA AND NAGAR HAVELI AND DAMAN AND DIU", "DN"},
            {"LAKSHADWEEP", "LD"},
            {"DELHI", "DL"},
            {"PUDUCHERRY", "PY"}
            };
        public AspNetUser PatientLogin(AspNetUser aspNetUser)
        {
            AspNetUser user = _repo.GetAspUserData(aspNetUser.Email ?? "");
            if (user.Id != 0 && Crypto.VerifyHashedPassword(user.PasswordHash, aspNetUser.PasswordHash))//in crypto. method (hash password,plain text)
            {
                return user;
            }
            return new AspNetUser();
        }

        public Task<bool> validate_Email(string email)
        {
            //var user = await _repo.AspNetUsers.FirstOrDefaultAsync(u => u.Email == email);
            var user = _repo.GetAspUserData(email);
            if (user.Id == 0)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public string PatientInfoForm(PatientInfoVM model)
        {
            AspNetUser aspnetuser = _repo.GetAspUserData(model.Email);

            if (aspnetuser.Id == 0)
            {

                AspNetUser aspnetuser1 = new()
                {

                    UserName = model.FirstName + " " + model.LastName,
                    Email = model.Email,
                    PasswordHash = model.FirstName,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now, //here,modified Date,modified By coulmns-remaining to add
                    Roles = _repo.GetPatientRole()
                };
                aspnetuser = aspnetuser1;
                if (model.Password != null)
                {
                    aspnetuser.PasswordHash = Crypto.HashPassword(model.Password);
                }
                _repo.AddAspNetUser(aspnetuser1);

            }


            var reg = _repo.GetUniqueRegion(model.State);
            if (reg.RegionId == 0)
            {
                Region region = new()
                {
                    Name = model.State.ToUpper(),
                    Abbreviation = stateAbbreviations[model.State.ToUpper()]
                };
                _repo.AddRegion(region);
                reg = region;
            }


            User user1 = _repo.GetUserUserData(model.Email);
            if (user1.UserId == 0)
            {
                User user = new()
                {
                    AspNetUserId = aspnetuser.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email??"",
                    Mobile = model.PhoneNumber,
                    ZipCode = model.ZipCode,
                    State = model.State,
                    City = model.City,
                    Street = model.Street,
                    IntDate = model.BirthDate.Day,
                    IntYear = model.BirthDate.Year,
                    StrMonth = model.BirthDate.ToString("MMMM"),
                    CreatedDate = DateTime.Now,
                    RegionId = reg.RegionId,
                    CreatedBy = model.FirstName,
                    AspNetUser = aspnetuser,
                    Status = 1//modified_by,modified_date,status,ip,is_request_with_email,is_deleted remaining to add
                };
                user1 = user;
                _repo.AddUser(user1);

            }


            Request request = new()
            {
                RequestType = 2,//2 for patient 
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email??"",
                CreatedDate = DateTime.Now,
                Status = 1,
                PatientAccountId = aspnetuser.Id,
                UserId = user1.UserId,

                ConfirmationNumber = reg.Abbreviation?.Substring(0, 2) + DateTime.Now.ToString("ddMMyyyy").Substring(0, 4)
                                    + model.LastName[..2] + model.FirstName[..2]
                                    + _repo.GetConfirmationNo(),
                //case number is here------ 
                //physician is here------
            };
            _repo.AddRequest(request);


            RequestClient requestClient = new()
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.Room,
                Address = model.State,
                RegionId = reg.RegionId,
                Email = model.Email??"",
                Notes = model.Notes,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                StrMonth = model.BirthDate.ToString("MMMM"),
                IntYear = model.BirthDate.Year,
                IntDate = model.BirthDate.Day,

            };
            _repo.AddReqClient(requestClient);
            if (model.Files != null)
            {
                foreach (IFormFile files in model.Files)
                {
                    string filename = model.FirstName + model.LastName + Path.GetExtension(files.FileName);
                    string path = Path.Combine("D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\", filename);
                    using (FileStream stream = new(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new()
                    {
                        FileName = filename,
                        RequestId = request.RequestId,
                        DocType = 1,
                        CreatedDate = DateTime.Now
                    };
                    _repo.AddReqWisFile(requestWiseFile);
                }
            }

            return "yes";
        }

        public string PatientFamilyFriendForm(PatientFamilyFriendInfoVM model)
        {
            AspNetUser aspnetuser = _repo.GetAspUserData(model.Email);
            if (aspnetuser.Id == 0)
            {
                bool isRegistered = _repo.GetEmail(model.Email);
                if (!isRegistered)
                {
                    var receiver = model.Email ?? "";
                    var subject = "Create Account";
                    var message = "Tap on link for Create Account: http://localhost:5093/Login/PatientCreateAccount";

                    var mail = "tatva.dotnet.dhruvrajsinhvaghela@outlook.com";
                    var password = "Vagheladhruv@123";

                    var client = new SmtpClient("smtp.office365.com")
                    {
                        Port = 587,
                        EnableSsl = true,
                        Credentials = new NetworkCredential(mail, password)
                    };

                    client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
                }


            }
            User user = _repo.GetUserUserData(model.Email??"");

            var reg = _repo.GetUniqueRegion(model.State);
            if (reg.RegionId == 0)
            {
                Region region = new()
                {
                    Name = model.State.ToUpper(),
                    Abbreviation = stateAbbreviations[model.State.ToUpper()]
                };
                _repo.AddRegion(region);
                reg = region;
            }

            Request request = new()
            {
                RequestType = 3,
                FirstName = model.FFFirstName,
                LastName = model.FFLastName,
                PhoneNumber = model.FFPhoneNumber,
                Email = model.FFEmail,
                RelationName = model.FFRelation,
                CreatedDate = DateTime.Now,
                Status = 1,
                ConfirmationNumber = reg.Abbreviation?.Substring(0, 2) + DateTime.Now.ToString("ddMMyyyy").Substring(0, 4)
                                    + model.LastName[..2] + model.FirstName[..2]
                                    + _repo.GetConfirmationNo(),
                PatientAccountId = aspnetuser.Id,
            };
            if (user.UserId != 0)
            {
                request.UserId = user.UserId;
                request.User = user;
            }
            _repo.AddRequest(request);

            RequestClient requestClient = new()
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.Room,
                Address = model.State,
                RegionId = reg.RegionId,
                Email = model.Email??"",
                IntDate = model.BirthDate.Day,
                IntYear = model.BirthDate.Year,
                StrMonth = model.BirthDate.ToString("MMMM"),
                Notes = model.Notes,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,

            };
            _repo.AddReqClient(requestClient);

            if (model.Files != null)
            {
                foreach (IFormFile files in model.Files)
                {
                    string filename = model.FirstName + model.LastName + Path.GetExtension(files.FileName);
                    string path = Path.Combine("D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\", filename);
                    using (FileStream stream = new(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new()
                    {
                        FileName = filename,
                        RequestId = request.RequestId,
                        DocType = 1
                    };
                    _repo.AddReqWisFile(requestWiseFile);
                }
            };

            return "yes";
        }

        public string PatientConciergeForm(PatientConciergeInfoVM model)
        {
            AspNetUser aspnetuser = _repo.GetAspUserData(model.Email);
            if (aspnetuser.Id == 0)
            {
                bool isRegistered = _repo.GetEmail(model.Email);
                if (!isRegistered)
                {
                    var receiver = model.Email ?? "";
                    var subject = "Create Account";
                    var message = "Tap on link for Create Account: http://localhost:5093/Login/PatientCreateAccount";


                    var mail = "tatva.dotnet.dhruvrajsinhvaghela@outlook.com";
                    var password = "Vagheladhruv@123";

                    var client = new SmtpClient("smtp.office365.com")
                    {
                        Port = 587,
                        EnableSsl = true,
                        Credentials = new NetworkCredential(mail, password)
                    };

                    client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
                }


            }
            User user = _repo.GetUserUserData(model.Email??"");

            var reg = _repo.GetUniqueRegion(model.CState);
            if (reg == null)
            {
                Region region = new()
                {
                    Name = model.CState.ToUpper(),
                    Abbreviation = stateAbbreviations[model.CState.ToUpper()]
                };
                _repo.AddRegion(region);
                reg = region;
            }

            Request request = new()
            {
                RequestType = 4,
                FirstName = model.CFirstName,
                LastName = model.CLastName,
                PhoneNumber = model.CPhoneNumber,
                Email = model.CEmail,
                RelationName = model.CHotelName,
                CreatedDate = DateTime.Now,
                Status = 1,
                ConfirmationNumber = reg.Abbreviation?.Substring(0, 2) + DateTime.Now.ToString("ddMMyyyy").Substring(0, 4)
                                    + model.LastName[..2] + model.FirstName[..2]
                                    + _repo.GetConfirmationNo(),
                PatientAccountId = aspnetuser.Id,

            };
            if (user.UserId != 0)
            {
                request.UserId = user.UserId;
                request.User = user;
            }
            _repo.AddRequest(request);

            RequestClient requestClient = new()
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.Room,
                Address = model.CState,
                RegionId = reg.RegionId,
                Email = model.Email??"",
                IntDate = model.BirthDate.Day,
                IntYear = model.BirthDate.Year,
                StrMonth = model.BirthDate.ToString("MMMM"),
                Notes = model.Notes,
                State = model.CState,
                City = model.CCity,
                Street = model.CStreet,
                ZipCode = model.CZipCode,

            };
            _repo.AddReqClient(requestClient);

            Concierge concierge = new()
            {
                ConciergeName = model.CFirstName + " " + model.CLastName,
                Address = model.CCity,
                Street = model.CStreet,
                City = model.CCity,
                State = model.CState,
                ZipCode = model.CZipCode,
                CreatedDate = DateTime.Now,
                RegionId = reg.RegionId

            };
            _repo.AddConcierge(concierge);
            RequestConcierge request1 = new()
            {
                RequestId = request.RequestId,
                ConciergeId = concierge.ConciergeId
            };
            _repo.AddReqConcierge(request1);
            return "yes";
        }

        public string PatientBusinessForm(PatientBusinessInfoVM model)
        {
            AspNetUser aspnetuser = _repo.GetAspUserData(model.Email);
            if (aspnetuser.Id == 0)
            {
                bool isRegistered = _repo.GetEmail(model.Email);
                if (!isRegistered)
                {
                    var receiver = model.Email ?? "";
                    var subject = "Create Account";
                    //var token = _token.GenerateJwtToken;
                    var message = "Tap on link for Create Account: http://localhost:5093/Login/PatientCreateAccount";


                    var mail = "tatva.dotnet.dhruvrajsinhvaghela@outlook.com";
                    var password = "Vagheladhruv@123";

                    var client = new SmtpClient("smtp.office365.com")
                    {
                        Port = 587,
                        EnableSsl = true,
                        Credentials = new NetworkCredential(mail, password)
                    };

                    client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
                }


            }
            User user = _repo.GetUserAspIdData(aspnetuser.Id);

            var reg = _repo.GetUniqueRegion(model.State);
            if (reg.RegionId == 0)
            {

                Region region = new()
                {
                    Name = model.State.ToUpper(),
                    Abbreviation = stateAbbreviations[model.State.ToUpper()]
                };
                _repo.AddRegion(region);
                reg = region;
            }

            Request request = new()
            {
                RequestType = 1,
                FirstName = model.BFirstName,
                LastName = model.BLastName,
                PhoneNumber = model.BPhoneNumber,
                Email = model.BEmail,
                RelationName = model.BBusinessName,
                CreatedDate = DateTime.Now,
                Status = 1,
                ConfirmationNumber = reg.Abbreviation?.Substring(0, 2) + DateTime.Now.ToString("ddMMyyyy").Substring(0, 4)
                                    + model.LastName[..2] + model.FirstName[..2]
                                    + _repo.GetConfirmationNo(),
                PatientAccountId = aspnetuser.Id,
            };
            if (user.UserId != 0)
            {
                request.UserId = user.UserId;
                request.User = user;
            }
            _repo.AddRequest(request);

            RequestClient requestClient = new()
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.Room,
                Address = model.State,
                RegionId = reg.RegionId,
                Email = model.Email??"",
                IntDate = model.BirthDate.Day,
                IntYear = model.BirthDate.Year,
                StrMonth = model.BirthDate.ToString("MMMM"),
                Notes = model.Notes,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,

            };
            _repo.AddReqClient(requestClient);
            Business business = new()
            {
                Name = model.BFirstName,
                RegionId = reg.RegionId,
                PhoneNumber = model.BPhoneNumber,
                CreatedDate = DateTime.Now,
            };
            _repo.AddBusiness(business);

            RequestBusiness requestBusiness = new()
            {
                RequestId = request.RequestId,
                BusinessId = business.BusinessId
            };
            _repo.AddReqBusiness(requestBusiness);

            return "yes";
        }

        public List<PatientDashboardVM> PatientDashboard(int id)
        {

            var count_file = _repo.GetReqWisFileData()
                        .GroupBy(r => r.RequestId)
                        .Select(g => new
                        {
                            RequestId = g.Key,
                            g.First().FileName,
                            FileCount = g.Count()
                        })
                        .ToList();
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
            DateOnly? date=DateOnly.FromDateTime(DateTime.Now);
            if(user2.UserId!=0)
            {
                date = new DateOnly(user2.IntYear!.Value, DateOnly.ParseExact(user2.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, user2.IntDate!.Value);
            }


            List<PatientDashboardVM> dashboard = new();
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
                        RequestID = vm.datatbl.r.RequestId,
                        CreatedDate = vm.datatbl.r.CreatedDate,
                        Status = vm.datatbl.r.Status,
                        FirstName = vm.datatbl.r.FirstName,
                        CountFile = 0,
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
                        RequestID = vm.datatbl.r.RequestId,
                        CreatedDate = vm.datatbl.r.CreatedDate,
                        Status = vm.datatbl.r.Status,
                        FirstName = vm.datatbl.r.FirstName,
                        CountFile = vm.datatbl.coun.FileCount,
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


            var user2 = _repo.GetUser(query.First().PatientAccountId);

            DateOnly date = new(user2!.IntYear!.Value, DateOnly.ParseExact(user2!.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, user2!.IntDate!.Value);

            List<ViewDocumentVM> viewdoc = new();
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
                    Mobile = d.qu.Mobile,
                    State = d.qu.State,
                    ZipCode = d.qu.ZipCode,
                    Street = d.qu.Street,
                    UploadDate = d.doctbl.CreatedDate,
                    Date = date,
                    user = user2,
                    ConfirmationNo = d.reqtbl.ConfirmationNumber


                });
            }
            return viewdoc;
        }
        public RequestWiseFile Download(int id)
        {
            return _repo.Download(id) ?? new RequestWiseFile();
        }

        public List<RequestWiseFile> DownloadAll(PatientDashboardVM vm, int id)
        {
            return _repo.DownloadAll(vm, id);
        }

        public string Update(int id, ViewDocumentVM vm)
        {
            var use = _repo.GetUserAspIdData(id);
            var asp_net_u = _repo.GetAspNetUser(use.AspNetUserId);
            var req = _repo.GetRequestData().Where(u => u.User!.AspNetUserId == id).ToList();

            //return View(dashboard);

            use.FirstName = vm.user.FirstName;
            use.LastName = vm.user.LastName;
            use.Email = vm.user.Email;
            use.Mobile = vm.user.Mobile;
            use.Street = vm.user.Street;
            use.City = vm.user.City;
            use.State = vm.user.State;
            use.ZipCode = vm.user.ZipCode;


            _repo.UpdateUser(use);

            if (asp_net_u.Id != 0)
            {
                asp_net_u.UserName = vm.user.FirstName + vm.user.LastName;
                asp_net_u.Email = vm.user.Email;
                asp_net_u.PhoneNumber = vm.user.Mobile;
                _repo.UpdateAspNetUser(asp_net_u);
            }

            foreach (var re in req)
            {
                re.FirstName = vm.user.FirstName;
                re.LastName = vm.user.LastName;
                re.PhoneNumber = vm.user.Mobile;
                re.Email = vm.user.Email;
                _repo.UpdateRequest(re);

                var reqclient = _repo.GetReqClientById(re.RequestId);
                if (reqclient.RequestId != 0)
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
                    _repo.UpdateReqClient(reqclient);
                }
            }

            return "yes";
        }

        public string PatientFileSave(int id, PatientDashboardVM model)
        {
            Request user1 = _repo.GetRequestById(id);
            //Request request = new Request { };
            if (model.View!.UploadFile != null && user1.RequestId != 0)
            {
                foreach (IFormFile files in model.View.UploadFile)
                {
                    string filename = user1.FirstName + user1.LastName + Path.GetExtension(files.FileName);
                    string path = Path.Combine("D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\", filename);
                    using (FileStream stream = new(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new()
                    {
                        FileName = filename,
                        RequestId = user1.RequestId,
                        DocType = 1,
                        CreatedDate = DateTime.Now
                    };
                    _repo.AddReqWisFile(requestWiseFile);
                }
            }

            return "yes";
        }

        public PatientInfoVM PatientMeRequest(int id, PatientInfoVM model)
        {
            //var userid = HttpContext.Session.GetInt32("userId");
            var userobj = _repo.GetUserUserIdData(id);
            DateOnly dateofbirth = new(userobj.IntYear!.Value, DateOnly.ParseExact(userobj.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, userobj.IntDate!.Value);
            ViewDocumentVM dvm = new()
            {
                user = userobj,
                Date = dateofbirth

            };
            PatientInfoVM pr = new()
            {
                Email = dvm.user.Email,
                FirstName = dvm.user.FirstName,
                LastName = dvm.user.LastName ?? "",
                PhoneNumber = dvm.user.Mobile ?? "",
                ZipCode = dvm.user.ZipCode ?? "",
                State = dvm.user.State ?? "",
                City = dvm.user.City ?? "",
                Street = dvm.user.Street ?? "",
                Notes = dvm.Notes,
                BirthDate = dvm.Date,
                Room = dvm.Room ?? ""
                //BirthDate=dvm.dob

            };

            return pr;
        }

        public PatientInfoVM PatientSomeOneElseRequest(int id, PatientInfoVM model)
        {
            //var userid = HttpContext.Session.GetInt32("userId");
            var userobj = _repo.GetUserUserIdData(id);
            var request = _repo.GetRequestByEmail(model.Email);


            Request req = new()
            {
                RequestType = 2,
                UserId = id,
                CreatedUserId = id,
                FirstName = userobj.FirstName,
                LastName = userobj.LastName,
                Email = userobj.Email??"",
                PhoneNumber = userobj.Mobile,
                Status = 1,
                CreatedDate = DateTime.Now,
                CallType = 1,
                RelationName = model.RelationName
            };
            if (request.RequestId != 0)
            {
                req.UserId = request.UserId;

                req.PatientAccountId = request.PatientAccountId;
            }

            _repo.AddRequest(req);

            if (model.Files != null)
            {
                foreach (IFormFile files in model.Files)
                {
                    string filename = files.FileName;
                    string path = Path.Combine("D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\", filename);
                    using (FileStream stream = new(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }

                    RequestWiseFile requestWiseFile = new()
                    {
                        FileName = filename,
                        RequestId = req.RequestId,
                        DocType = 1
                    };
                    _repo.AddReqWisFile(requestWiseFile);
                }
            }


            Region reg = new()
            {
                Name = model.State,
                Abbreviation = model.State
            };
            _repo.AddRegion(reg);

            RequestClient reqClient = new()
            {
                RequestId = req.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Location = model.Room,
                Address = model.Street + " , " + model.City + " , " + model.State,
                RegionId = reg.RegionId,
                Email = model.Email??"",
                Notes = model.Notes,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                StrMonth = DateTime.Now.ToString("MMMM"),
                IntYear = DateTime.Now.Year,
                IntDate = DateTime.Now.Day
            };
            _repo.AddReqClient(reqClient);


            return model;
        }

        public bool ValidateUserByEmail(string? email)
        {
            bool ret = _repo.GetEmail(email);
            return ret;

        }

        public SendAgreementVM GetRequest(int id)
        {
            var x = _repo.GetReqClientById(id);
            SendAgreementVM vm = new()
            {
                PatientName = x.FirstName,
                ReqId = x.RequestId
            };
            return vm;
        }

        public bool GetAgree(int id)
        {
            //RequestClient reqCli = _repo.GetRequestClientinfo(id);

            var req = _repo.GetRequestById(id);

            if (req.Status != 4 && req.RequestId != 0)
            {
                req.Status = 4;
                RequestStatusLog reqlog = new()
                {
                    // reqlog.AdminId = adminid;
                    CreatedDate = DateTime.Now,
                    Notes = "The request has been transfered to Active state",
                    RequestId = id,
                    Status = 4
                };
                _repo.UpdateRequest(req);
                _repo.AddReqStatusLog(reqlog);

                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CancelAgreement(int id, SendAgreementVM VM) /*int adminid*/
        {
            //RequestClient reqCli = _repo.GetRequestClientinfo(id);
            var req = _repo.GetRequestById(id);
            if (req.Status != 7 && req.RequestId != 0)
            {
                req.Status = 7;
                RequestStatusLog reqlog = new()
                {
                    //reqlog.AdminId = adminid;
                    CreatedDate = DateTime.Now,
                    Notes = VM.Note,
                    RequestId = id
                };
                _repo.UpdateRequest(req);
                _repo.AddReqStatusLog(reqlog);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateAspUser(ResetPwVM RP, int id)
        {
            AspNetUser user = _repo.GetAspNetUser(id);
            if (user.Id != 0)
            {
                user.PasswordHash = Crypto.HashPassword(RP.Password);
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = id.ToString();
                _repo.UpdateAspNetUser(user);
            }
            return true;
        }

        public bool CreateUser(ResetPwVM rP)
        {
            bool user = _repo.GetEmail(rP.Email??"");
            RequestClient requestClient = _repo.GetReqClientByEmail(rP.Email??"");
            AspNetUser aspUser = _repo.GetAspUserData(rP.Email??"");
            User user1 = _repo.GetUserUserData(rP.Email??"");
            Request req = _repo.GetRequestById(requestClient.RequestId);
            if (!user)
            {
                AspNetUser asp = new()
                {
                    UserName = requestClient.FirstName + " " + requestClient.LastName,
                    Email = rP.Email??"",
                    PasswordHash = Crypto.HashPassword(rP.Password),
                    CreatedDate = DateTime.Now,
                    PhoneNumber = requestClient.PhoneNumber,
                    Roles = _repo.GetPatientRole()
                };
                _repo.AddAspNetUser(asp);
                if (user1.UserId == 0)
                {
                    AspNetUser aspUser1 = _repo.GetAspUserData(rP.Email??"");
                    User use = new()
                    {
                        AspNetUserId = aspUser1.Id,
                        FirstName = requestClient.FirstName,
                        LastName = requestClient.LastName,
                        Email = requestClient.Email,
                        Mobile = requestClient.PhoneNumber,
                        ZipCode = requestClient.ZipCode,
                        State = requestClient.State,
                        City = requestClient.City,
                        Street = requestClient.Street,
                        IntDate = requestClient.IntDate,
                        IntYear = requestClient.IntYear,
                        StrMonth = requestClient.StrMonth,
                        CreatedDate = DateTime.Now,
                        RegionId = requestClient.RegionId,
                        CreatedBy = req.RequestId.ToString(),
                        AspNetUser = aspUser1,
                        Status = 1//modified_by,modified_date,status,ip,is_request_with_email,is_deleted remaining to add
                    };
                    user1 = use;
                    _repo.AddUser(user1);

                    User userData = _repo.GetUserAspIdData(aspUser1.Id);
                    if (req.RequestId != 0)
                    {
                        req.UserId = userData.UserId;
                        req.User = userData;
                        _repo.UpdateRequest(req);
                    }
                }

                return true;
            }
            return false;
        }
    }
}
