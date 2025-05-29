using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class TaskAssignment
{
    public int TaskId { get; set; }

    public int EmployeeId { get; set; }

    public int OperationId { get; set; }

    public int Priority { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Operation Operation { get; set; } = null!;
}
