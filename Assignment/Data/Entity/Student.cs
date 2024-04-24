using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public int? CourseId { get; set; }

    public int? Age { get; set; }

    public string Email { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Course { get; set; }

    public string? Grade { get; set; }

    public virtual Course? CourseNavigation { get; set; }
}
