using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

// ProductDetail Service
public class ProductDetailService : IProductDetailService
{
    private readonly IProductDetailRepo _repo;
    private readonly IProductDetailImageRepo _imageRepo;
    private readonly IDiscountRepo _discountRepo;
    
    public ProductDetailService(IProductDetailRepo repo, IProductDetailImageRepo imageRepo, IDiscountRepo discountRepo)
    {
        _repo = repo;
        _imageRepo = imageRepo;
        _discountRepo = discountRepo;
    }

    public async Task<IEnumerable<ProductDetailResponseDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<ProductDetailResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : MapToDto(item);
    }

    public async Task<IEnumerable<ProductDetailResponseDto>> GetByProductIdAsync(int productId)
    {
        var items = await _repo.GetByProductIdAsync(productId);
        return items.Select(MapToDto);
    }

    public async Task<ProductDetailResponseDto> CreateAsync(ProductDetailCreateDto dto)
    {
        // Validate required fields
        if (!dto.ProductId.HasValue || !dto.ColorId.HasValue || !dto.SizeId.HasValue)
            throw new BadRequestException("ProductId, ColorId, and SizeId are required");

        // Check for duplicate variant
        var existing = await _repo.GetByVariantAsync(
            dto.ProductId.Value, 
            dto.ColorId.Value, 
            dto.SizeId.Value
        );
        
        if (existing != null)
            throw new DuplicateVariantException(
                dto.ProductId.Value, 
                dto.ColorId.Value, 
                dto.SizeId.Value
            );

        var entity = new ProductDetail
        {
            Price = dto.Price,
            Quantity = dto.Quantity,
            ImageId = dto.ImageId,
            ColorId = dto.ColorId,
            SizeId = dto.SizeId,
            ProductId = dto.ProductId
        };
        var created = await _repo.AddAsync(entity);

        // Add multiple images if provided (maximum 4 images per variant)
        if (dto.ImageIds != null && dto.ImageIds.Any())
        {
            // Limit to maximum 4 images
            var imageIdsToAdd = dto.ImageIds.Take(4).ToList();
            if (dto.ImageIds.Count > 4)
            {
                throw new BadRequestException("Mỗi biến thể chỉ được tối đa 4 hình ảnh");
            }

            int order = 0;
            foreach (var imageId in imageIdsToAdd)
            {
                var productDetailImage = new ProductDetailImage
                {
                    ProductDetailId = created.Id,
                    ImageId = imageId,
                    DisplayOrder = order++
                };
                await _imageRepo.AddAsync(productDetailImage);
            }
        }

        var result = await _repo.GetByIdAsync(created.Id);
        return MapToDto(result!);
    }

    public async Task<bool> UpdateAsync(int id, ProductDetailUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        // If color or size is being changed, check for duplicate
        // Note: ProductId should not be changed when updating a variant
        if (dto.ColorId.HasValue || dto.SizeId.HasValue)
        {
            var productId = entity.ProductId;
            var colorId = dto.ColorId ?? entity.ColorId;
            var sizeId = dto.SizeId ?? entity.SizeId;

            if (productId.HasValue && colorId.HasValue && sizeId.HasValue)
            {
                var existing = await _repo.GetByVariantAsync(
                    productId.Value, 
                    colorId.Value, 
                    sizeId.Value, 
                    id
                );
                
                if (existing != null)
                    throw new DuplicateVariantException(
                        productId.Value, 
                        colorId.Value, 
                        sizeId.Value
                    );
            }
        }

        entity.Price = dto.Price;
        entity.Quantity = dto.Quantity;
        entity.ImageId = dto.ImageId;
        if (dto.ColorId.HasValue) entity.ColorId = dto.ColorId;
        if (dto.SizeId.HasValue) entity.SizeId = dto.SizeId;

        await _repo.UpdateAsync(entity);

        // Update images if provided (maximum 4 images per variant)
        if (dto.ImageIds != null)
        {
            // Limit to maximum 4 images
            if (dto.ImageIds.Count > 4)
            {
                throw new BadRequestException("Mỗi biến thể chỉ được tối đa 4 hình ảnh");
            }

            // Delete existing images
            await _imageRepo.DeleteByProductDetailIdAsync(id);

            // Add new images
            int order = 0;
            foreach (var imageId in dto.ImageIds)
            {
                var productDetailImage = new ProductDetailImage
                {
                    ProductDetailId = id,
                    ImageId = imageId,
                    DisplayOrder = order++
                };
                await _imageRepo.AddAsync(productDetailImage);
            }
        }

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        await _repo.DeleteAsync(entity);
        return true;
    }

    private ProductDetailResponseDto MapToDto(ProductDetail entity)
    {
        var dto = new ProductDetailResponseDto
        {
            Id = entity.Id,
            Price = entity.Price,
            Quantity = entity.Quantity,
            ImageId = entity.ImageId,
            ColorId = entity.ColorId,
            SizeId = entity.SizeId,
            ProductId = entity.ProductId,
            CreateDate = entity.CreateDate,
            ColorName = entity.Color?.Name,
            SizeName = entity.Size?.SizeName,
            ImageUrl = entity.Image?.Url,
            Images = (entity.ProductDetailImages ?? new List<ProductDetailImage>())
                .OrderBy(pdi => pdi.DisplayOrder ?? 0)
                .Where(pdi => pdi.Image != null) // Filter out null images
                .Select(pdi => new ImageResponseDto
                {
                    Id = pdi.Image!.Id,
                    Url = pdi.Image.Url,
                    Type = pdi.Image.Type,
                    Status = pdi.Image.Status
                })
                .ToList()
        };
        
        // Tính discount nếu có ProductId và Price
        if (entity.ProductId.HasValue && entity.Price.HasValue)
        {
            var discountInfo = CalculateDiscount(entity.ProductId.Value, entity.Price.Value);
            if (discountInfo != null)
            {
                dto.DiscountType = discountInfo.DiscountType;
                dto.DiscountValue = discountInfo.DiscountValue;
                dto.FinalPrice = discountInfo.FinalPrice;
                dto.DiscountActive = true;
            }
            else
            {
                dto.FinalPrice = entity.Price.Value;
                dto.DiscountActive = false;
            }
        }
        else
        {
            dto.FinalPrice = entity.Price;
            dto.DiscountActive = false;
        }
        
        return dto;
    }
    
    // Tính discount: lấy discount active cao nhất
    private DiscountCalculationResult? CalculateDiscount(int productId, decimal originalPrice)
    {
        var now = DateTime.Now;
        
        // Lấy tất cả discount active
        var activeDiscounts = _discountRepo.GetActiveByProductIdAsync(productId).Result
            .Where(d => d.Status == "Active" 
                     && d.StartDate <= now 
                     && d.EndDate >= now)
            .ToList();
        
        if (!activeDiscounts.Any())
            return null;
        
        // Tính discount amount cho mỗi discount và chọn discount cao nhất
        Discount? bestDiscount = null;
        decimal maxDiscountAmount = 0;
        
        foreach (var discount in activeDiscounts)
        {
            decimal discountAmount = 0;
            
            if (discount.DiscountType == "Percentage")
            {
                discountAmount = originalPrice * (discount.DiscountValue / 100);
            }
            else if (discount.DiscountType == "Fixed")
            {
                discountAmount = discount.DiscountValue;
            }
            
            if (discountAmount > maxDiscountAmount)
            {
                maxDiscountAmount = discountAmount;
                bestDiscount = discount;
            }
        }
        
        if (bestDiscount == null)
            return null;
        
        // Tính final price
        decimal finalPrice = originalPrice - maxDiscountAmount;
        if (finalPrice < 0)
            finalPrice = 0;
        
        return new DiscountCalculationResult
        {
            DiscountType = bestDiscount.DiscountType,
            DiscountValue = bestDiscount.DiscountValue,
            FinalPrice = finalPrice
        };
    }
    
    private class DiscountCalculationResult
    {
        public string DiscountType { get; set; } = null!;
        public decimal DiscountValue { get; set; }
        public decimal FinalPrice { get; set; }
    }
}

