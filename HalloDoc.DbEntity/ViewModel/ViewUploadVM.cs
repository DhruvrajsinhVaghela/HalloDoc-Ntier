using HalloDoc.DbEntity.Models;

namespace HalloDoc.DbEntity.ViewModel
{
    public class ViewUploadVM
    {
        public int ReqId { get; set; }
        public string? PatientName { get; set; }
        public string? ConfNumber { get; set; }
        public List<RequestWiseFile>? File { get; set; }
        public string? Email { get; set; }
        public DateTime? Date { get; set; }
        public ViewDocumentVM? View { get; set; }
    }
}
