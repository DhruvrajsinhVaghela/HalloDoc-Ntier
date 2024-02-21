using HalloDoc.DbEntity.Data;
using HalloDoc.DbEntity.ViewModels;
using HalloDoc.Repositories.Implementation;
using HalloDoc.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HalloDoc.DbEntity.Models;
using System.Web.Helpers;
using Microsoft.PowerBI.Api.Models;
using Microsoft.PowerBI.Api;
using System.IO.Compression;
using System.Runtime.Intrinsics.X86;
using HalloDoc.services.Interface;
using HalloDoc.DbEntity.ViewModel;

namespace HalloDoc.Controllers
{

    public class PatientController : Controller
    {
        private readonly IPatientService _service;
        public PatientController(IPatientService service)
        {
            _service = service;
        }

        //-------------------Patient Site
        public IActionResult PatientSite()
        {
            return View();
        }

        //------------------Patient Login
        public IActionResult PatientLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientLogin(GetAuthenticationVM test)
        {
            if (!ModelState.IsValid)
            {
                return View(test);
            }

            var data =await _service.PatientLogin(test);

            if (data > 0)
            {

                HttpContext.Session.SetInt32("userId", data);
                /*HttpContext.Session.SetString("userName", userIdObj.FirstName);*/
                return RedirectToAction(nameof(PatientDashboard), "Patient", new { id = data });
            }
            return View();


        }
        //------------------Patient Reset PW
        public IActionResult PatientResetPw()
        {
            return View();
        }

        //------------------Patient Submit Request

        public IActionResult PatientSubmitRequest()
        {
            return View();
        }

        //-----------------   Patient Info
        public IActionResult PatientInfoForm()
        {
            return View();
        }

        public async Task<IActionResult> validate_Email(System.String email)
        {
            var ans = await _service.validate_Email(email);
            if (ans == false)
            {
                return Json(new { exist = false });
            }
            return Json(new { exist = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientInfoForm([FromForm] PatientInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var data = _service.PatientInfoForm(model);
            if (data == "yes")
            {
                return RedirectToAction("PatientSite", "Patient");
            }
            return View();
        }

        //-----------------Patient Family Friend Form
        public IActionResult PatientFamilyFriendForm()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> PatientFamilyFriendForm(PatientFamilyFriendInfo model)
        //{


        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var data = _patient.PatientFamilyFriendForm(model); 
        //    if (data == "yes") 
        //    {
        //        return RedirectToAction("PatientSite", "Patient");
        //    }
        //    return View();
        //}

        //-----------------Patient Concierge Info Form
        public IActionResult PatientConciergeInfo()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> PatientConciergeInfo([FromForm] PatientConciergeInfo model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var data = _patient.PatientConciergeForm(model);
        //    {
        //        if (data == "yes")
        //        {
        //            return RedirectToAction("PatientSite", "Patient");
        //        }
        //        return View();
        //    }
            
        //}


        //---------------Patient Business Info Form
        public IActionResult PatientBusinessInfo()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> PatientBusinessInfo([FromForm] PatientBusinessInfo model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var data = _patient.PatientBusinessForm(model);
        //    {
        //        if(data == "yes")
        //        {
        //            return RedirectToAction("PatientSite", "Patient");
        //        }
        //        return View();
        //    }
        //}

        //-------------------------------Patient Dashboard
        //public IActionResult PatientDashboard(int id)
        //{
        //    var dash = _service.PatientDashboard(id);
        //    return View(dash);
        //}

        //-------------------------------Patient View Documents
        //public IActionResult PatientViewDocuments(int id)
        //{
        //    List<PatientDashboardVM> View_doc = new List<PatientDashboardVM>();
        //    var res = _patient.PatientViewDocuments(id).ToList();
        //    res.ForEach(item =>
        //    {
        //        View_doc.Add(new PatientDashboardVM
        //        {
        //            View = item
        //        }); 

        //    });
        //    return View(View_doc);
        //}

        //-------------Document Download
        //public IActionResult Download(int id)//6//22
        //{

        //    var file = _patient.Download(id);
        //    //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
        //    var path = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
        //    var bytes = System.IO.File.ReadAllBytes(path);
        //    return File(bytes, "application/octet-stream", file.FileName);
        //}

        //-------------Document Download All
        //public IActionResult DownloadAll(int id)
        //{
        //    var filesRow = _patient.DownloadAll(id);
        //    MemoryStream ms = new MemoryStream();
        //    using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
        //        filesRow.ForEach(file =>
        //        {
        //            var path = "D:\\Project\\HalloDoc-Ntier\\HalloDoc\\wwwroot\\UploadFiles\\" + file.FileName;
        //            //D:\\Project\\HalloDoc1\\HalloDoc_Dotnet\\HalloDoc\\wwwroot\\UploadedFiles\\
        //            ZipArchiveEntry zipEntry = zip.CreateEntry(file.FileName);
        //            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        //            using (Stream zipEntryStream = zipEntry.Open())
        //            {
        //                fs.CopyTo(zipEntryStream);
        //            }
        //        });
        //    return File(ms.ToArray(), "application/zip", "download.zip");
        //}

        //-------------PatientDetails Update
        //public async Task<IActionResult> Update(int id, ViewDocumentVM vm)
        //{
        //    var data=_patient.Update(id, vm);
        //    if (data == "yes") 
        //    {
        //        return RedirectToAction(nameof(PatientDashboard), new { id = id });
        //    }
        //    return View();
        //}

        //-------------PatientFileSave
        //public async Task<IActionResult> PatientFileSave(int id, PatientDashboardVM model)
        //{
        //    var data=_patient.PatientFileSave(id, model);
        //    if (data == "yes")
        //    {
        //        return RedirectToAction("PatientViewDocuments", "Patient", new { id = id });
        //    }
        //    return View();
        //}

        //public async Task<IActionResult> PatientMeRequest(int id,PatientInfo model)//int id, ViewDocumentVM model
        //{
        //    var data=_patient.PatientMeRequest(id, model);
        //    if(data != null)
        //    {
        //        return View(data);
        //    }
        //    return View();
        //}

        public IActionResult PatientSomeOneElseRequest()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult PatientSomeOneElseRequest(int id,PatientInfo model)
        //{
        //    var data=_patient.PatientSomeOneElseRequest(id, model);
        //    if(data != null)
        //    { 
        //        return View(data);
        //    }
        //    return View();
        //}
        /* public async Task<IActionResult> PatientFileSave(int id, ViewDocumentVM model)
         {

         }*/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}