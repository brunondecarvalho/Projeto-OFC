using System;
using System.Collections.Generic;

namespace EsfihariaAPI.Models;

public partial class Order
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdOrderCategory { get; set; }

    public int? IdAddress { get; set; }

    public int IdStatus { get; set; }

    public decimal SubtotalValue { get; set; }

    public decimal DeliveryValue { get; set; }

    public decimal DiscountValue { get; set; }

    public decimal TotalValue { get; set; }

    public DateTime Date { get; set; }

    public decimal DeliveryTime { get; set; }

    public string? Note { get; set; }

    public virtual Addresses? IdAddressNavigation { get; set; }

    public virtual Ordercategory IdOrderCategoryNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual ICollection<Transactions> Transactions { get; set; } = new List<Transactions>();
}
