﻿using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Menu
{
    public int Menuid { get; set; }

    public string Name { get; set; } = null!;

    public short Accounttype { get; set; }

    public int? Sortorder { get; set; }
}
