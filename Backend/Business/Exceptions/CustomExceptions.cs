namespace Business.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string entity, int id) 
        : base($"{entity} with ID {id} not found") { }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }
    
    public ValidationException(Dictionary<string, string[]> errors) 
        : base("Validation failed")
    {
        Errors = errors;
    }
}

public class InsufficientStockException : Exception
{
    public int ProductDetailId { get; }
    public int RequestedQuantity { get; }
    public int AvailableQuantity { get; }
    
    public InsufficientStockException(int productDetailId, int requested, int available)
        : base($"Insufficient stock. Requested: {requested}, Available: {available}")
    {
        ProductDetailId = productDetailId;
        RequestedQuantity = requested;
        AvailableQuantity = available;
    }
}

public class DuplicateVariantException : Exception
{
    public int ProductId { get; }
    public int ColorId { get; }
    public int SizeId { get; }
    
    public DuplicateVariantException(int productId, int colorId, int sizeId)
        : base($"Variant with ProductId {productId}, ColorId {colorId}, and SizeId {sizeId} already exists")
    {
        ProductId = productId;
        ColorId = colorId;
        SizeId = sizeId;
    }
}
