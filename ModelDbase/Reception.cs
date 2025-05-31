using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class Reception
{
    public int ReceptionId { get; set; }

    public int ItemId { get; set; }

    public string SupplierInvoiceNumber { get; set; } = null!;

    public DateTime ReceiptDate { get; set; }

    public string ReceivedQuantity { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
