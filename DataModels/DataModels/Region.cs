using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Region
{
    public int Regionid { get; set; }

    public string Name { get; set; } = null!;

    public string? Abbreviation { get; set; }
}
