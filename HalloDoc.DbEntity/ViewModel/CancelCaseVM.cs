using HalloDoc.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class CancelCaseVM
    {
        public int ReqID { get; set; }
        public string? AdminNotes {  get; set; }
        public string? PatientName { get; set; }
        public int status { get; set; }
        public string? CaseID { get; set; }
        public List<CaseTag>? ReasonName {  get; set; }
        public AdminDashboardVM? AdminDashboardVM { get; set; }
    }
}
