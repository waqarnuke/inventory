using System;

namespace API.Dtos;

public class PhotoDto
{
    public int Id { get; set; }
    public string? PublicId { get; set; }
    public string? Url {get; set;}
    public bool? IsMain { get; set; }
    public string? UserId { get; set; }
    public int ItemId { get; set; }
}

public class ImageDto
{
    public int Id { get; set; }
    public string? PublicId { get; set; }
    public string? Url {get; set;}
    public bool? IsMain { get; set; }
    public string? UserId { get; set; }
    public int ItemId { get; set; }
}
