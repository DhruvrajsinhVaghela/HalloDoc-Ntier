using HalloDoc.DbEntity.Models;

namespace HalloDoc.DbEntity.ViewModel
{
    public class CloseCaseVM
    {
        public int ReqId { get; set; }
        public string? ConfNo { get; set; }
        public List<RequestWiseFile>? FileList { get; set; }
        public DateOnly UploadDate { get; set; }
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PhoneNo { get; set; }
    }
}
