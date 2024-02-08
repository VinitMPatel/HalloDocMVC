using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Shift
{
    public int Shiftid { get; set; }

    public int Physicianid { get; set; }

    public DateOnly Startdate { get; set; }

    public BitArray Isrepeat { get; set; } = null!;

    public string? Weekdays { get; set; }

    public int? Repeatupto { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Ip { get; set; }
}
