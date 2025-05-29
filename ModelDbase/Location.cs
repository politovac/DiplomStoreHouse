using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class Location
{
    public string LocationId { get; set; } = null!;

    public string MaxCapacity { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Operation> OperationFinishLocations { get; set; } = new List<Operation>();

    public virtual ICollection<Operation> OperationStartLocations { get; set; } = new List<Operation>();

    public virtual ICollection<Transfer> TransferFromLocations { get; set; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferToLocations { get; set; } = new List<Transfer>();
}
