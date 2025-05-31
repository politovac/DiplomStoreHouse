using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public int ItemId { get; set; }

    public string ClientName { get; set; } = null!;

    public string DeliveryPoint { get; set; } = null!;

    public DateTime ExpectedDate { get; set; }

    public string State { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
