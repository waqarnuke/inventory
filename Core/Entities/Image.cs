using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Image
{
    [Key]
    public int Id { get; set; }
    public string? PublicId { get; set; }
    public string? Url {get; set;}
    public bool? IsMain { get; set; }
    public string? UserId { get; set; }
    [ForeignKey("Item")]
    public int? ItemId { get; set; }
    public Item? Item { get; set; }
}
