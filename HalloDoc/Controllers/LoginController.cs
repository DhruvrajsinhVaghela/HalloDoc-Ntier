using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Service.Interface;
using HalloDoc.services.Interface;
using HalloDoc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.PowerBI.Api.Models;

namespace HalloDoc.Controllers
{
    public class LoginController : Controller
    {

        private readonly IAdminService _Aservice;
        private readonly IJwtToken _token;
        private readonly IPatientService _Pservice;
        public LoginController(IAdminService Aservice, IJwtToken token,IPatientService Pservice)
        {
            _Aservice = Aservice;
            _token = token;
            _Pservice=Pservice;
        }

        //Admin Login----------------------------------------
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return View(aspNetUser);
            }

            AspNetUser data = _Aservice.AdmintLogin(aspNetUser);

            if (data.Id > 0)
            {
                HttpContext.Session.SetInt32("userId", data.Id);
                HttpContext.Session.SetString("User'sName", data.UserName??"");
                var token = _token.GenerateJwtToken(data);
                Response.Cookies.Append("jwt", token);
                //HttpContext.Session.SetString("userName", userIdObj.FirstName);
                return RedirectToAction(nameof(AdminDashboard), "Admin", new { id = data });
            }
            return View();

        }

        public IActionResult AdminLogout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction(nameof(AdminLogin), "Login");
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
        public IActionResult PatientLogin(AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return View(aspNetUser);
            }

            var data = _Pservice.PatientLogin(aspNetUser);

            if (data.Id > 0)
            {


                HttpContext.Session.SetInt32("userId", data.Id);
                var token = _token.GenerateJwtToken(data);
                Response.Cookies.Append("jwt", token);
                //HttpContext.Session.SetString("userName", userIdObj.FirstName);
                return RedirectToAction("PatientDashboard", "Patient", new { id = data.Id });
            }
            return View();


        }
        //------------------Patient Reset PW
        public IActionResult PatientResetPw()
        {
            return View();
        }
        [HttpPost]
        public IActionResult P_Forgetpass(AspNetUser preq)
        {
            bool isRegistered = _Pservice.ValidateUserByEmail(preq.Email);
            if (isRegistered)
            {
                var receiver = preq.Email ?? "";

                var subject = "Create Account";
                var message = "Tap on link for Create Account: https://localhost:44308/Patient/PatientResetPw";


                var mail = "tatva.dotnet.dhruvrajsinhvaghela@outlook.com";
                var password = "Vagheladhruv@123";

                var client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
                return RedirectToAction(nameof(PatientSubmitRequest));
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
            var ans = await _Pservice.validate_Email(email);
            if (ans == false)
            {
                return Json(new { exist = false });
            }
            return Json(new { exist = true });
        }

        [HttpPost]
        public IActionResult PatientInfoForm([FromForm] PatientInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var data = _Pservice.PatientInfoForm(model);
            if (data == "yes")
            {
                return RedirectToAction("PatientSite", "Login");
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
        public IActionResult PatientFamilyFriendForm(PatientFamilyFriendInfo model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var data = _Pservice.PatientFamilyFriendForm(model);
            if (data == "yes")
            {
                return RedirectToAction("PatientSite", "Login");
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
        public IActionResult PatientConciergeInfo([FromForm] PatientConciergeInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = _Pservice.PatientConciergeForm(model);
            {
                if (data == "yes")
                {
                    return RedirectToAction("PatientSite", "Login");
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
        public IActionResult PatientBusinessInfo([FromForm] PatientBusinessInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = _Pservice.PatientBusinessForm(model);
            {
                if (data == "yes")
                {
                    return RedirectToAction("PatientSite", "Login");
                }
                return View();
            }
        }
        public IActionResult PatientLogout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction(nameof(PatientLogin), "Login");
        }
        public IActionResult EncounterForm()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
