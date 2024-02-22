using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.services.Interface
{
    public interface IAdminService
    {
        public List<AdminDashboardVM> GetUserData();
    }
}
