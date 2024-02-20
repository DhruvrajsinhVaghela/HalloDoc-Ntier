using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? AspNetUserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public BitArray? IsMobile { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int? RegionId { get; set; }

    public string? ZipCode { get; set; }

    public string? StrMonth { get; set; }

    public int? IntYear { get; set; }

    public int? IntDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? Status { get; set; }

    public string? Ip { get; set; }

    public BitArray? IsRequestWithEmail { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual AspNetUser? AspNetUser { get; set; }

    public virtual ICollection<Request> RequestCreatedUsers { get; set; } = new List<Request>();

    public virtual ICollection<Request> RequestUsers { get; set; } = new List<Request>();
}
