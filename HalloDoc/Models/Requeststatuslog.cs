using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Requeststatuslog
{
    public int Requeststatuslogid { get; set; }

    public int Requestid { get; set; }

    public short Status { get; set; }

    public int? Physicianid { get; set; }

    public int? Adminid { get; set; }

    public int? Transtophysicianid { get; set; }

    public string? Notes { get; set; }

    public DateTime Createddate { get; set; }

    public string? Ip { get; set; }

    public BitArray? Transtoadmin { get; set; }
}