// Image Service
public class ImageService : IImageService
{
    private readonly IImageRepo _repo;
    public ImageService(IImageRepo repo) => _repo = repo;

    public async Task<IEnumerable<ImageResponseDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<ImageResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : MapToDto(item);
    }

    public async Task<IEnumerable<ImageResponseDto>> GetByTypeAsync(string type)
    {
        var items = await _repo.GetByTypeAsync(type);
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<string>> GetDistinctTypesAsync()
    {
        return await _repo.GetDistinctTypesAsync();
    }

    public async Task<ImageResponseDto> CreateAsync(ImageCreateDto dto)
    {
        var entity = new Image
        {
            Url = dto.Url,
            Type = dto.Type,
            Status = dto.Status ?? "Active"
        };
        var created = await _repo.AddAsync(entity);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, ImageUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        if (dto.Url != null) entity.Url = dto.Url;
        if (dto.Type != null) entity.Type = dto.Type;
        if (dto.Status != null) entity.Status = dto.Status;

        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        await _repo.DeleteAsync(entity);
        return true;
    }

    private ImageResponseDto MapToDto(Image entity) => new()
    {
        Id = entity.Id,
        Url = entity.Url,
        Type = entity.Type,
        Status = entity.Status
    };
}

// Address Service
public class AddressService : IAddressService
{
    private readonly IAddressRepo _repo;
    private readonly SneakerShopContext _context;
    public AddressService(IAddressRepo repo, SneakerShopContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<IEnumerable<AddressResponseDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<AddressResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : MapToDto(item);
    }

