using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.DbEntity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.services.Interface
{
    public interface IPatientService
    {
        
        public Task<int> PatientLogin(GetAuthenticationVM test);

        //public string? PatientDashboard(int id);


        public Task<bool> validate_Email(string email);

        public string PatientInfoForm(PatientInfo model);
    }
}
