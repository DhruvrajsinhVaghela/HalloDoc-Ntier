using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Service.Interface;
using HalloDoc.services.Interface;
using HalloDoc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.PowerBI.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;

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
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {

                AspNetUser data = _Aservice.AdmintLogin(aspNetUser);

                if (data.Id > 0)
                {
                    HttpContext.Session.SetInt32("userId", data.Id);//asp net user id in session
                    HttpContext.Session.SetString("User'sName", data.UserName ?? "");
                    //---for admin id
                    Admin admin = _Aservice.GetAdminDataById(data.Id);
                    HttpContext.Session.SetInt32("adminId", admin.AdminId);
                    //----------
                    var token = _token.GenerateJwtToken(data);
                    Response.Cookies.Append("jwt", token);
                    return RedirectToAction(nameof(AdminDashboard), "Admin");
                }
                return View();
            }
            return View(aspNetUser);
        }

        [HttpGet("/admin/logout")]
        public IActionResult AdminLogout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction(nameof(AdminLogin), "Login");
        }

        //-------------------Patient Site
        [HttpGet]
        public IActionResult PatientSite()
        {
            return View();
        }

        //------------------Patient Login
        [HttpGet("login/site/patient")]
        public IActionResult PatientLogin()
        {
            return View();
        }

        [HttpPost("login/site/patient")]
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
        [HttpGet("login/site/patient/forget-password")]
        public IActionResult PatientResetPw()
        {
            return View();
        }

        [HttpGet("login/site/patient/change-password")]
        public IActionResult PatientChangePassword(string token,int id)
        {

            ResetPwVM RP = new()
            {
                AspId = id
            };
            bool x = _token.
                ValidateJwtToken(token, out _);
            if (x)
            {
                return View(RP);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult P_Forgetpass(AspNetUser preq)
        {
            AspNetUser aspdata=new();
            bool isRegistered=true;
            if(preq.Email!="")
            {
                isRegistered = _Pservice.ValidateUserByEmail(preq.Email);
                aspdata = _Aservice.AspUserData(preq.Email!);
            }
            if (isRegistered)
            {
                var receiver = preq.Email ?? "";
                var token = _token.GenerateJwtToken(aspdata);
                var id=aspdata.Id;
                var subject = "Forget Password";
                var message = "Tap on link for change your Password: http://localhost:5093/login/site/patient/change-password?token=" + token+"&id="+id;


                var mail = "tatva.dotnet.dhruvrajsinhvaghela@outlook.com";
                var password = "Vagheladhruv@123";

                var client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));

                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadJwtToken(token);

                // Accessing the email claim
                var emailClaim = tokenS.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                var role = _Aservice.GetUserRoleByEmail(emailClaim?.Value??"");
                if (emailClaim != null && role[0] =="Admin")
                {
                    return RedirectToAction(nameof(AdminLogin));
                }
                else
                {
                    return RedirectToAction(nameof(PatientLogin)); // No email claim found in the token
                }
                
            }

            return NotFound();

        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPwVM RP,int id)
        {
            bool data = _Pservice.UpdateAspUser(RP,id);
            List<string> role=_Aservice.GetUserRoleById(id);
            foreach(var item in role)
            {
                if (data == true && item=="Patient")
                {
                    return RedirectToAction(nameof(PatientLogin));
                }
                else if (data == true && item == "Admin")
                {
                    return RedirectToAction(nameof(AdminLogin));
                }
            }
            return NotFound();
        }



        [HttpGet]
        public IActionResult PatientCreateAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(ResetPwVM RP)
        {
            bool data = _Pservice.CreateUser(RP);
            if( data==true)
            {
                return RedirectToAction(nameof(PatientLogin));
            }
            return NotFound();
        }

        //------------------Patient Submit Request
        [HttpGet("login/site/submit-request")]
        public IActionResult PatientSubmitRequest()
        {
            return View();
        }

        //-----------------   Patient Info
        [HttpGet("login/site/submit-request/patient-form")]
        public IActionResult PatientInfoForm()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ValidateEmail(string email)
        {
            var ans = await _Pservice.validate_Email(email);
            if (ans == false)
            {
                return Json(new { exist = false });
            }
            return Json(new { exist = true });
        }

        [HttpPost("login/site/submit-request/patient-form")]
        [ValidateAntiForgeryToken]
        public IActionResult PatientInfoForm(PatientInfoVM model)
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
        [HttpGet("login/site/submit-request/family-friend-form")]
        public IActionResult PatientFamilyFriendForm()
        {
            return View();
        }

        [HttpPost("login/site/submit-request/family-friend-form")]
        [ValidateAntiForgeryToken]
        public IActionResult PatientFamilyFriendForm(PatientFamilyFriendInfoVM model)
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
        [HttpGet("login/site/submit-request/concierge-form")]
        public IActionResult PatientConciergeInfo()
        {
            return View();
        }

        [HttpPost("login/site/submit-request/concierge-form")]
        [ValidateAntiForgeryToken]
        public IActionResult PatientConciergeInfo(PatientConciergeInfoVM model)
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
        [HttpGet("login/site/submit-request/business-form")]
        public IActionResult PatientBusinessInfo()
        {
            return View();
        }

        [HttpPost("login/site/submit-request/business-form")]
        [ValidateAntiForgeryToken]
        public IActionResult PatientBusinessInfo(PatientBusinessInfoVM model)
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

        [HttpGet]
        public IActionResult PatientLogout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction(nameof(PatientLogin), "Login");
        }

        [HttpGet]
        public IActionResult EncounterForm()
        {
            return View();
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
