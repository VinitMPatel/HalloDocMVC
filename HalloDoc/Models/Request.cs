using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Request
{
    public int Requestid { get; set; }

    public int Requesttypeid { get; set; }

    public int? Userid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public short Status { get; set; }

    public int? Physicianid { get; set; }

    public string? Confirmationnumber { get; set; }

    public DateTime Createddate { get; set; }

    public BitArray? Isdeleted { get; set; }

    public DateTime? Modifieddate { get; set; }

    public string? Declinedby { get; set; }

    public BitArray? Isurgentemailsent { get; set; }

    public DateTime? Lastwellnessdate { get; set; }

    public BitArray? Ismobile { get; set; }

    public short? Calltype { get; set; }

    public BitArray? Completedbyphysician { get; set; }

    public DateTime? Lastreservationdate { get; set; }

    public DateTime? Accepteddate { get; set; }

    public string? Relationname { get; set; }

    public string? Casenumber { get; set; }

    public string? Ip { get; set; }

    public string? Casetag { get; set; }

    public string? Casetagphysician { get; set; }

    public string? Patientaccountid { get; set; }

    public int? Createduserid { get; set; }
}
