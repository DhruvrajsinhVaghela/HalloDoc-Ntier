using HalloDoc.DbEntity.Models;

namespace HalloDoc.DbEntity.ViewModel 
{ 
    public class PatientDashboardVM
    {
        public int requestID { get; set; }

        public string? FirstName { get; set; }

        

        public DateTime CreatedDate { get; set; }

        public short Status { get; set; }


        public int count_file { get; set; }

        public string? FileName { get; set; }

        public User? user { get; set; }

        public string? PhysicianName { get; set; }

        public DateOnly? Date { get; set; }

        public ViewDocumentVM? View { get; set; }

     
    }
}