using HalloDoc.DbEntity.Models;

namespace HalloDoc.DbEntity.ViewModels
{
    public class PatientDashboardVM
    {
        public int requestID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string State { get; set; }

        public DateTime? CreatedDate { get; set; }

        public short Status { get; set; }


        public int count_file { get; set; }

        public string FileName { get; set; }

        public User? user { get; set; }

        public string? PhysicianName { get; set; }

        public DateOnly? Date { get; set; }

        public ViewDocumentVM? View { get; set; }
    }
}