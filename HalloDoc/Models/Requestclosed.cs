﻿using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Requestclosed
{
    public int Requestclosedid { get; set; }

    public int Requestid { get; set; }

    public int Requeststatuslogid { get; set; }

    public string? Phynotes { get; set; }

    public string? Clientnotes { get; set; }

    public string? Ip { get; set; }
}