    public async Task<IEnumerable<AddressResponseDto>> GetByAccountIdAsync(int accountId)
    {
        var items = await _repo.GetByAccountIdAsync(accountId);
        return items.Select(MapToDto);
    }

    public async Task<AddressResponseDto> CreateAsync(AddressCreateDto dto)
    {
        var entity = new Address
        {
            ProvinceName = dto.ProvinceName,
            DistrictName = dto.DistrictName,
            WardName = dto.WardName,
            ProvinceId = dto.ProvinceId,
            DistrictId = dto.DistrictId,
            WardCode = dto.WardCode,
            Status = dto.Status ?? "Active", // Mặc định là Active khi tạo mới
            ReceiverName = dto.ReceiverName,
            ReceiverPhone = dto.ReceiverPhone,
            ReceiverEmail = dto.ReceiverEmail,
            AccountId = dto.AccountId
        };
        var created = await _repo.AddAsync(entity);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, AddressUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        if (dto.ProvinceName != null) entity.ProvinceName = dto.ProvinceName;
        if (dto.DistrictName != null) entity.DistrictName = dto.DistrictName;
        if (dto.WardName != null) entity.WardName = dto.WardName;
        if (dto.ProvinceId.HasValue) entity.ProvinceId = dto.ProvinceId;
        if (dto.DistrictId.HasValue) entity.DistrictId = dto.DistrictId;
        if (dto.WardCode != null) entity.WardCode = dto.WardCode;
        if (dto.Status != null) entity.Status = dto.Status;
        if (dto.ReceiverName != null) entity.ReceiverName = dto.ReceiverName;
        if (dto.ReceiverPhone != null) entity.ReceiverPhone = dto.ReceiverPhone;
        if (dto.ReceiverEmail != null) entity.ReceiverEmail = dto.ReceiverEmail;

        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        
        // Soft delete: chỉ thay đổi status thành Inactive thay vì xóa thật sự
        entity.Status = "Inactive";
        await _repo.UpdateAsync(entity);
        return true;
    }

    private AddressResponseDto MapToDto(Address entity) => new()
    {
        Id = entity.Id,
        ProvinceName = entity.ProvinceName,
        DistrictName = entity.DistrictName,
        WardName = entity.WardName,
        ProvinceId = entity.ProvinceId,
        DistrictId = entity.DistrictId,
        WardCode = entity.WardCode,
        Status = entity.Status,
        ReceiverName = entity.ReceiverName,
        ReceiverPhone = entity.ReceiverPhone,
        ReceiverEmail = entity.ReceiverEmail,
        AccountId = entity.AccountId
    };
}

// Voucher Service
public class VoucherService : IVoucherService
{
    private readonly IVoucherRepo _repo;
    public VoucherService(IVoucherRepo repo) => _repo = repo;

    public async Task<IEnumerable<VoucherResponseDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<VoucherResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : MapToDto(item);
    }

    public async Task<VoucherResponseDto?> GetByCodeAsync(string code)
    {
        var item = await _repo.GetByCodeAsync(code);
        return item == null ? null : MapToDto(item);
    }

    public async Task<VoucherResponseDto> CreateAsync(VoucherCreateDto dto)
    {
        var entity = new Voucher
        {
            Code = dto.Code,
            Name = dto.Name,
            Type = dto.Type,
            Discount = dto.Discount,
            MaxDiscount = dto.MaxDiscount,
            Quantity = dto.Quantity,
            Description = dto.Description,
            Status = dto.Status ?? "Active",
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CategoryId = dto.CategoryId,
            CreateBy = dto.CreateBy
        };
        var created = await _repo.AddAsync(entity);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, VoucherUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        entity.Name = dto.Name;
        entity.Type = dto.Type;
        entity.Discount = dto.Discount;
        entity.MaxDiscount = dto.MaxDiscount;
        entity.Quantity = dto.Quantity;
        entity.Description = dto.Description;
        entity.Status = dto.Status;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;
        entity.UpdateBy = dto.UpdateBy;

        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        await _repo.DeleteAsync(entity);
        return true;
    }

    private VoucherResponseDto MapToDto(Voucher entity) => new()
    {
        Id = entity.Id,
        Code = entity.Code,
        Name = entity.Name,
        Type = entity.Type,
        Discount = entity.Discount,
        MaxDiscount = entity.MaxDiscount,
        Quantity = entity.Quantity,
        Description = entity.Description,
        Status = entity.Status,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        CategoryId = entity.CategoryId
    };
}
