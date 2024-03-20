﻿using System;
using System.Collections.Generic;

namespace HalloDoc.DbEntity.Models;

public partial class AdminRegion
{
    public int AdminRegionId { get; set; }

    public int? AdminId { get; set; }

    public int RegionId { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Region? Region { get; set; }
}