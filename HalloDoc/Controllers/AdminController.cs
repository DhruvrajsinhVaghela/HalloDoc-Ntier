using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HalloDoc.DbEntity.Data;
using HalloDoc.DbEntity.Models;
using HalloDoc.services.Interface;
using HalloDoc.DbEntity.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        public async Task<IActionResult> AdminDashboard(StatusWiseCount swc)
        {
            var data = _service.PatientStatus(swc);
            return View(data);
           
        }

        public IActionResult CardsView(int status)
        {

            List<AdminDashboardVM> data = _service.GetNewStateData(status);

            switch (status)
            {
                case 1: return PartialView("_AdminNewStateData", data);/*,data*/
                case 2: return PartialView("_AdminPendingstateData");
                case 3: return PartialView("_Active");
                case 4: return PartialView("_Conclude");
                case 5: return PartialView("_ToCLose");
                case 6: return PartialView("_UnPaid");
                default: return PartialView("_NewState");
            }
        }


    }
}
