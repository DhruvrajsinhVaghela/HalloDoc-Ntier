using HalloDoc.DbEntity.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using HalloDoc.Service.Interface;
using HalloDoc.Services.Interfaces;
using HalloDoc.Auth;
using System.IdentityModel.Tokens.Jwt;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("Patient")]

    public class PatientController : Controller
    {
        private readonly IPatientService _service;
        private readonly IJwtToken _token;
        public PatientController(IPatientService service, IJwtToken token)
        {
            _service = service;
            _token = token;
        }

       

        //-------------------------------Patient Dashboard
        
        public IActionResult PatientDashboard(int id)
        {
            var dash = _service.PatientDashboard(id);
            return View(dash);
        }

        //-------------------------------Patient View Documents
        public IActionResult PatientViewDocuments(int id)
        {
            List<PatientDashboardVM> View_doc = new List<PatientDashboardVM>();
            var res = _service.PatientViewDocuments(id).ToList();
            res.ForEach(item =>
            {
                View_doc.Add(new PatientDashboardVM
                {
                    View = item
                });

            });
            return View(View_doc);
        }

        //-------------Document Download
        public IActionResult Download(int id)//6//22
        {

            var file = _service.Download(id);
            //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
            var path = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", file.FileName);
        }

        //-------------Document Download All
        public IActionResult DownloadAll(PatientDashboardVM vm,int id)
        {
            var filesRow = _service.DownloadAll(vm,id);
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

        //-------------PatientDetails Update
        public IActionResult Update(int id, ViewDocumentVM vm)
        {
            var data = _service.Update(id, vm);
            if (data == "yes")
            {
                return RedirectToAction(nameof(PatientDashboard), new { id = id });
            }
            return View();
        }

        //-------------PatientFileSave
        public IActionResult PatientFileSave(int id, PatientDashboardVM model)
        {
            var data = _service.PatientFileSave(id, model);
            if (data == "yes")
            {
                return RedirectToAction("PatientViewDocuments", "Patient", new { id = id });
            }

            return View();
        }

        public IActionResult PatientMeRequest(int id, PatientInfoVM model)//int id, ViewDocumentVM model
        {
            var data = _service.PatientMeRequest(id, model);
            if (data != null)
            {
                return View(data);
            }
            return View();
        }

        public IActionResult PatientSomeOneElseRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PatientSomeOneElseRequest(int id, PatientInfoVM model)
        {
            var data = _service.PatientSomeOneElseRequest(id, model);
            if (data != null)
            {
                return View(data);
            }
            return View();
        }
        /* public async Task<IActionResult> PatientFileSave(int id, ViewDocumentVM model)
         {

         }*/

        
        
        public IActionResult Agreement(string token, int id)
        {
            var data = _service.GetRequest(id);
            //return View(data);
            bool x=_token.  
                ValidateJwtToken(token, out JwtSecurityToken jwtSecurityToken);
            if (x)
            {
                return View(data);
            }
            return NotFound();
        }

        public IActionResult PatientAgreed(SendAgreementVM vm,int id)
        {
            var x=_service.GetAgree(id);
            if(x==true)
            {
                return RedirectToAction("PatientDashboard","Patient",new { id = id });
            }
            return NotFound();
        }
         
        public IActionResult PatientCanceled(SendAgreementVM vm,int id)
        {
            var x =_service.CancelAgreement(id,vm);
            if (x == true)
            {
                return RedirectToAction("PatientDashboard", "Patient", new { id = id });
            }
            return NotFound();
        }

       

    }
}