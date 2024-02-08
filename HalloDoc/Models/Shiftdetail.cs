﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Shiftdetail
{
    public int Shiftdetailid { get; set; }

    public int Shiftid { get; set; }

    public DateTime Shiftdate { get; set; }

    public int? Regionid { get; set; }

    public TimeOnly Starttime { get; set; }

    public TimeOnly Endtime { get; set; }

    public short Status { get; set; }

    public BitArray Isdeleted { get; set; } = null!;

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public DateTime? Lastrunningdate { get; set; }

    public string? Eventid { get; set; }

    public BitArray? Issync { get; set; }
}