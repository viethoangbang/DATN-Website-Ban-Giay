namespace Business.DTOs;

public class SizeResponseDto
{
    public int Id { get; set; }
    public string? SizeName { get; set; }
    public int? Quantity { get; set; }
}

public class SizeCreateDto
{
    public string? SizeName { get; set; }
    public int? Quantity { get; set; }
}

public class SizeUpdateDto
{
    public string? SizeName { get; set; }
    public int? Quantity { get; set; }
}
