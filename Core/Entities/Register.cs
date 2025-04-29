using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Register :BaseEntity
{
    public decimal CashBalance { get; set; } = 0;
    public decimal CardBalance { get; set; } = 0;
    public int? LocationId { get; set; }
}
