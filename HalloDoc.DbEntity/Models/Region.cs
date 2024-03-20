using System;
using System.Collections.Generic;

namespace HalloDoc.DbEntity.Models;

public partial class Region
{
    public int RegionId { get; set; }

    public string? Name { get; set; }

    public string? Abbreviation { get; set; }

    public virtual ICollection<AdminRegion> AdminRegions { get; set; } = new List<AdminRegion>();

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();

    public virtual ICollection<Concierge> Concierges { get; set; } = new List<Concierge>();

    public virtual ICollection<HealthProfessional> HealthProfessionals { get; set; } = new List<HealthProfessional>();

    public virtual ICollection<RequestClient> RequestClients { get; set; } = new List<RequestClient>();
}
