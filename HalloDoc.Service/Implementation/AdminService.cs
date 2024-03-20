using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using HalloDoc.services.Interface;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;

namespace HalloDoc.services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IAAdmin _repo;

        public AdminService(IAAdmin repo)
        {
            _repo = repo;
        }

        public AdminDashboardVM PatientStatus(AdminDashboardVM swc)
        {
            var x = _repo.GetAdminDashboardData();

            List<Region> reg = _repo.GetAllRegions();

            var count = x.GroupBy(x => x.Status).Select(c => new
            {
                s = c.Key,
                num = c.Count(),
            }).ToList();

            var CountNew = count.FirstOrDefault(x => x.s == 1)?.num ?? 0;
            var CountPen = count.FirstOrDefault(x => x.s == 2)?.num ?? 0;
            var CountAct = count.FirstOrDefault(x => x.s == 4)?.num ?? 0 + count.FirstOrDefault(x => x.s == 5)?.num ?? 0;
            var CountClo = count.FirstOrDefault(x => x.s == 3)?.num ?? 0 + count.FirstOrDefault(x => x.s == 7)?.num ?? 0 + count.FirstOrDefault(x => x.s == 8)?.num ?? 0;
            var CountCon = count.FirstOrDefault(x => x.s == 6)?.num ?? 0;
            var CountUnp = count.FirstOrDefault(x => x.s == 9)?.num ?? 0;
            /* foreach(int data in count.f)
             {

             }*/

            swc.CountNewState = CountNew;
            swc.CountPendingState = CountPen;
            swc.CountActiveState = CountAct;
            swc.CountConcludeState = CountCon;
            swc.CountCloseState = CountClo;
            swc.CountUnpaidState = CountUnp;
            swc.RegionList = reg;
            return swc;
        }
        public List<AdminDashboardVM> GetNewStateData(int status, int id)
        {
            Dictionary<int, int> dictonary = new Dictionary<int, int>()
            {
                {1,1 },
                {2,2 },
                {3,5},
                {4,3},
                {5,3},
                {6,4},
                {7,5},
                {8,5},
                {9,6},
                {10,7}
            };

            var x = _repo.GetAdminDashboardData().Where(x => dictonary[x.Status] == status).ToList();
            
            if (id != 0)
            {
                x = x.Where(x => x.RequestType == id).ToList();
            }


            List<AdminDashboardVM> adminDashboardVMs = new List<AdminDashboardVM>();
            x.ForEach(a =>
            {
                var y = _repo.GetStatusLogs1(a.RequestId);
                List<string>? transfer = new List<string>();
                foreach (var data in y)
                {
                    Physician phy = _repo.GetPhysicianDataByID(data.TransToPhysicianId);
                    if (phy != null)
                    {
                        transfer.Add("Admin Transfered to Dr. " + phy.FirstName + " on " + data.CreatedDate.ToString("dd/MMM/yyyy") + " at " + data.CreatedDate.ToString("hh:mm:ss") + " : " + data.Notes);
                    }

                }
                DateOnly Dateofbirth = new DateOnly(a.RequestClients.FirstOrDefault()!.IntYear, DateOnly.ParseExact(a.RequestClients.FirstOrDefault()!.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, a.RequestClients.FirstOrDefault()!.IntDate!);
                //count++;
                adminDashboardVMs.Add(new AdminDashboardVM
                {

                    PatientName = a.RequestClients.FirstOrDefault()?.FirstName ?? "",
                    PatientLastName = a.RequestClients.FirstOrDefault()?.LastName ?? "",
                    ReqID = a.RequestId,
                    Email = a.RequestClients.FirstOrDefault()?.Email ?? "",
                    RequestorName = a.FirstName,
                    RequestDate = DateOnly.FromDateTime(a.CreatedDate),
                    PhoneNumber = a.RequestClients.FirstOrDefault()?.PhoneNumber ?? "",
                    Region = a.RequestClients.FirstOrDefault()?.RegionId,
                    RequestorPhoneNumber = a.PhoneNumber ?? "",
                    Status = a.Status,
                    ProviderName = a.Physician?.FirstName ?? "",
                    BirthDate =Dateofbirth, 
                    Address = a.RequestClients.FirstOrDefault()?.Street + " " + a.RequestClients.FirstOrDefault()?.City + " " + a.RequestClients.FirstOrDefault()?.State + " " + a.RequestClients.FirstOrDefault()?.ZipCode,
                    AdminNotes = transfer,
                    RequestType = a.RequestType,
                    
                    
                });
                



            });
            return adminDashboardVMs;
        }
        public ViewCaseVM ViewPatientData(int id)
        {
            var PatientData = _repo.GetPatientData(id);

            ViewCaseVM PatientCase = new()
            {
                FirstName = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.FirstName ?? " ",
                LastName = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.LastName ?? " ",
                DateOfBirth = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.IntDate + " " + PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.StrMonth + " " + PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.IntYear,
                Email = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Email ?? " ",
                Notes = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Notes ?? " ",
                PhoneNumber = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.PhoneNumber ?? " ",
                Region = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.State ?? " ",
                Address = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Street + " " + PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.City + " " + PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.ZipCode,
                ConfNO = PatientData.FirstOrDefault()?.ConfirmationNumber ?? " ",
                Room = PatientData.FirstOrDefault()?.RequestClients.FirstOrDefault()?.Location ?? "",
                RequestType = PatientData.FirstOrDefault()?.RequestType ?? 0

            };


            return PatientCase;
        }
        public RequestNote ViewNotes(int id, RequestNote vm)
        {

            RequestNote isEntry = _repo.checkNotes(id, vm);
            if (isEntry == null)
            {
                RequestNote x = new RequestNote();
                x.RequestId = id;
                x.CreatedBy = 46;//Admin ID
                x.CreatedDate = DateTime.Now;
                x.AdminNotes = vm.AdminNotes;
                _repo.GetReqNotesData(x);
            }
            else
            {
                isEntry.AdminNotes = vm.AdminNotes;
                _repo.DbUpdatedNotes(isEntry);

            }
            return isEntry??new RequestNote();
        }

        public ViewNotesVM ViewNotes2(int id)
        {
            var y = _repo.GetStatusLogs1(id);
            List<string>? transfer = new List<string>();
            foreach (var item in y)
            {
                Physician phy = _repo.GetPhysicianDataByID(item.TransToPhysicianId);
                if (phy != null)
                {
                    transfer.Add("Admin Transfered to Dr. " + phy.FirstName + " on " + item.CreatedDate.ToString("dd/MM/YYYY") + " at " + item.CreatedDate.ToString("hh:mm:ss") + " : " + item.Notes);
                }

            }
            List<Request> data = _repo.GetNotes2(id);
            ViewNotesVM viewNotesVM = new ViewNotesVM();
            //RequestStatusLog y=_repo.GetStatusLogs(id);
            if (data!=null && y != null)
            {
                viewNotesVM.AdminNotes = data.FirstOrDefault()?.RequestNotes.FirstOrDefault()?.AdminNotes;
                viewNotesVM.PhysicianName = data.FirstOrDefault().Physician?.FirstName;
                viewNotesVM.StatusLogNotes = transfer;
                viewNotesVM.ReqId = id;
            }
            else
            {
                // Handle the case when data is empty (e.g., return an error message or default values)
                viewNotesVM.AdminNotes = "No data found";
                viewNotesVM.PhysicianName = "Unknown";
            }

            return viewNotesVM;
        }


        public void UpStatus(int id, CancelCaseVM vm)
        {

            var x = _repo.GetRequestStatus(id);
            x.Status = 3;
            x.CaseTag = vm.CaseID;
            _repo.DbReqStatusUpdate(x);

            RequestNote y = _repo.GetRequestNotesData(id);
            if (y != null)
            {
                y.AdminNotes = vm.AdminNotes;
                //y.ModifiedBy=aspnetuserid
                y.ModifiedDate = DateTime.Now;
            }
            else
            {
                RequestNote requestNote = new RequestNote();
                requestNote.RequestId = id;
                /*y.PhysicianNotes*/
                requestNote.AdminNotes = vm.AdminNotes;
                //requestNote.CreatedBy=aspnetuserid
                requestNote.ModifiedDate = DateTime.Now;
                _repo.GetReqNotesData(requestNote);
            }

            RequestStatusLog statusLog = new RequestStatusLog
            {
                RequestId = id,
                Status = 3,
                // PhysicianId = vm.AdminDashboardVM.PhysicianId,
                Notes = vm.AdminNotes,
                CreatedDate = DateTime.Now
                //AdminId=adminid
            };
            _repo.GetAddStatusLog(statusLog);
            /*vm.status = 3;*/

        }

        public CancelCaseVM CancelCaseData(int id)
        {
            RequestClient req = _repo.GetDataReqClient(id);
            List<CaseTag> tags = _repo.GetCaseTags();
            CancelCaseVM vm = new CancelCaseVM();
            //vm.reqID = id;
            vm.PatientName = req.FirstName;
            vm.ReasonName = tags;
            vm.ReqID = id;
            return vm;

        }

        public AssignCaseVM GetPhysician(int id, AssignCaseVM vm)
        {
            List<Region> reg = _repo.GetAllRegions();
            List<Physician> physi = _repo.GetPhysicianData();
            AssignCaseVM case1 = new AssignCaseVM();
            {
                case1.ReqId = id;
                case1.Physicians = physi;
                case1.Region = reg;
            }
            return case1;
        }

        public object GetPhysiciansByRegionId(int id)
        {
            return _repo.GetPhysicianByReg(id);
        }

        public void UpAssignStatus(int id, AssignCaseVM vm)
        {
            Request req = _repo.GetRequestStatus(id);
            req.Status = 2;
            req.PhysicianId = vm.SelectedPhysicianName;
            req.ModifiedDate = DateTime.Now;
            _repo.GetAddRequest(req);

            RequestNote reqNote = _repo.GetNotes(id);

            if (reqNote != null)
            {
                reqNote.RequestId = id;
                reqNote.AdminNotes = vm.AdminNotes;
                reqNote.ModifiedDate = DateTime.Now;
                _repo.GetUpDateReqNote(reqNote);
            }
            else
            {
                RequestNote Note = new RequestNote {

                    RequestId = id,
                    AdminNotes = vm.AdminNotes,
                    CreatedDate = DateTime.Now,

                };
                _repo.GetReqNotesData(Note);
            }

            RequestStatusLog StatusLog = new RequestStatusLog
            {
                RequestId = id,
                Status = 2,
                Notes = vm.AdminNotes,
                //AdminId=
                TransToPhysicianId = req.PhysicianId,
                CreatedDate = DateTime.Now
            };
            _repo.GetAddStatusLog(StatusLog);
        }

        public BlockCaseVM BlockCaseData(int id, BlockCaseVM vm)
        {
            RequestClient req = _repo.GetDataReqClient(id);

            BlockCaseVM obj = new BlockCaseVM {
                PateintFirstName = req.FirstName,
                PatientLastName = req.LastName,
                ReqId = id
            };
            return obj;
        }

        public void UpBlockCase(int id, BlockCaseVM vm)
        {
            Request req = _repo.GetRequestStatus(id);
            req.Status = 10;
            req.ModifiedDate = DateTime.Now;

            _repo.DbReqStatusUpdate(req);

            RequestStatusLog statusLog = new RequestStatusLog
            {
                RequestId = id,
                Status = 10,
                // PhysicianId = vm.AdminDashboardVM.PhysicianId,
                Notes = vm.ReasonForBlock,
                CreatedDate = DateTime.Now
                //AdminId=adminid
            };
            _repo.GetAddStatusLog(statusLog);

            RequestClient rc = _repo.GetDataReqClient(id);

            Blockrequest BlReq = new()
            {
                Phonenumber = rc.PhoneNumber,
                Email = rc.Email,
                Reason = vm.ReasonForBlock,
                CreatedDate = DateTime.Now

            };

            _repo.DbAddReqClient(BlReq);


        }

        public ViewUploadVM PatientViewDocuments(int id)
        {
            RequestClient reqclient = _repo.GetDataReqClient(id);
            Request request = _repo.GetRequestStatus(id);
            User user2 = _repo.GetUserData().FirstOrDefault(u => u.AspNetUserId ==request.PatientAccountId);

            DateOnly date = new DateOnly(user2.IntYear.Value, DateOnly.ParseExact(user2.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, user2.IntDate.Value);

            ViewUploadVM document = new ViewUploadVM();
            document.PatientName = reqclient.FirstName + ' ' + reqclient.LastName;
            Request req = _repo.GetRequestStatus(id)
    ;
            document.ConfNumber = req.ConfirmationNumber;
            document.ReqId = req.RequestId;
            List<RequestWiseFile> files2 = _repo.GetReqWiseFile(id)
    ;
            List<RequestWiseFile> fl = new();
            foreach (var data in files2)
            {
                if (data.IsDeleted != true)
                {
                    fl.Add(data);
                }
            }
            document.Email = req.Email;
            document.File = fl;
            document.Date = files2.FirstOrDefault()?.CreatedDate;
            return document;
        }
        public string PatientFileSave(int id, PatientDashboardVM model)
        {
            Request user1 = _repo.GetRequestData().FirstOrDefault(u => u.RequestId == id);
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
                    requestWiseFile.CreatedDate = model.CreatedDate;
                    _repo.GetReqWisFileData(requestWiseFile);

                }
            }

            return "yes";
        }
        public List<RequestWiseFile> DownloadAll(int id)
        {
            return _repo.DownloadAll(id);

        }


        public RequestWiseFile Download(int id)
        {
            RequestWiseFile data = _repo.Download(id);
            return data;
        }

        public RequestWiseFile Delete(int id)
        {
            RequestWiseFile data = _repo.Download(id);
            data.IsDeleted = true;
            _repo.GetDbUpRequestWise(data);
            return data;
        }
        public object DeleteAll(int id)
        {
            List<RequestWiseFile> data = _repo.DownloadAll(id);
            foreach (var item in data)
            {
                item.IsDeleted = true;
                _repo.GetDbUpRequestWise(item);
            }

            return data;
        }

        public string SMail(int id)
        {
            Request request = _repo.GetRequestStatus(id);
            List<RequestWiseFile> requestWiseFile = _repo.GetReqWiseFile(id);
            var receiver = "xyz32322@gmail.com";
            var subject = "Documents of Request " + request.ConfirmationNumber?.ToUpper();
            var message = "Find the Files uploaded for your request in below:";
            var mailMessage = new MailMessage(from: "tatva.dotnet.dhruvrajsinhvaghela@outlook.com", to: receiver, subject, message);

            foreach (var file in requestWiseFile)
            {
                var filePath = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
                if (File.Exists(filePath))
                {
                    byte[] fileContent;
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        fileContent = new byte[fileStream.Length];
                        fileStream.Read(fileContent, 0, (int)fileStream.Length);
                    }
                    var attachment = new Attachment(new MemoryStream(fileContent), file.FileName);
                    mailMessage.Attachments.Add(attachment);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }

            var mail = "tatva.dotnet.dhruvrajsinhvaghela@outlook.com";
            var password = "Vagheladhruv@123";

            var client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            client.SendMailAsync(mailMessage);
            return "yes";
        }

        public AspNetUser AdmintLogin(AspNetUser aspNetUser)
        {
            AspNetUser user = _repo.GetAspUserData(aspNetUser.Email);
            if (user.Id != 0 && Crypto.VerifyHashedPassword(user.PasswordHash, aspNetUser.PasswordHash))//in crypto. method (hash password,plain text)
            {
                return user;
            }
            return new AspNetUser();
        }

        public SendMailVM SendAgreement(int id)
        {
            Request ret = _repo.GetEmail(id);
            SendMailVM vm = new SendMailVM()
            {
                Email=ret.Email,
                ReqId=ret.RequestId,
                PhoneNo=ret.PhoneNumber,
            };
            return vm;
        }


        public SendMailVM GetReqType(int id, SendMailVM vm)
        {
            var data = _repo.GetRequestStatus(id);
            vm.ReqType = data.RequestType;
            vm.ReqId = data.RequestId;
            return vm;
        }

        public SendOrderVM GetProfessions(int id, SendOrderVM vm)
        {
            List<HealthProfessionalType> proff = _repo.GetProfessionList(id);
            SendOrderVM model = new SendOrderVM
            {
                ProfessionTypes = proff,
                ReqId = id
            };
            return model;
        }

        public List<HealthProfessional> GetVendorNames(int id)
        {
            List<HealthProfessional> proff = _repo.GetVendorByProfId(id);
            return proff;
        }
        public object GetVendorData(int vendorId)
        {
            HealthProfessional data = _repo.GetVendor(vendorId);

            SendOrderVM vm = new SendOrderVM();
            vm.Email = data.Email;
            vm.Phone = data.PhoneNumber;
            vm.Fax = data.FaxNumber;

            return vm;
        }

        public bool AddOrderData(int id, SendOrderVM vm)
        {
            OrderDetail model = new OrderDetail()
            {
                RequestId = id,
                Email = vm.Email,
                BusinessContact = vm.Phone,
                FaxNumber = vm.Fax,
                Prescription = vm.Prescription,
                NoOfRefill = vm.ReFill,
                CreatedDate = DateTime.Now
                //CreatedBy=AdminID
            };
                _repo.UpdateOrders(model);
            return true; 
        }

        public object GetClearCase(int id)
        {
            var data=_repo.GetRequestStatus(id);
            BlockCaseVM vm = new BlockCaseVM 
            {
                ReqId= id
            };
            return vm;
        }

        public bool UpStatusClear(int id)
        {
            var x=_repo.GetRequestStatus(id);
            x.Status = 10;
            x.ModifiedDate = DateTime.Now;
            _repo.GetAddRequest(x);
            return true;
        }

        public object GetCloseCase(int id)
        {
            var reqCl = _repo.GetDataReqClient(id);
            var req=_repo.GetRequestStatus(id);
            var reqWise = _repo.GetReqWiseFile(id);
            DateOnly date = new DateOnly(reqCl.IntYear, DateOnly.ParseExact(reqCl.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, reqCl.IntDate);
            CloseCaseVM vm = new CloseCaseVM() { };
            vm.ReqId = id;
            vm.FirstName = reqCl.FirstName;
            vm.LastName = reqCl.LastName;
            vm.Email = reqCl.Email;
            vm.PhoneNo = reqCl.PhoneNumber;
            vm.BirthDate = date;
            vm.ConfNo = req.ConfirmationNumber;

            List < RequestWiseFile > fl = new();
            foreach (var data in reqWise)
            {
                if (data.IsDeleted != true)
                {
                    fl.Add(data);
                }
            }
            vm.FileList = fl;
            
            return vm;
        }
        public bool UpCloseCase(int id, CloseCaseVM vm)
        {
            Request req = _repo.GetRequestStatus(id);
            AspNetUser asp = _repo.GetAspUserData(req.Email);
            if(req.RequestType==1)
            {
            req.PhoneNumber=vm.PhoneNo; 
            req.Email = vm.Email;
            _repo.GetAddRequest(req);
            asp.Email = vm.Email;
            asp.PhoneNumber = vm.PhoneNo;
            _repo.GetUpAspUser(asp);
            }

            RequestClient reqcl = _repo.GetDataReqClient(id);
            reqcl.Email = vm.Email;
            reqcl.PhoneNumber = vm.PhoneNo;
            _repo.UpReqClient(reqcl);

            User use = _repo.GetUserData().FirstOrDefault(x=>x.AspNetUserId==asp.Id);
            use.FirstName = vm.FirstName??"";
            use.Mobile = vm.PhoneNo;
            _repo.GetUpUser(use);

            return true;
        }

        public object AdminProfileData(int? aspId,int? adminId)
        {
            Admin data = _repo.GetAdminData(aspId);
            AspNetUser aspData = _repo.GetAspNetUserData(aspId);
            List<string> role = _repo.GetRole(aspData.Id);
            List<AdminRegion> adminRegion = _repo.GetAdminRegion(adminId);

            List<Region> reg = new();
            foreach(var item in adminRegion)
            {
                Region region = _repo.GetRegionById(item.RegionId);
                if(region != null)
                {
                    reg.Add(region);
                }
            }

            AdminProfileVM vm = new()
            {
                AdminId=adminId,
                UserName = aspData.UserName,
                Roll = role[0],
                Status = data.Status,
                FirstName =data.FirstName,
                LastName=data.LastName,
                Email=data.Email,
                Address1=data.Address1,
                Address2=data.Address2,
                City=data.City,
                PhoneNo=data.Mobile,
                ZipCode=data.Zip,
                AltPhone=data.AltPhone,
                RegionList= _repo.GetAllRegions(),
                AdminRegList=reg
            };
            return vm;
        }

        public AspNetUser AspUserData(string email)
        {
            var data=_repo.GetAspUserData(email); 
            return data;
        }

        public List<AdminDashboardVM> GetDataPagination(int status,int pn,int item)
        {
            Dictionary<int, int> dictonary = new Dictionary<int, int>()
            {
                {1,1},
                {2,2},
                {3,5},
                {4,3},
                {5,3},
                {6,4},
                {7,5},
                {8,5},
                {9,6},
                {10,7}
            };

            var x = _repo.GetAdminDashboardData().Where(x => dictonary[x.Status] == status).ToList();
            var count= x.Count();
            var joinedquery = x.Skip((pn - 1) * item).Take(item).ToList();


            List<AdminDashboardVM> adminDashboardVMs = new List<AdminDashboardVM>();
            joinedquery.ForEach(a =>
            {
                var y = _repo.GetStatusLogs1(a.RequestId);
                List<string>? transfer = new List<string>();
                foreach (var data in y)
                {
                    Physician phy = _repo.GetPhysicianDataByID(data.TransToPhysicianId);
                    if (phy != null)
                    {
                        transfer.Add("Admin Transfered to Dr. " + phy.FirstName + " on " + data.CreatedDate.ToString("dd/MMM/yyyy") + " at " + data.CreatedDate.ToString("hh:mm:ss") + " : " + data.Notes);
                    }

                }
                DateOnly Dateofbirth = new DateOnly(a.RequestClients.FirstOrDefault()!.IntYear, DateOnly.ParseExact(a.RequestClients.FirstOrDefault()!.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, a.RequestClients.FirstOrDefault()!.IntDate!);
                //count++;
                adminDashboardVMs.Add(new AdminDashboardVM
                {
                    ItemCountPagination=count,
                    PatientName = a.RequestClients.FirstOrDefault()?.FirstName ?? "",
                    PatientLastName = a.RequestClients.FirstOrDefault()?.LastName ?? "",
                    ReqID = a.RequestId,
                    Email = a.RequestClients.FirstOrDefault()?.Email ?? "",
                    BirthMonth = a.RequestClients.FirstOrDefault()?.StrMonth ?? "",
                    BirthYear = a.RequestClients.FirstOrDefault()?.IntYear ?? 9999,
                    BirthDay = a.RequestClients.FirstOrDefault()?.IntDate ?? 31,
                    RequestorName = a.FirstName,
                    RequestDate = DateOnly.FromDateTime(a.CreatedDate),
                    PhoneNumber = a.RequestClients.FirstOrDefault()?.PhoneNumber ?? "",
                    Region = a.RequestClients.FirstOrDefault()?.RegionId,
                    RequestorPhoneNumber = a.PhoneNumber ?? "",
                    Status = a.Status,
                    ProviderName = a.Physician?.FirstName ?? "",
                    BirthDate=Dateofbirth,
                    Address = a.RequestClients.FirstOrDefault()?.Street + " " + a.RequestClients.FirstOrDefault()?.City + " " + a.RequestClients.FirstOrDefault()?.State + " " + a.RequestClients.FirstOrDefault()?.ZipCode,
                    AdminNotes = transfer,
                    RequestType = a.RequestType,

                });

            });
            return adminDashboardVMs;
        }

        public List<AdminDashboardVM> GetFilteredData(string keywrd, int regId, int status, int reqType, int item, int pn)
        {
            var joinedquery = _repo.GetAdminDashboardData();
            joinedquery = joinedquery.Where(x => x.Status == status).ToList();
            if (keywrd != "undefined" && keywrd != null)
            {
                joinedquery = joinedquery.Where(x => x.RequestClients.FirstOrDefault()!.FirstName.StartsWith(keywrd!, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (regId != 0)
            {
                joinedquery = joinedquery.Where(x => x.RequestClients.FirstOrDefault()?.RegionId == regId).ToList();
            }
            if (reqType != 0)
            {
                joinedquery = joinedquery.Where(x => x.RequestType == reqType).ToList();
            }
            var pagecount = joinedquery.Count();
            if (item != 0 && pn != 0)
            {
                joinedquery = joinedquery.Skip((pn - 1) * item).Take(item).ToList();
            }
            List<AdminDashboardVM> dashboard = new();
            foreach (var item1 in joinedquery)
            {
                List<RequestStatusLog> reqlog = _repo.GetStatusLogs(item1.RequestId);

                List<string> transfer = new();
                foreach (var log in reqlog)
                {

                    Physician? phy = _repo.GetPhysicianDataByID(log.TransToPhysicianId);
                    if (phy == null)
                    {
                        transfer.Add("Admin status changed to " + item1.Status);
                    }
                    else
                    {
                        transfer.Add("Admin transfered to Dr." + phy.FirstName + " on " + phy.CreatedDate?.ToString("dd/MM/yyyy") + " at " + phy.CreatedDate?.ToString("hh:mm:ss") + " : " + log.Notes);
                    }

                }
                DateOnly Dateofbirth = new DateOnly(item1.RequestClients.FirstOrDefault()!.IntYear, DateOnly.ParseExact(item1.RequestClients.FirstOrDefault()!.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, item1.RequestClients.FirstOrDefault()!.IntDate!);
                if (item != null)
                {
                    dashboard.Add(new AdminDashboardVM()
                    {
                        ItemCountPagination = pagecount,
                        PatientName = item1.RequestClients.FirstOrDefault()?.FirstName + ' ' ?? " " + item1.RequestClients.FirstOrDefault()?.LastName ?? "",
                        BirthDate = Dateofbirth,
                        RequestType = item1.RequestType,
                        RequestorName = item1?.FirstName ?? "",
                        RequestDate = DateOnly.FromDateTime(item1!.CreatedDate),
                        PhoneNumber = item1?.PhoneNumber ?? "",
                        RequestorPhoneNumber = item1?.RequestClients.FirstOrDefault()?.PhoneNumber ?? "",
                        Address = item1?.RequestClients.FirstOrDefault()?.Street ?? "" + ' ' + item1!.RequestClients.FirstOrDefault()?.City ?? "" + ' ' + item1.RequestClients.FirstOrDefault()?.State ?? "",
                        Notes = transfer,
                        Region = item1.RequestClients.FirstOrDefault()?.RegionId ?? 0,
                        Email = item1.RequestClients.FirstOrDefault()?.Email ?? "",
                        ProviderName = item1.Physician?.FirstName ?? "",
                        ReqID = item1.RequestId
                    }) ;
                }

            }
            return dashboard;

        }
        public void EditAdminProfile(AdminProfileVM model, int id)
        {
            
            Admin adminData = _repo.GetAdminDataById(id)
;
            AspNetUser aspnetuser = _repo.GetAspNetUserData(adminData.AspNetUserId);
            List<AdminRegion> regions = _repo.GetAdminRegion(id)
;

            List<int> AddRegion = new();

            foreach (var region in regions)
            {
                AddRegion.Add(region.RegionId);
            }

            List<int> AddAminRegion = model.SelectedRegions.Except(AddRegion).ToList();
            List<int> RemoveAminRegion = AddRegion.Except(model.SelectedRegions).ToList();

            foreach (var region in AddAminRegion)
            {
                AdminRegion adds = new() { AdminId = id, RegionId = region };
                _repo.AddAdminRegion(adds);
            }

            foreach (var region in RemoveAminRegion)
            {
                AdminRegion removes = new() { AdminId = id, RegionId = region };
                _repo.RemoveAdminRegion(removes);
            }

            adminData.FirstName = model.FirstName;
            adminData.LastName = model.LastName;
            adminData.Email = model.Email;
            adminData.Mobile = model.PhoneNo;
            adminData.ModifiedDate = DateTime.Now;
            //admin.ModifiedBy = admin.AspNetUserId;
            _repo.UpAdmin(adminData);

            aspnetuser.Email = model.Email;
            aspnetuser.PhoneNumber = model.PhoneNo;
            aspnetuser.ModifiedDate = DateTime.Now;
            _repo.GetUpAspUser(aspnetuser);

        }

        public Admin GetAdminDataById(int id)
        {
            Admin data=_repo.GetAdminDataById(id);
            return data;
        }
    }

      
}
