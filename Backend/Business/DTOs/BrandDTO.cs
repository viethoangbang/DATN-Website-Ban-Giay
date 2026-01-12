namespace Business.DTOs;

public class BrandResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Status { get; set; }
    public int? DisplayOrder { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public class BrandCreateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Status { get; set; }
    public int? DisplayOrder { get; set; }
    public string? CreateBy { get; set; }
}

public class BrandUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Status { get; set; }
    public int? DisplayOrder { get; set; }
    public string? UpdateBy { get; set; }
}
