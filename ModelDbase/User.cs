using System;
using System.Collections.Generic;

namespace DiplomStoreHouse.ModelDbase;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
}
