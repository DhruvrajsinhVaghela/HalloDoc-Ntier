using HalloDoc.DbEntity.Models;

namespace HalloDoc.DbEntity.ViewModel
{
    public class SendOrderVM
    {
        public int ReqId {  get; set; }
        public List<HealthProfessionalType>? ProfessionTypes { get; set; }
        public List<HealthProfessional>? VendorNames { get; set; }
        public string? Email { get; set; }
        public string? Fax { get; set; }
        public string? Phone { get; set; }
        public string? Prescription { get; set; }
        public int ReFill { get; set; } 
    }
}