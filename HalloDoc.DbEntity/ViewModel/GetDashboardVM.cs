using HalloDoc.DbEntity.Models;
using HalloDoc.DbEntity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class GetDashboardVM
    {
        public List<dynamic> details { get; set; }

        public User? user_details { get; set; }


        public DateOnly? date {  get; set; }
    }
}
