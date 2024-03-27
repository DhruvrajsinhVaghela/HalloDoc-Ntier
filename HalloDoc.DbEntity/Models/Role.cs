using System;
using System.Collections.Generic;
using System.Net;

namespace HalloDoc.DbEntity.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public short AccountType { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public IPAddress? Ip { get; set; }

    public virtual AspNetUser? CreatedByNavigation { get; set; }

    public virtual AspNetUser? ModifiedByNavigation { get; set; }
}
