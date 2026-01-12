namespace Business.DTOs;

public class ColorResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public class ColorCreateDto
{
    public string? Name { get; set; }
    public string? Status { get; set; }
}

public class ColorUpdateDto
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? UpdateBy { get; set; }
}
