using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Productcategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
