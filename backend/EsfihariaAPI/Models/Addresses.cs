using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Addresses
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public string Address { get; set; } = null!;

    public int Number { get; set; }

    public string Neighborhood { get; set; } = null!;

    public int Cep { get; set; }

    public string? Complement { get; set; }

    public string? Landmark { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
