using HalloDoc.DbEntity.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class ProviderCreateAccountVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? License { get; set; }
        public string? NpiNum { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string?  State { get; set; }
        public string? Zip { get; set; }
        public string MailNumber { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessSite { get; set; }
        public string? AdminNotes { get; set; }
        public List<Region> RegionList {  get; set; }
        public List<int> SelectedRegions { get; set; }
        public List<Role> PhysicianRoleList { get; set; }
        public int SelectedRole { get; set; }
        public IFormFile? Photo {  get; set; }
        public IFormFile? ContractorAgreement { get; set; }
        public IFormFile? BackgroundCheck { get; set; }
        public IFormFile? HipaaCompliance { get; set; }
        public IFormFile? NonDisclosureAgreement { get; set; }
        
    }
}
