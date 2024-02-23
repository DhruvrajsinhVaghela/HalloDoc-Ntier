using HalloDoc.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace HalloDoc.DbEntity.ViewModel
{
    public class AdminDashboardVM
    {
        //public List<User> userData { get; set; }
        public string PatientName { get; set; } //from request client
        public string? BirthMonth { get; set; }//from request client
        public int? BirthYear { get; set; }//from request client
        public int? BirthDay { get; set; }//from request client
        public string RequestorName { get; set; }//from request
        public DateTime? RequestDate { get; set; }//from request
        public string Phonenumber { get; set; }//from request client
        public string State { get; set; }//from request client
        public string City { get; set; }//from request client
        public string Street { get; set; }//from request client
        public string ZipCode { get; set; }//from request client
        public string Notes { get; set; }//from request client
        public short? Status { get; set; }//from physician
        public string ProviderName { get; set; }//from physician
        public string? BirthDate { get; set; }
        public string? Address { get; set; }
        public int RequestType { get; set; }

       
    }
}
