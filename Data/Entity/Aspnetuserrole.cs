using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entity;

[Table("aspnetuserroles")]
public partial class Aspnetuserrole
{
    [Column("userid")]
    [StringLength(128)]
    public string Userid { get; set; } = null!;

    [Key]
    [Column("roleid")]
    public int Roleid { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [ForeignKey("Userid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetuser User { get; set; } = null!;
}
