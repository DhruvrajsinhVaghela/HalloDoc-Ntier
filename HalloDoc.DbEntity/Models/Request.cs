using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.DbEntity.Models;

public partial class Request
{
    public int RequestId { get; set; }

    public int RequestType { get; set; }

    public int? UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public short Status { get; set; }

    public int? PhysicianId { get; set; }

    public string? ConfirmationNumber { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? DeclinedBy { get; set; }

    public bool IsUrgentEmailSent { get; set; }

    public DateTime? LasWellnessDate { get; set; }

    public BitArray? IsMobile { get; set; }

    public short? CallType { get; set; }

    public bool? CompletedByPhysician { get; set; }

    public DateTime? LastReservationDate { get; set; }

    public DateTime? AcceptedDate { get; set; }

    public string? RelationName { get; set; }

    public string? CaseNumber { get; set; }

    public string? Ip { get; set; }

    public string? CaseTag { get; set; }

    public string? CaseTagPhysician { get; set; }

    public int? CreatedUserId { get; set; }

    public int PatientAccountId { get; set; }

    public virtual ICollection<Blockrequest> Blockrequests { get; set; } = new List<Blockrequest>();

    public virtual User? CreatedUser { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Physician? Physician { get; set; }

    public virtual ICollection<RequestBusiness> RequestBusinesses { get; set; } = new List<RequestBusiness>();

    public virtual ICollection<RequestClient> RequestClients { get; set; } = new List<RequestClient>();

    public virtual ICollection<RequestConcierge> RequestConcierges { get; set; } = new List<RequestConcierge>();

    public virtual ICollection<RequestNote> RequestNotes { get; set; } = new List<RequestNote>();

    public virtual ICollection<RequestStatusLog> RequestStatusLogs { get; set; } = new List<RequestStatusLog>();

    public virtual User? User { get; set; }
}
