using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.DbEntity.ViewModels;
using HalloDoc.Repositories.Interfaces;
using HalloDoc.services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace HalloDoc.services.Implementation
{
    public class PatientService:IPatientService
    {
        private readonly IPatient _repo;

        public PatientService(IPatient repo)
        {
            _repo = repo;
        }
        public Task<int> PatientLogin(GetAuthenticationVM test)
        {
            //AspNetUser aspuser = _logger.AspNetUsers.FirstOrDefault(u => u.Email == aspNetUser.Email);
            GetAuthenticationVM aspuser =_repo.AuthenticateUser(test);

            if (aspuser != null && Crypto.VerifyHashedPassword(aspuser.Passwordhashed, test.PasswordHash))//in crypto. method (hash password,plain text)
            {
                return Task.FromResult(test.Id);
            }
            return Task.FromResult(0);
        }

        //public string? PatientDashboard(int id)
        //{

        //}

        public Task<bool> validate_Email(GetAuthenticationVM test)
        {
            GetAuthenticationVM aspuser=_repo.ValidateEmail(test);
            if(aspuser==null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public AspNetUser PatientInfoForm(GetAllFormsVM forms)
        {
            GetAllFormsVM userForm = _repo.GetAllForms(forms);
            if (userForm.aspnetuser==null) 
            {
                AspNetUser aspnetuser1 = new AspNetUser
                {

                    UserName = forms.user.FirstName + "_" + forms.user.LastName,
                    Email = forms.Email,
                    PasswordHash = forms.user.FirstName,
                    PhoneNumber = forms.user.FirstName,
                    CreatedDate = DateTime.Now //here,modified Date,modified By coulmns-remaining to add    
                };
                userForm.aspnetuser = aspnetuser1;
                if (forms.aspnetuser.PasswordHash != null)
                {
                    forms.aspnetuser.PasswordHash = Crypto.HashPassword(forms.aspnetuser.PasswordHash);
                }
                _repo.ADDAspNetUser(aspnetuser1);
                _repo.SaveChanges();

            }
            Region region = new Region
            {
                Name = forms.RegName,
                Abbreviation = forms.RegName.Substring(0, 3)
            };
            _repo.ADDRegion(region);
            _repo.SaveChanges();

            if (userForm.user == null)
            {
                User user = new User
                {
                    AspNetUserId = forms.aspnetuser.Id,
                    FirstName = forms.user.FirstName,
                    LastName = forms.user.LastName,
                    Email = forms.user.Email,
                    Mobile = forms.user.Mobile,
                    ZipCode = forms.user.ZipCode,
                    State = forms.user.State,
                    City = forms.user.City,
                    Street = forms.user.Street,
                    IntDate = forms.user.IntDate,
                    IntYear = forms.user.IntYear,
                    StrMonth = forms.user.StrMonth,
                    CreatedDate = DateTime.Now,
                    RegionId = region.RegionId,
                    CreatedBy = forms.user.FirstName,
                    AspNetUser = forms.aspnetuser //modified_by,modified_date,status,ip,is_request_with_email,is_deleted remaining to add
                };
                forms.user = user;
                _repo.ADDUser(user);
                _repo.SaveChanges();
            }

    }
}
