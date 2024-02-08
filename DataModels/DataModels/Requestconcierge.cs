using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Requestconcierge
{
    public int Id { get; set; }

    public int Requestid { get; set; }

    public int Conciergeid { get; set; }

    public string? Ip { get; set; }
}
