using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using HalloDoc.Repositories.Interfaces;
using HalloDoc.services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.services.Implementation
{
    public class AdminService:IAdminService
    {
        private readonly IAAdmin _repo;

        public AdminService(IAAdmin repo)
        {
            _repo = repo;
        }

        public List<AdminDashboardVM> GetUserData()
        {
            var x = _repo.GetDetails();
            //DateOnly dateofbirth = new DateOnly(x., DateOnly.ParseExact(userobj.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, userobj.IntDate.Value);
            //var count = 0;
            List<AdminDashboardVM> adminDashboardVMs = new List<AdminDashboardVM>();
            x.ForEach(a =>
            {
                
                if (a.Status == 1)
                {
                    //count++;
                    adminDashboardVMs.Add(new AdminDashboardVM
                    {

                        PatientName = a.RequestClients.FirstOrDefault()?.FirstName ?? "",
                        BirthMonth = a.RequestClients.FirstOrDefault()?.StrMonth ?? "",
                        BirthYear = a.RequestClients.FirstOrDefault()?.IntYear ?? 9999,
                        BirthDay = a.RequestClients.FirstOrDefault()?.IntDate ?? 31,
                        RequestorName = a.FirstName,
                        RequestDate = a.CreatedDate,
                        Phonenumber = a.RequestClients.FirstOrDefault()?.PhoneNumber ?? "",
                        Status = a.Status,
                        ProviderName = a.Physician?.FirstName ?? "",
                        BirthDate = a.RequestClients.FirstOrDefault()?.IntDate+" "+ a.RequestClients.FirstOrDefault()?.StrMonth+" "+ a.RequestClients.FirstOrDefault()?.IntYear,
                        Address = a.RequestClients.FirstOrDefault()?.Street + " " + a.RequestClients.FirstOrDefault()?.City + " " + a.RequestClients.FirstOrDefault()?.State + " " + a.RequestClients.FirstOrDefault()?.ZipCode,
                        //Patientcount = count,
                        RequestType=a.RequestType
                    });

                }
                else
                {
                    return ;
                }

                
            });


            return adminDashboardVMs;
        }
    }
}
