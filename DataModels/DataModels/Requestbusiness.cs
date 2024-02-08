using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Requestbusiness
{
    public int Requestbusinessid { get; set; }

    public int Requestid { get; set; }

    public int Businessid { get; set; }

    public string? Ip { get; set; }
}
