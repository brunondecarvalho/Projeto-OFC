using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public int IdRole { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Addresses> Addresses { get; set; } = new List<Addresses>();

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
