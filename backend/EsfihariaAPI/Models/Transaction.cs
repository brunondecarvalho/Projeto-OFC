using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdOrder { get; set; }

    public int IdPaymentMethod { get; set; }

    public int IdStatus { get; set; }

    public decimal TotalValue { get; set; }

    public int IdGatewayTransaction { get; set; }

    public int Payload { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? PayloadResponse { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
