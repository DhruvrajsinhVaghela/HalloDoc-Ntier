using System;
using System.Collections.Generic;
using System.Net;

namespace HalloDoc.DbEntity.Models;

public partial class Business
{
    public int BusinessId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? RegionId { get; set; }

    public string? ZipCode { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FaxNumber { get; set; }

    public bool? IsRegistered { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public short? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public IPAddress? Ip { get; set; }

    public virtual AspNetUser? CreatedByNavigation { get; set; }

    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    public virtual Region? Region { get; set; }

    public virtual ICollection<RequestBusiness> RequestBusinesses { get; set; } = new List<RequestBusiness>();
}
