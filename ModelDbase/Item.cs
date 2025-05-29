using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public string PackageType { get; set; } = null!;

    public string UnitWeight { get; set; } = null!;

    public string MeasurementUnit { get; set; } = null!;

    public string? Image { get; set; }

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();

    public virtual ICollection<QualityControl> QualityControls { get; set; } = new List<QualityControl>();

    public virtual Reception? Reception { get; set; }

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    public virtual ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
}
