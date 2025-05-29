using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class Operation
{
    public int OperationId { get; set; }

    public string TypeOperation { get; set; } = null!;

    public string? StartLocationId { get; set; }

    public string? FinishLocationId { get; set; }

    public int ItemId { get; set; }

    public string Quantity { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Location? FinishLocation { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Location? StartLocation { get; set; }

    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}
