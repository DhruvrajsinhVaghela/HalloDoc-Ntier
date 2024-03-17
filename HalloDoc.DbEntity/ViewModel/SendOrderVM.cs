using HalloDoc.DbEntity.Models;

namespace HalloDoc.DbEntity.ViewModel
{
    public class SendOrderVM
    {
        public int reqId {  get; set; }
        public List<HealthProfessionalType>? professionTypes { get; set; }
        public List<HealthProfessional>? VendorNames { get; set; }
        public string? email { get; set; }
        public string? fax { get; set; }
        public string? phone { get; set; }
        public string? Prescription { get; set; }
        public int refill { get; set; } 
    }
}