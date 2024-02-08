using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Shiftdetailregion
{
    public int Shiftdetailregionid { get; set; }

    public int Shiftdetailid { get; set; }

    public int Regionid { get; set; }

    public BitArray? Isdeleted { get; set; }
}
