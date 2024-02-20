using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Physician
{
    public int PhysicianId { get; set; }

    public int? AspNetUserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? MedicalLicense { get; set; }

    public string? Photo { get; set; }

    public string? AdminNotes { get; set; }

    public bool? IsAgreementDoc { get; set; }

    public bool? IsBackgroundDoc { get; set; }

    public bool? IsTrainingDoc { get; set; }

    public bool? IsNonDisclosureDoc { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? RegionId { get; set; }

    public string? ZipCode { get; set; }

    public string? AltPhone { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public short? Status { get; set; }

    public string? BusinessName { get; set; }

    public string? BusinessWebsite { get; set; }

    public bool? IsDeleted { get; set; }

    public int? RoleId { get; set; }

    public string? NpiNumber { get; set; }

    public bool? IsLicenseDoc { get; set; }

    public string? Signature { get; set; }

    public bool? IsCredentialDoc { get; set; }

    public bool? IsTokenGenerate { get; set; }

    public string? SyncEmailAddress { get; set; }

    public virtual AspNetUser? AspNetUser { get; set; }

    public virtual AspNetUser? CreatedByNavigation { get; set; }

    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    public virtual ICollection<RequestWiseFile> RequestWiseFiles { get; set; } = new List<RequestWiseFile>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
