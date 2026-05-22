using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Transactions
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdOrder { get; set; }

    public int IdStatus { get; set; }

    public string GatewayTransactionId { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;

    public int Installments { get; set; }

    public decimal TotalValue { get; set; }

    public string PayloadResponse { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
