using HalloDoc.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class AdminProfileVM
    {
        public int? AdminId { get; set; }
        public string? UserName { get; set; }
        public string? Roll { get; set; }
        public short? Status { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public  string? AltPhone { get; set; }
        public string? Address1 {  get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? ZipCode{ get; set; }
        public List<Region>? RegionList { get; set; }
        public List<Region>? AdminRegList { get; set; }
        public List<int>? SelectedRegions { get; set; }

    }
}
