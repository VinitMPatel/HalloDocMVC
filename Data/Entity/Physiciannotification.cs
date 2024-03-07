using System;
using System.Collections;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Physiciannotification
{
    public int Id { get; set; }

    public int Pysicianid { get; set; }

    public BitArray Isnotificationstopped { get; set; } = null!;

    public virtual Physician Pysician { get; set; } = null!;
}
