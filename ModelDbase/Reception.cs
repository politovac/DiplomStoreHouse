using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class Reception
{
    public int ReceptionId { get; set; }

    public int ItemId { get; set; }

    public string NameItem { get; set; } = null!;

    public string SupplierInvoiceNumber { get; set; } = null!;

    public DateOnly ReceiptDate { get; set; }

    public string ReceivedQuantity { get; set; } = null!;

    public virtual Item ReceptionNavigation { get; set; } = null!;
}
