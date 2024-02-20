using HalloDoc.DbEntity.Data;
using HalloDoc.ViewModels;
using HalloDoc.Models;
using HalloDoc.Repositories.Implementation;
using HalloDoc.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HalloDoc.DbEntity.Models;
using System.Web.Helpers;
using Microsoft.PowerBI.Api.Models;
using Microsoft.PowerBI.Api;

namespace HalloDoc.Controllers
{

    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatient _patient;
        public PatientController(ILogger<PatientController> logger, IPatient patient)
        {
            _logger = logger;
            _patient = patient;
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
        public async Task<IActionResult> PatientLogin([Bind("Email,PasswordHash")] DbEntity.Models.AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return View(aspNetUser);
            }

            var data = await _patient.PatientLogin(aspNetUser);

            if (data > 0)
            {

                HttpContext.Session.SetInt32("userId", data);
                /*HttpContext.Session.SetString("userName", userIdObj.FirstName);*/
                return RedirectToAction(nameof(PatientDashboard), "Patient", new { id = data });
            }
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
            var ans = await _patient.validate_Email(email);
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
            var data = _patient.PatientInfoForm(model);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientFamilyFriendForm(PatientFamilyFriendInfo model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var data = _patient.PatientFamilyFriendForm(model); 
            if (data == "yes") 
            {
                return RedirectToAction("PatientSite", "Patient");
            }
            return View();
        }

        //-----------------Patient Concierge Info Form
        public IActionResult PatientConciergeInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientConciergeInfo([FromForm] PatientConciergeInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = _patient.PatientConciergeForm(model);
            {
                if (data == "yes")
                {
                    return RedirectToAction("PatientSite", "Patient");
                }
                return View();
            }
            
        }


        //---------------Patient Business Info Form
        public IActionResult PatientBusinessInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientBusinessInfo([FromForm] PatientBusinessInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = _patient.PatientBusinessForm(model);
            {
                if(data == "yes")
                {
                    return RedirectToAction("PatientSite", "Patient");
                }
                return View();
            }
        }

        //-------------------------------Patient Dashboard
        public IActionResult PatientDashboard(int id)
        {
            var dash = _patient.PatientDashboard(id);
            return View(dash);
        }

        //-------------------------------Patient View Documents
        public IActionResult PatientViewDocuments(int id)
        {
            List<PatientDashboardVM> View_doc = new List<PatientDashboardVM>();
            var res = _patient.PatientViewDocuments(id).ToList();
            res.ForEach(item =>
            {
                View_doc.Add(new PatientDashboardVM
                {
                    View = item
                }); 

            });
            return View(View_doc);
        }

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