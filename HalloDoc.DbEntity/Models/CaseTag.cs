﻿using System;
using System.Collections.Generic;

namespace HalloDoc.DbEntity.Models;

public partial class CaseTag
{
    public int CaseTagId { get; set; }

    public string Name { get; set; } = null!;
}