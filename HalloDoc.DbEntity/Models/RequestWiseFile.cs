using System;
using System.Collections.Generic;
using System.Net;

namespace HalloDoc.DbEntity.Models;

public partial class RequestWiseFile
{
    public int RequestWiseFileId { get; set; }

    public int RequestId { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public int? PhysicianId { get; set; }

    public int? AdminId { get; set; }

    public short? DocType { get; set; }

    public bool? IsFrontSide { get; set; }

    public bool? IsCompensation { get; set; }

    public IPAddress? Ip { get; set; }

    public bool? IsFinalize { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsPatientRecords { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Physician? Physician { get; set; }
}
