﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Physiciannotification
{
    public int Id { get; set; }

    public int Physicianid { get; set; }

    public BitArray? Isnotificationstopped { get; set; }
}
