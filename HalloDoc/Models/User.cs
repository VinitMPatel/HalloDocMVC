﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class User
{
    public int Userid { get; set; }

    public string? Aspnetuserid { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public BitArray? Ismobile { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int? Regionid { get; set; }

    public string? Zipcode { get; set; }

    public string? Strmonth { get; set; }

    public int? Integeryear { get; set; }

    public int? Integerdate { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public short? Status { get; set; }

    public BitArray? Isdeleted { get; set; }

    public string? Ip { get; set; }

    public BitArray? Isrequestwithemail { get; set; }
}
