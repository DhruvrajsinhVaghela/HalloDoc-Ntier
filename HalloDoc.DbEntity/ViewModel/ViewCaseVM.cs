using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class ViewCaseVM
    {
        public int ReqId { get; set; }
        public int? Status { get; set; }
        public string? Notes { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Region { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Room { get; set; }
        public  string? ConfNO { get; set; }
        public int RequestType { get; set; }
    }
}
