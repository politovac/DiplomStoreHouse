using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class QualityControl
{
    public int QualityControlId { get; set; }

    public int ItemId { get; set; }

    public DateTime ControlDate { get; set; }

    public string Result { get; set; } = null!;

    public int? ResponsibleEmployee { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Employee? ResponsibleEmployeeNavigation { get; set; }
}
