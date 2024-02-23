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

        public StatusWiseCount PatientStatus(StatusWiseCount swc)
        {
            var x = _repo.GetAdminDashboardData();

            var count = x.GroupBy(x => x.Status).Select(c => new
            {
                s=c.Key,
                num=c.Count(),
            }).ToList();

            var CountNew = count.FirstOrDefault(x => x.s == 1)?.num?? 0;
            var CountPen = count.FirstOrDefault(x => x.s == 2)?.num ?? 0;
            var CountAct = count.FirstOrDefault(x => x.s == 4)?.num ?? 0 + count.FirstOrDefault(x => x.s == 5)?.num ?? 0;
            var CountCon = count.FirstOrDefault(x => x.s == 3)?.num ?? 0 + count.FirstOrDefault(x => x.s == 7)?.num ?? 0 + count.FirstOrDefault(x => x.s == 8)?.num ?? 0;
            var CountClo = count.FirstOrDefault(x => x.s == 9)?.num ?? 0;
            var CountUnp = count.FirstOrDefault(x => x.s == 10)?.num ?? 0;
           /* foreach(int data in count.f)
            {

            }*/

            swc.CountNewState = CountNew;
            swc.CountPendingState = CountPen;
            swc.CountActiveState = CountAct;
            swc.CountConcludeState = CountCon;
            swc.CountCloseState = CountClo;
            swc.CountUnpaidState = CountUnp;

            return swc;
        }
        public List<AdminDashboardVM> GetNewStateData(int status)
        {
            var x = _repo.GetAdminDashboardData();
            
            List<AdminDashboardVM> adminDashboardVMs = new List<AdminDashboardVM>();
            x.ForEach(a =>
            {
                
                if (a.Status == status)
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
                        Notes=a.RequestClients.FirstOrDefault()?.Notes ?? "",
                        RequestType=a.RequestType
                    });

                }
                
            });
            return adminDashboardVMs;
        }
            
        



    }
}
