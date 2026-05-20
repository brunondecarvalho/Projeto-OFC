using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Ordercategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
