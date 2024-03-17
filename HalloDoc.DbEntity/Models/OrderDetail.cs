using System;
using System.Collections.Generic;

namespace HalloDoc.DbEntity.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? VendorId { get; set; }

    public int? RequestId { get; set; }

    public string? FaxNumber { get; set; }

    public string? Email { get; set; }

    public string? BusinessContact { get; set; }

    public string? Prescription { get; set; }

    public int? NoOfRefill { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public virtual AspNetUser? CreatedByNavigation { get; set; }

    public virtual Request? Request { get; set; }

    public virtual HealthProfessional? Vendor { get; set; }
}
