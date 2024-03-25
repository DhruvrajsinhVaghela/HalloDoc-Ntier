using HalloDoc.DbEntity.Models;


namespace HalloDoc.DbEntity.ViewModel
{
    public class AdminDashboardVM
    {
        //public List<User> userData { get; set; }
        public string? PatientName { get; set; } //from request client
        public string? PatientLastName { get; set; } //From Request Client
        public int ReqID { get; set; }
        public string? Email { get; set; }//from request client
        public string? BirthMonth { get; set; }//from request client
        public int? BirthYear { get; set; }//from request client
        public int? BirthDay { get; set; }//from request client
        public string? RequestorName { get; set; }//from request
        public string? RequestorLastName { get; set; }
        public DateOnly? RequestDate { get; set; }//from request
        public string? PhoneNumber { get; set; }//from request client
        public string? RequestorPhoneNumber { get; set; }//from request table
        public string? State { get; set; }//from request client
        public string? City { get; set; }//from request client
        public string? Street { get; set; }//from request client
        public string? ZipCode { get; set; }//from request client
        public List<string>? Notes { get; set; }//from request client
        public short? Status { get; set; }//from physician
        public string? ProviderName { get; set; }//from physician
        public DateOnly? BirthDate { get; set; }
        public string? Address { get; set; }
        public int RequestType { get; set; }
        public int? Region { get; set; }
        public int? PhysicianId { get; set; }
        public List<string>? AdminNotes { get; set; }
        public double? ItemCountPagination { get; set; }
        public int CountNewState { get; set; }

        public int CountPendingState { get; set; }

        public int CountActiveState { get; set; }

        public int CountConcludeState { get; set; }

        public int CountCloseState { get; set; }

        public int CountUnpaidState { get; set; }

        public IEnumerable<Region>? RegionList { get; set; }
        //public string AdminNotes { get; set; }

    }
}
