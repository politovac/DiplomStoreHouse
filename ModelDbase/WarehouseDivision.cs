using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class WarehouseDivision
{
    public int DivisionId { get; set; }

    public string Name { get; set; } = null!;

    public string ResponsibilityOperation { get; set; } = null!;

    public string Manager { get; set; } = null!;

    public string? Image { get; set; }
}
