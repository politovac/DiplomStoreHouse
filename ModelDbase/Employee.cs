using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Posititon { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<QualityControl> QualityControls { get; set; } = new List<QualityControl>();

    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}
