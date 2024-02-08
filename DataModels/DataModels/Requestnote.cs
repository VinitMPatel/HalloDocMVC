﻿using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Requestnote
{
    public int Requestnotesid { get; set; }

    public int Requestid { get; set; }

    public string? Strmonth { get; set; }

    public int? Integeryear { get; set; }

    public int? Integerdate { get; set; }

    public string? Physiciannotes { get; set; }

    public string? Adminnotes { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public string? Ip { get; set; }

    public string? Administrativenotes { get; set; }
}
