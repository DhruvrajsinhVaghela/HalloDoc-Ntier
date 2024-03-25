using Microsoft.AspNetCore.Mvc;
using HalloDoc.DbEntity.Models;
using HalloDoc.services.Interface;
using HalloDoc.DbEntity.ViewModel;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using HalloDoc.Services.Interfaces;
using HalloDoc.Auth;
using Microsoft.PowerBI.Api.Models;
using ClosedXML.Excel;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;

        private readonly IJwtToken _token;

        public AdminController(IAdminService service, IJwtToken token)
        {
            _service = service;
            _token = token;
        }

        [HttpGet("admin/dashboard")]
        public IActionResult AdminDashboard(AdminDashboardVM swc)
        {
            var data = _service.PatientStatus(swc);
            return View(data);

        }

        public IActionResult CardsView(int status, int pn, int item)
        {

            List<AdminDashboardVM> data = _service.GetDataPagination(status, pn, item);

            return status switch
            {
                1 => PartialView("_AdminNewStateData", data),/*,data*/
                2 => PartialView("_AdminPendingstateData", data),
                3 => PartialView("_AdminActiveStateData", data),
                4 => PartialView("_AdminConcludeStateData", data),
                5 => PartialView("_AdminToCLoseStatusData", data),
                6 => PartialView("_AdminUnpaidStatusData", data),
                _ => PartialView("_NewState", data),
            };
        }


        [HttpGet("admin/dashboard/view-case")]
        public IActionResult ViewReservation(int id)
        {
            var data = _service.ViewPatientData(id);
            return View(data);
        }

        [HttpGet("admin/dashboard/view-notes")]
        public IActionResult ViewNotes(int id)
        {
            ViewNotesVM data = _service.ViewNotes2(id);
            return View(data);
        }

        [HttpPost("admin/dashboard/view-notes")]
        public IActionResult ViewNotes(int id, ViewNotesVM vm)
        {
            vm.AdminAspId = HttpContext.Session.GetInt32("userId");
            _service.ViewNotes(id, vm);
            return RedirectToAction("ViewNotes", new {id,vm });
        }

        public IActionResult CancelCase(int id)
        {
            var data = _service.CancelCaseData(id);
            return PartialView("_CancleCaseModal", data);
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id, CancelCaseVM vm)
        {
            _service.UpStatus(id, vm);
            return RedirectToAction("AdminDashboard", new { id });
        }


        public IActionResult AssignCase(int id, AssignCaseVM vm)
        {

            var phy = _service.GetPhysician(id, vm);

            return PartialView("_AssignCaseModal", phy);
        }

        public IActionResult TransferCase(int id, AssignCaseVM vm)
        {
            var phy = _service.GetPhysician(id, vm);

            return PartialView("_TransferCase", phy);
        }

        public IActionResult AssignCase1(int id)
        {
            var physicians = _service.GetPhysiciansByRegionId(id); // Assuming this method returns physicians based on the region ID

            if (physicians != null)
            {
                return Json(physicians); // Return a single-item array
            }
            else
            {
                return Json(Array.Empty<Physician>()); // Return an empty array
            }
        }

        [HttpPost]
        public IActionResult UpdateAssignCase(int id, AssignCaseVM vm)
        {
            vm.AdminAspId = HttpContext.Session.GetInt32("userId");
            _service.UpAssignStatus(id, vm);
            return RedirectToAction("AdminDashboard", new { id });
        }

        public IActionResult BlockCase(int id, BlockCaseVM vm)
        {
            var data = _service.BlockCaseData(id, vm);
            return PartialView("_BlockCaseModal", data);
        }

        [HttpPost]
        public IActionResult UpdateBlockCase(int id, BlockCaseVM vm)
        {
            _service.UpBlockCase(id, vm);
            return RedirectToAction("AdminDashboard", new { id });
        }

        [HttpGet("admin/dashboard/view-uploads")]
        public IActionResult ViewUploads(int id)
        {
            _ = new List<PatientDashboardVM>();
            var res = _service.PatientViewDocuments(id);

            return View(res);
        }
        public IActionResult PatientFileSave(int id, PatientDashboardVM model)
        {
            var data = _service.PatientFileSave(id, model);
            if (data == "yes")
            {
                return RedirectToAction("ViewUploads", "Admin", new { id });
            }

            return View();
        }

        public IActionResult DownloadAll(int id)
        {
            var filesRow = _service.DownloadAll(id);

            MemoryStream ms = new();
            using (ZipArchive zip = new(ms, ZipArchiveMode.Create, true))
                filesRow.ForEach(file =>
                {
                    var path = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
                    //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
                    ZipArchiveEntry zipEntry = zip.CreateEntry(file.FileName);
                    using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
                    using Stream zipEntryStream = zipEntry.Open();
                    fs.CopyTo(zipEntryStream);
                });
            return File(ms.ToArray(), "application/zip", "download.zip");
        }

        public IActionResult Download(int id)
        {

            var file = _service.Download(id);
            //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
            var path = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", file.FileName);
        }

        public IActionResult DeleteFile(int id)
        {
            RequestWiseFile data = _service.Delete(id);
            return RedirectToAction("ViewUploads", "Admin", new { id = data.RequestId });

        }

        public IActionResult DeleteAll(int id)
        {
            _ = _service.DeleteAll(id);
            return RedirectToAction("ViewUploads", "Admin", new {id});
        }

        public IActionResult SendMail(int id)
        {
            var data = _service.SMail(id);
            if (data == "yes")
            {
                return RedirectToAction("ViewUploads", "Admin", new { id });
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        public IActionResult SendAgreement(int id, SendMailVM vm)
        {
            var data = _service.GetReqType(id, vm);
            return PartialView("_SendAgreement", data);
        }

        [HttpPost]
        public IActionResult SendAgreementPost(SendMailVM vm, int id)
        {
            SendMailVM data = _service.SendAgreement(id);
            AspNetUser aspdata = _service.AspUserData(vm.Email);
            if (data.ReqId != 0)
            {
                var receiver = vm.Email;
                var idd = data.ReqId;
                var token = _token.GenerateJwtToken(aspdata);
                var subject = "Create Account";
                var message = $"Tap on link for accept agreement: [1] http://localhost:5093/Patient/Agreement?token=" + token + "&id=" + idd;




                var mail = "tatva.dotnet.dhruvrajsinhvaghela@outlook.com";
                var password = "Vagheladhruv@123";

                var client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
                return RedirectToAction(nameof(AdminDashboard));
            }

            return NotFound();

        }

        public IActionResult SendOrder(int id, SendOrderVM vm)
        {
            var data = _service.GetProfessions(id, vm);
            return PartialView("_SendOrder", data);//,data
        }

        public JsonResult SelectVendor(int ProfId)
        {
            var data = _service.GetVendorNames(ProfId); // Assuming this method returns physicians based on the region ID

            if (data != null)
            {
                return Json(data.Select(x => new { x.VendorName, x.VendorId })); // Return a single-item array
            }
            else
            {
                return Json(Array.Empty<HealthProfessional>()); // Return an empty array
            }
        }

        public JsonResult VendorData(int vendorId)
        {
            var data = _service.GetVendorData(vendorId);
            return Json(data);
        }

        [HttpPost]
        public IActionResult AddSendOrders(int id, SendOrderVM vm)
        {
            var data = _service.AddOrderData(id, vm);
            if (data == true)
            {
                return View("AdminDashboard");//,data
            }
            return NotFound();
        }

        public IActionResult ClearCase(int id)
        {
            var data = _service.GetClearCase(id);
            return PartialView("_ClearCaseModal", data);
        }

        [HttpPost]
        public IActionResult UpStatusClearCase(int id)
        {
            var data = _service.UpStatusClear(id);
            if (data == true)
            {
                return RedirectToAction(nameof(AdminDashboard));
            }
            return NotFound();
        }


        public IActionResult CloseCase(int id)
        {
            var data = _service.GetCloseCase(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult UpCloseCaseUser(int id, CloseCaseVM vm)
        {
            bool data = _service.UpCloseCase(id, vm);
            if (data == true)
            {
                return RedirectToAction(nameof(AdminDashboard));
            }
            return NotFound();
        }


        public IActionResult NavTabs(int nav)
        {
            AdminDashboardVM swc = new();
            var data = _service.PatientStatus(swc);

            switch (nav)
            {
                case 1: return PartialView("_AdminHome", data);/*,data*/

                case 3:
                    {
                        var aspId = HttpContext.Session.GetInt32("userId");
                        var adminId = HttpContext.Session.GetInt32("adminId");
                        var ProfileData = _service.AdminProfileData(aspId, adminId);
                        return PartialView("_AdminProfile", ProfileData);
                    }
                case 4:
                    {
                        List<ProviderInformation> regions  = _service.GetProviderRegion();

                        return PartialView("_ProviderTab", regions);
                    }

                default: return PartialView("_AdminNewStateData");
            }
        }

        public IActionResult Page(int status, int pn, int item, string Keyword, int Reqtype, int RegionId)
        {
            List<AdminDashboardVM> data = _service.GetFilteredData(Keyword, RegionId, status, Reqtype, pn, item);

            return status switch
            {
                1 => PartialView("_AdminNewStateData", data),/*,data*/
                2 => PartialView("_AdminPendingstateData", data),
                3 => PartialView("_AdminActiveStateData", data),
                4 => PartialView("_AdminConcludeStateData", data),
                5 => PartialView("_AdminToCLoseStatusData", data),
                6 => PartialView("_AdminUnpaidStatusData", data),
                _ => PartialView("_NewState", data),
            };
        }
        [HttpGet]
        public IActionResult SendLink()
        {
            return PartialView("_SendLinkModal");
        }
        [HttpPost]
        public IActionResult SendLink(SendMailVM vm)
        {
            if (vm.Email != null)
            {
                var receiver = vm.Email;
                var subject = "Create Request";
                var message = $"Tap on link for Creating Request: [1] https://localhost:5093/Login/PatientSubmitRequest";




                var mail = "tatva.dotnet.dhruvrajsinhvaghela@outlook.com";
                var password = "Vagheladhruv@123";

                var client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
                return RedirectToAction(nameof(AdminDashboard));
            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult SortByFilter(string keywrd, int RegId, int status, int reqType, int pn, int item)
        {
            List<AdminDashboardVM> data = _service.GetFilteredData(keywrd, RegId, status, reqType, pn, item);
            return status switch
            {
                1 => PartialView("_AdminNewStateData", data),
                2 => PartialView("_AdminPendingstateData", data),
                3 => PartialView("_AdminActiveStateData", data),
                4 => PartialView("_AdminConcludeStateData", data),
                5 => PartialView("_AdminToCLoseStatusData", data),
                6 => PartialView("_AdminUnpaidStatusData", data),
                _ => PartialView("_NewState", data),
            };
        }

        public IActionResult Export(int status, int Regionid, string Keyword, int ReqType)
        {
            try
            {
                List<AdminDashboardVM> data = _service.GetFilteredData(Keyword, Regionid, status, ReqType, 0, 0);
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Data");


                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Date Of Birth";
                worksheet.Cell(1, 3).Value = "Requestor";
                worksheet.Cell(1, 4).Value = "Physician Name";
                worksheet.Cell(1, 5).Value = "Date of Service";
                worksheet.Cell(1, 6).Value = "Requested Date";
                worksheet.Cell(1, 7).Value = "Phone Number";
                worksheet.Cell(1, 8).Value = "Address";
                worksheet.Cell(1, 9).Value = "Notes";
                worksheet.Cell(1, 10).Value = "Request Type";
                worksheet.Cell(1, 11).Value = "Status";

                int row = 2;
                foreach (var item in data)
                {
                    var statusClass = "";
                    if (item.RequestType == 1)
                    {
                        statusClass = "Business";
                    }
                    else if (item.RequestType == 4)
                    {
                        statusClass = "Concierge";
                    }
                    else if (item.RequestType == 2)
                    {
                        statusClass = "Patient";
                    }
                    else
                    {
                        statusClass = "Family/Friend";
                    }
                    var s = "";
                    if (item.Status == 1)
                    {
                        s = "New";
                    }
                    else if (item.Status == 2)
                    {
                        s = "Pending";
                    }
                    else if (item.Status == 4 || item.Status == 5)
                    {
                        s = "Active";
                    }
                    else if (item.Status == 6)
                    {
                        s = "Conclude";
                    }
                    else if (item.Status == 7 || item.Status == 3 || item.Status == 8)
                    {
                        s = "To Close";
                    }
                    else
                    {
                        s = "Unpaid";
                    }
                    worksheet.Cell(row, 1).Value = item.PatientName;
                    worksheet.Cell(row, 2).Value = DateTime.Parse(item.BirthDate.ToString()??"");
                    worksheet.Cell(row, 3).Value = item.RequestorName + item.RequestorLastName;
                    worksheet.Cell(row, 4).Value = item.ProviderName;
                    worksheet.Cell(row, 5).Value = item.RequestDate.ToString();
                    worksheet.Cell(row, 6).Value = item.RequestDate.ToString();

                    if (item.PhoneNumber != "")
                    {
                        worksheet.Cell(row, 7).Value = item.PhoneNumber;
                    }
                    if (item.RequestType != 2 && item.RequestorPhoneNumber != "")
                    {
                        worksheet.Cell(row, 7).Value = item.PhoneNumber + ' ' + item.RequestorPhoneNumber;
                    }
                    worksheet.Cell(row, 8).Value = item.Address;
                    worksheet.Cell(row, 9).Value = item.Notes?.ToString();
                    worksheet.Cell(row, 10).Value = statusClass;
                    worksheet.Cell(row, 11).Value = s;
                    row++;
                }
                worksheet.Columns().AdjustToContents();

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "usersData.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
        public IActionResult ExportAll()
        {

            try
            {
                List<AdminDashboardVM> data = _service.GetNewStateData(0, 0);
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Data");


                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Date Of Birth";
                worksheet.Cell(1, 3).Value = "Requestor";
                worksheet.Cell(1, 4).Value = "Physician Name";
                worksheet.Cell(1, 5).Value = "Date of Service";
                worksheet.Cell(1, 6).Value = "Requested Date";
                worksheet.Cell(1, 7).Value = "Phone Number";
                worksheet.Cell(1, 8).Value = "Address";
                worksheet.Cell(1, 9).Value = "Notes";
                worksheet.Cell(1, 10).Value = "Request Type";
                worksheet.Cell(1, 11).Value = "Status";

                int row = 2;
                foreach (var item in data)
                {
                    var statusClass = "";
                    if (item.RequestType == 1)
                    {
                        statusClass = "Business";
                    }
                    else if (item.RequestType == 4)
                    {
                        statusClass = "Concierge";
                    }
                    else if (item.RequestType == 2)
                    {
                        statusClass = "Patient";
                    }
                    else
                    {
                        statusClass = "Family/Friend";
                    }
                    var s = "";
                    if (item.Status == 1)
                    {
                        s = "New";
                    }
                    else if (item.Status == 2)
                    {
                        s = "Pending";
                    }
                    else if (item.Status == 4 || item.Status == 5)
                    {
                        s = "Active";
                    }
                    else if (item.Status == 6)
                    {
                        s = "Conclude";
                    }
                    else if (item.Status == 7 || item.Status == 3 || item.Status == 8)
                    {
                        s = "To Close";
                    }
                    else
                    {
                        s = "Unpaid";
                    }
                    worksheet.Cell(row, 1).Value = item.PatientName;
                    worksheet.Cell(row, 2).Value = DateTime.Parse(item.BirthDate.ToString()??"");
                    worksheet.Cell(row, 3).Value = item.RequestorName + item.RequestorLastName;
                    worksheet.Cell(row, 4).Value = item.ProviderName;
                    worksheet.Cell(row, 5).Value = item.RequestDate.ToString();
                    worksheet.Cell(row, 6).Value = item.RequestDate.ToString();

                    if (item.PhoneNumber != "")
                    {
                        worksheet.Cell(row, 7).Value = item.PhoneNumber;
                    }
                    if (item.RequestType != 2 && item.RequestorPhoneNumber != "")
                    {
                        worksheet.Cell(row, 7).Value = item.PhoneNumber + ' ' + item.RequestorPhoneNumber;
                    }
                    worksheet.Cell(row, 8).Value = item.Address;
                    worksheet.Cell(row, 9).Value = item.Notes?.ToString();
                    worksheet.Cell(row, 10).Value = statusClass;
                    worksheet.Cell(row, 11).Value = s;
                    row++;
                }
                worksheet.Columns().AdjustToContents();

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "usersData.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }

        }

        [HttpPost]
        public IActionResult EditAdminProfile(AdminProfileVM model, List<int> reg)
        {
            int adminId=0;
            if (reg is null)
            {
                throw new ArgumentNullException(nameof(reg));
            }
            if(HttpContext.Session.GetInt32("adminId") !=null)
            {
                adminId = (int)HttpContext.Session.GetInt32("adminId")!;
            }
            //ViewBag.Username = _service.Adminname(admin);
            _service.EditAdminProfile(model, adminId);
            return RedirectToAction("AdminDashboard");
        }

        /*   [HttpPost]
           public IActionResult EditAdminp(Profile model)
           {
               int admin = (int)HttpContext.Session.GetInt32("Id");
               _service.editadminp(model, admin);
               return RedirectToAction("Admin_profile");
           }*/

        [HttpGet]
        public IActionResult CreateRequest()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RequestSupport()
        {
            return PartialView("_RequestSupport");
        }

        [HttpGet]
        public IActionResult ProviderByRegion(int regionId)
        {
            var data = _service.GetProviderInfo(regionId);
            return PartialView("_ProviderInfo",data);
        }
    }
}
