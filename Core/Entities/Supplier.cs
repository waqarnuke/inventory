using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Supplier
{
    [Key]
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}
