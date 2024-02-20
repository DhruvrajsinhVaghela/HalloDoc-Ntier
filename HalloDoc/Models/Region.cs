using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Region
{
    public int RegionId { get; set; }

    public string? Name { get; set; }

    public string? Abbreviation { get; set; }

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();

    public virtual ICollection<Concierge> Concierges { get; set; } = new List<Concierge>();

    public virtual ICollection<RequestClient> RequestClients { get; set; } = new List<RequestClient>();
}
