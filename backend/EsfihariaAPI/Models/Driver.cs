using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Driver
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdStatus { get; set; }

    public string Cnh { get; set; } = null!;

    public string LicensePlate { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
