using System;
using System.Collections.Generic;

namespace MinimalAPI.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string Name { get; set; } = null!;
}
