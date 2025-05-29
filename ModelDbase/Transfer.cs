using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class Transfer
{
    public int TransferId { get; set; }

    public int ItemId { get; set; }

    public string? FromLocationId { get; set; }

    public string? ToLocationId { get; set; }

    public DateTime TransferDate { get; set; }

    public string Quantity { get; set; } = null!;

    public virtual Location? FromLocation { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Location? ToLocation { get; set; }
}
