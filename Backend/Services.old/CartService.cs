using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Business.Services;

public class CartService : ICartService
{
    private readonly ICartRepo _cartRepo;
    private readonly ICartDetailRepo _cartDetailRepo;
    private readonly IProductDetailRepo _productDetailRepo;
    private readonly IDiscountRepo _discountRepo;

    public CartService(ICartRepo cartRepo, ICartDetailRepo cartDetailRepo, IProductDetailRepo productDetailRepo, IDiscountRepo discountRepo)
    {
        _cartRepo = cartRepo;
        _cartDetailRepo = cartDetailRepo;
        _productDetailRepo = productDetailRepo;
        _discountRepo = discountRepo;
    }

    public async Task<CartResponseDto?> GetMyCartAsync(int accountId)
    {
        var cart = await _cartRepo.GetActiveCartByAccountIdAsync(accountId);
        if (cart == null) return null;

        var items = new List<CartDetailResponseDto>();
        
        foreach (var cd in cart.CartDetails)
        {
            var productDetail = cd.ProductDetail;
            if (productDetail == null) continue;
            
            var originalPrice = productDetail.Price ?? 0;
            var finalPrice = originalPrice;
            var discountInfo = CalculateDiscountForProductDetail(productDetail);
            
            if (discountInfo != null)
            {
                finalPrice = discountInfo.FinalPrice;
            }
            
            items.Add(new CartDetailResponseDto
            {
                Id = cd.Id,
                Quantity = cd.Quantity,
                ProductDetailId = cd.ProductDetailId,
                ProductName = productDetail.Product?.Name,
                Price = originalPrice, // Giá gốc
                FinalPrice = finalPrice, // Giá sau discount
                ColorName = productDetail.Color?.Name,
                SizeName = productDetail.Size?.SizeName,
                ImageUrl = productDetail.Image?.Url,
                DiscountType = discountInfo?.DiscountType,
                DiscountValue = discountInfo?.DiscountValue,
                DiscountActive = discountInfo != null
            });
        }

        return new CartResponseDto
        {
            Id = cart.Id,
            CreateDate = cart.CreateDate,
            EndDate = cart.EndDate,
            AccountId = cart.AccountId,
            Items = items
        };
    }
    
    // Tính discount cho ProductDetail (giống logic trong ProductDetailService)
    private DiscountCalculationResult? CalculateDiscountForProductDetail(ProductDetail productDetail)
    {
        if (!productDetail.ProductId.HasValue || !productDetail.Price.HasValue)
            return null;
            
        var now = DateTime.Now;
        
        // Lấy tất cả discount active
        var activeDiscounts = _discountRepo.GetActiveByProductIdAsync(productDetail.ProductId.Value).Result
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
                discountAmount = productDetail.Price.Value * (discount.DiscountValue / 100);
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
        decimal finalPrice = productDetail.Price.Value - maxDiscountAmount;
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

    public async Task<CartResponseDto> AddToCartAsync(int accountId, AddToCartDto dto)
    {
        // Validate product detail exists and has stock
        var productDetail = await _productDetailRepo.GetByIdAsync(dto.ProductDetailId);
        if (productDetail == null)
            throw new NotFoundException("ProductDetail", dto.ProductDetailId);

        if (productDetail.Quantity < dto.Quantity)
            throw new InsufficientStockException(dto.ProductDetailId, dto.Quantity, productDetail.Quantity ?? 0);

        // Get or create cart
        var cart = await _cartRepo.GetActiveCartByAccountIdAsync(accountId);
        if (cart == null)
        {
            cart = new Cart
            {
                AccountId = accountId,
                CreateDate = DateTime.Now
            };
            cart = await _cartRepo.AddAsync(cart);
        }

        // Check if product already in cart
        var existingItem = await _cartDetailRepo.GetByCartAndProductDetailAsync(cart.Id, dto.ProductDetailId);
        if (existingItem != null)
        {
            var newQuantity = (existingItem.Quantity ?? 0) + dto.Quantity;
            if (productDetail.Quantity < newQuantity)
                throw new InsufficientStockException(dto.ProductDetailId, newQuantity, productDetail.Quantity ?? 0);

            existingItem.Quantity = newQuantity;
            await _cartDetailRepo.UpdateAsync(existingItem);
        }
        else
        {
            var cartDetail = new CartDetail
            {
                CartId = cart.Id,
                ProductDetailId = dto.ProductDetailId,
                Quantity = dto.Quantity
            };
            await _cartDetailRepo.AddAsync(cartDetail);
        }

        return (await GetMyCartAsync(accountId))!;
    }

    public async Task<bool> UpdateCartItemAsync(int cartDetailId, UpdateCartItemDto dto)
    {
        var item = await _cartDetailRepo.GetByIdAsync(cartDetailId);
        if (item == null) 
            throw new NotFoundException("CartDetail", cartDetailId);

        // Validate stock
        var productDetail = await _productDetailRepo.GetByIdAsync(item.ProductDetailId ?? 0);
        if (productDetail == null)
            throw new NotFoundException("ProductDetail", item.ProductDetailId ?? 0);

        if (productDetail.Quantity < dto.Quantity)
            throw new InsufficientStockException(item.ProductDetailId ?? 0, dto.Quantity, productDetail.Quantity ?? 0);

        item.Quantity = dto.Quantity;
        await _cartDetailRepo.UpdateAsync(item);
        return true;
    }

    public async Task<bool> RemoveFromCartAsync(int cartDetailId)
    {
        var item = await _cartDetailRepo.GetByIdAsync(cartDetailId);
        if (item == null) 
            throw new NotFoundException("CartDetail", cartDetailId);

        await _cartDetailRepo.DeleteAsync(item);
        return true;
    }

    public async Task<bool> ClearCartAsync(int accountId)
    {
        var cart = await _cartRepo.GetActiveCartByAccountIdAsync(accountId);
        if (cart == null) 
            throw new NotFoundException($"No active cart found for account {accountId}");

        cart.EndDate = DateTime.Now;
        await _cartRepo.UpdateAsync(cart);
        return true;
    }
}

