using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Product
{
    public int Id { get; set; }

    public int IdCategory { get; set; }

    public int IdStatus { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }

    public virtual Productcategory IdCategoryNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();
}
