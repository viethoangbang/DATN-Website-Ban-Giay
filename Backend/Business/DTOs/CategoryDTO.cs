namespace Business.DTOs;

public class CategoryResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? CreateBy { get; set; }
    public string? UpdateBy { get; set; }
    public string? Condition { get; set; }
}

public class CategoryCreateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public string? Condition { get; set; }
}

public class CategoryUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? UpdateBy { get; set; }
    public string? Condition { get; set; }
}
