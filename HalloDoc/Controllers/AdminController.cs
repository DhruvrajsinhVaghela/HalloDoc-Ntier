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

        public AdminController(IAdminService service,IJwtToken token)
        {
            _service = service;
            _token= token;
        }
      
        public IActionResult AdminDashboard(AdminDashboardVM swc)
        {
            var data = _service.PatientStatus(swc);
            return View(data);
           
        }

        public IActionResult CardsView(int status,int pn,int item)
        {

            List<AdminDashboardVM> data = _service.GetDataPagination(status, pn, item);

            switch (status)
            {
                case 1: return PartialView("_AdminNewStateData", data);/*,data*/
                case 2: return PartialView("_AdminPendingstateData",data);
                case 3: return PartialView("_AdminActiveStateData",data);
                case 4: return PartialView("_AdminConcludeStateData",data);
                case 5: return PartialView("_AdminToCLoseStatusData", data);
                case 6: return PartialView("_AdminUnpaidStatusData", data);
                default: return PartialView("_NewState", data);
            }
        }
        public IActionResult ViewReservation(int id)
        {
            var data = _service.ViewPatientData(id);
            return View(data);
        }

        public IActionResult ViewNotes(int id)
        {
           ViewNotesVM data = _service.ViewNotes2(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult ViewNotes(int id, RequestNote vm)
        {
            _service.ViewNotes(id,vm);
            return RedirectToAction("ViewNotes", new { id = id , vm = vm});
        }

        public IActionResult CancelCase(int id, CancelCaseVM vm)
        {
            var data=_service.CancelCaseData(id);
            return PartialView("_CancleCaseModal",data);
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id,CancelCaseVM vm)
        {
            _service.UpStatus(id,vm);
            return RedirectToAction("AdminDashboard", new { id = id });
        }


        public IActionResult AssignCase(int id,AssignCaseVM vm)
        {
            
            var phy = _service.GetPhysician(id,vm);
            
            return PartialView("_AssignCaseModal",phy);
        }

        public IActionResult TransferCase(int id,AssignCaseVM vm)
        {
            var phy=_service.GetPhysician(id, vm);

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
                return Json(new Physician[0]); // Return an empty array
            }
        }

        [HttpPost]
        public IActionResult UpdateAssignCase(int id, AssignCaseVM vm)
        {
            _service.UpAssignStatus(id, vm);
            return RedirectToAction("AdminDashboard", new { id = id });
        }

        public IActionResult BlockCase(int id, BlockCaseVM vm)
        {
            var data = _service.BlockCaseData(id,vm);
            return PartialView("_BlockCaseModal", data);
        }

        [HttpPost]
        public IActionResult UpdateBlockCase(int id, BlockCaseVM vm)
        {
            _service.UpBlockCase(id, vm);
            return View("AdminDashboard", new { id = id });
        }

        public IActionResult ViewUploads(int id)
        {
            List<PatientDashboardVM> View_doc = new List<PatientDashboardVM>();
            var res = _service.PatientViewDocuments(id);
            /*res.ForEach(item =>
            {
                View_doc.Add(new PatientDashboardVM
                {
                    View = item
                });

            });
            res.*/
            return View(res);
        }
        public IActionResult PatientFileSave(int id, PatientDashboardVM model)
        {
            var data = _service.PatientFileSave(id, model);
            if (data == "yes")
            {
                return RedirectToAction("ViewUploads", "Admin", new { id = id });
            }

            return View();
        }

        public IActionResult DownloadAll(int id)
        {
            var filesRow = _service.DownloadAll(id);

            MemoryStream ms = new MemoryStream();
            using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                filesRow.ForEach(file =>
                {
                    var path = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
                    //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
                    ZipArchiveEntry zipEntry = zip.CreateEntry(file.FileName);
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (Stream zipEntryStream = zipEntry.Open())
                    {
                        fs.CopyTo(zipEntryStream);
                    }
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
            RequestWiseFile data=_service.Delete(id);
            return RedirectToAction("ViewUploads", "Admin", new { id = data.RequestId });

        }

        public IActionResult DeleteAll(int id)
        {
            var filesRow = _service.DeleteAll(id);
            return RedirectToAction("AdminDashboard", "Admin");
        }

        public IActionResult SendMail(int id)
        {
            var data = _service.SMail(id);
            if(data == "yes")
            {
                return RedirectToAction("ViewUploads", "Admin", new { id = id });
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        public IActionResult SendAgreement(int id,SendMailVM vm)
        {
            var data=_service.GetReqType(id,vm);
            return PartialView("_SendAgreement",data);
        }

        [HttpPost]
        public IActionResult SendAgreementPost(SendMailVM vm,int id)
        {
            SendMailVM data = _service.SendAgreement(id);
            AspNetUser aspdata = _service.AspUserData(vm.Email);
            if (data.reqId!=0)
            {
                var receiver = vm.Email;
                var idd = data.reqId;
                var token = _token.GenerateJwtToken(aspdata);
                var subject = "Create Account";
                var message = $"Tap on link for accept agreement: [1] https://localhost:5093/Patient/Agreement?token="+token+"&id="+idd;




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
            return PartialView("_SendOrder",data);//,data
        }

        public JsonResult SelectVendor(int ProfId)
        {
            var data = _service.GetVendorNames(ProfId); // Assuming this method returns physicians based on the region ID

            if (data != null)
            {
                return Json(data.Select(x => new {x.VendorName, x.VendorId})); // Return a single-item array
            }
            else
            {
                return Json(new HealthProfessional[0]); // Return an empty array
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
            if(data==true)
            {
                return View("AdminDashboard");//,data
            }
            return NotFound();
        }

        public IActionResult ClearCase(int id)
        {
            var data = _service.GetClearCase(id);
            return PartialView("_ClearCaseModal",data);
        }

        [HttpPost]
        public IActionResult UpStatusClearCase(int id)
        {
            var data = _service.UpStatusClear(id);
            if(data==true)
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
        public IActionResult UpCloseCaseUser(int id,CloseCaseVM vm)
        {
            bool data=_service.UpCloseCase(id,vm);
            if(data==true)
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
                case 1: return PartialView("_AdminHome",data);/*,data*/
               
                case 3:
                    {
                        var adminId=HttpContext.Session.GetInt32("userId");
                        var ProfileData = _service.AdminProfileData(adminId);
                        return PartialView("_AdminProfile",ProfileData);
                    }
               
                default: return PartialView("_AdminNewStateData");
            }
        }

        public IActionResult Page(int status, int pn, int item)
        {
            List<AdminDashboardVM> data = _service.GetDataPagination(status,pn,item);

            switch (status)
            {
                case 1: return PartialView("_AdminNewStateData", data);/*,data*/
                case 2: return PartialView("_AdminPendingstateData", data);
                case 3: return PartialView("_AdminActiveStateData", data);
                case 4: return PartialView("_AdminConcludeStateData", data);
                case 5: return PartialView("_AdminToCLoseStatusData", data);
                case 6: return PartialView("_AdminUnpaidStatusData", data);
                default: return PartialView("_NewState", data);
            }
        }
        public IActionResult SendLink()
        {
            return PartialView("_SendLinkModal");
        }

        [HttpPost]
        public IActionResult SortByFilter([FromBody] Filter filter) 
        {
            List<AdminDashboardVM> data = _service.GetFilteredData(filter.keywrd, filter.RegId, filter.status, filter.reqType);
            switch (filter.status)
            {
                case 1: return PartialView("_AdminNewStateData", data);/*,data*/
                case 2: return PartialView("_AdminPendingstateData", data);
                case 3: return PartialView("_AdminActiveStateData", data);
                case 4: return PartialView("_AdminConcludeStateData", data);
                case 5: return PartialView("_AdminToCLoseStatusData", data);
                case 6: return PartialView("_AdminUnpaidStatusData", data);
                default: return PartialView("_NewState", data);
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
                    var dos = "";
                    var notes = "";
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
                    worksheet.Cell(row, 2).Value = DateTime.Parse(item.BirthDate.ToString());
                    worksheet.Cell(row, 3).Value = item.RequestorName;
                    worksheet.Cell(row, 4).Value = item.ProviderName;
                    worksheet.Cell(row, 5).Value = item.RequestDate.ToString();
                    worksheet.Cell(row, 6).Value = item.RequestDate.ToString();

                    if (item.Phonenumber != "")
                    {
                        worksheet.Cell(row, 7).Value = item.Phonenumber;
                    }
                    if (item.RequestType != 2 && item.RequestorPhoneNumber != "")
                    {
                        worksheet.Cell(row, 7).Value = item.Phonenumber + ' ' + item.RequestorPhoneNumber;
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

    }
}
