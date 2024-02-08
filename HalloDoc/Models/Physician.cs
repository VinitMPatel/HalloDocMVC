﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Physician
{
    public int Physicianid { get; set; }

    public string? Aspnetuserid { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Medicallicense { get; set; }

    public string? Photo { get; set; }

    public string? Adminnotes { get; set; }

    public BitArray? Isagreementdoc { get; set; }

    public BitArray? Isbackgrounddoc { get; set; }

    public BitArray? Istrainingdoc { get; set; }

    public BitArray? Isnondisclosuredoc { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? Regionid { get; set; }

    public string? Zip { get; set; }

    public string? Altphone { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public short? Status { get; set; }

    public string Businessname { get; set; } = null!;

    public string Businesswebsite { get; set; } = null!;

    public BitArray? Isdeleted { get; set; }

    public int? Roleid { get; set; }

    public string? Npinumber { get; set; }

    public BitArray? Islicensedoc { get; set; }

    public string? Signature { get; set; }

    public BitArray? Iscredentialdoc { get; set; }

    public BitArray? Istokengenerate { get; set; }

    public string? Syncemailaddress { get; set; }
}