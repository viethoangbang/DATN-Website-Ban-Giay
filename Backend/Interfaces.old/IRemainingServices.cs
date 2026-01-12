using Business.DTOs;

namespace Business.Interfaces;

public interface IProductDetailService
{
    Task<IEnumerable<ProductDetailResponseDto>> GetAllAsync();
    Task<ProductDetailResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductDetailResponseDto>> GetByProductIdAsync(int productId);
    Task<ProductDetailResponseDto> CreateAsync(ProductDetailCreateDto dto);
    Task<bool> UpdateAsync(int id, ProductDetailUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IImageService
{
    Task<IEnumerable<ImageResponseDto>> GetAllAsync();
    Task<ImageResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<ImageResponseDto>> GetByTypeAsync(string type);
    Task<IEnumerable<string>> GetDistinctTypesAsync();
    Task<ImageResponseDto> CreateAsync(ImageCreateDto dto);
    Task<bool> UpdateAsync(int id, ImageUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IAddressService
{
    Task<IEnumerable<AddressResponseDto>> GetAllAsync();
    Task<AddressResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<AddressResponseDto>> GetByAccountIdAsync(int accountId);
    Task<AddressResponseDto> CreateAsync(AddressCreateDto dto);
    Task<bool> UpdateAsync(int id, AddressUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface ICartService
{
    Task<CartResponseDto?> GetMyCartAsync(int accountId);
    Task<CartResponseDto> AddToCartAsync(int accountId, AddToCartDto dto);
    Task<bool> UpdateCartItemAsync(int cartDetailId, UpdateCartItemDto dto);
    Task<bool> RemoveFromCartAsync(int cartDetailId);
    Task<bool> ClearCartAsync(int accountId);
}

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(int customerId);
    Task<OrderResponseDto> CreateAsync(int customerId, CreateOrderDto dto);
    Task<bool> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
    Task<bool> CancelOrderAsync(int orderId, int customerId);
    Task<bool> UpdatePaymentStatusAsync(string orderCode, string paymentStatus, string? transactionId = null);
    Task<bool> DeleteAsync(int id);
}

public interface IVoucherService
{
    Task<IEnumerable<VoucherResponseDto>> GetAllAsync();
    Task<VoucherResponseDto?> GetByIdAsync(int id);
    Task<VoucherResponseDto?> GetByCodeAsync(string code);
    Task<VoucherResponseDto> CreateAsync(VoucherCreateDto dto);
    Task<bool> UpdateAsync(int id, VoucherUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
