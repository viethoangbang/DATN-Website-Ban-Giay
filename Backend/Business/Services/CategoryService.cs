using Business.DTOs;
using Business.Interfaces;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _repository;

    public CategoryService(ICategoryRepo repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Status = c.Status,
            CreateDate = c.CreateDate,
            UpdateDate = c.UpdateDate,
            CreateBy = c.CreateBy,
            UpdateBy = c.UpdateBy,
            Condition = c.Condition
        });
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return null;

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Status = category.Status,
            CreateDate = category.CreateDate,
            UpdateDate = category.UpdateDate,
            CreateBy = category.CreateBy,
            UpdateBy = category.UpdateBy,
            Condition = category.Condition
        };
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            CreateBy = dto.CreateBy,
            Condition = dto.Condition,
            CreateDate = DateTime.Now
        };

        var created = await _repository.AddAsync(category);
        return new CategoryResponseDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            Status = created.Status,
            CreateDate = created.CreateDate,
            CreateBy = created.CreateBy,
            Condition = created.Condition
        };
    }

    public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return false;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.Status = dto.Status;
        category.UpdateBy = dto.UpdateBy;
        category.Condition = dto.Condition;
        category.UpdateDate = DateTime.Now;

        await _repository.UpdateAsync(category);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return false;

        await _repository.DeleteAsync(category);
        return true;
    }
}
