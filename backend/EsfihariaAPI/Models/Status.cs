using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
