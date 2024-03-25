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
        [HttpGet("patient/site/login/dahboard")]
        public IActionResult PatientDashboard(int id)
        {
            var dash = _service.PatientDashboard(id);
            return View(dash);
        }

        //-------------------------------Patient View Documents
        [HttpGet("patient/site/login/dashboard/view-documents")]
        public IActionResult PatientViewDocuments(int id)
        {
            List<PatientDashboardVM> View_doc = new();
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
        [HttpGet]
        public IActionResult Download(int id)
        {

            var file = _service.Download(id);
            var path = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", file.FileName);
        }

        //-------------Document Download All
        /*[HttpGet]*/
        public IActionResult DownloadAll(PatientDashboardVM vm,int id)
        {
            var filesRow = _service.DownloadAll(vm,id);
            MemoryStream ms = new();
            using (ZipArchive zip = new(ms, ZipArchiveMode.Create, true))
                filesRow.ForEach(file =>
                {
                    var path = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
                    ZipArchiveEntry zipEntry = zip.CreateEntry(file.FileName);
                    using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
                    using Stream zipEntryStream = zipEntry.Open();
                    fs.CopyTo(zipEntryStream);
                });
            return File(ms.ToArray(), "application/zip", "download.zip");
        }

        //-------------PatientDetails Update
        public IActionResult Update(int id, ViewDocumentVM vm)
        {
            var data = _service.Update(id, vm);
            if (data == "yes")
            {
                return RedirectToAction(nameof(PatientDashboard), new { id });
            }
            return View();
        }

        //-------------PatientFileSave
        public IActionResult PatientFileSave(int id, PatientDashboardVM model)
        {
            var data = _service.PatientFileSave(id, model);
            if (data == "yes")
            {
                return RedirectToAction("PatientViewDocuments", "Patient", new { id });
            }

            return View();
        }

        [HttpGet("patient/site/login/dashboard/me-request")]
        public IActionResult PatientMeRequest(int id, PatientInfoVM model)
        {
            var data = _service.PatientMeRequest(id, model);
            if (data != null)
            {
                return View(data);
            }
            return View();
        }

        [HttpGet("patient/site/login/dashboard/some-else-request")]
        public IActionResult PatientSomeOneElseRequest()
        {
            return View();
        }

        [HttpPost("patient/site/login/dashboard/some-else-request")]
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
            bool x =_token.  
                ValidateJwtToken(token, out _);
            if (x)
            {
                return View(data);
            }
            return NotFound();
        }

        [HttpGet("patient/agreed-at-agreement")]
        public IActionResult PatientAgreed(int id)
        {


            var x=_service.GetAgree(id);
            if(x==true)
            {
                return RedirectToAction("PatientDashboard","Patient",new { id });
            }
            return NotFound();
        }
         
        public IActionResult PatientCanceled(SendAgreementVM vm,int id)
        {
            var x =_service.CancelAgreement(id,vm);
            if (x == true)
            {
                return RedirectToAction("PatientDashboard", "Patient", new { id });
            }
            return NotFound();
        }

       

    }
}