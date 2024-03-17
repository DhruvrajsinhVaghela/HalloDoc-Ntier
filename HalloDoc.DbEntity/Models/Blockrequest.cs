using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.DbEntity.Models;

public partial class Blockrequest
{
    public int BlockrequestId { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public BitArray? IsActive { get; set; }

    public string? Reason { get; set; }

    public string? Ip { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? RequestId { get; set; }

    public virtual Request? Request { get; set; }
}
