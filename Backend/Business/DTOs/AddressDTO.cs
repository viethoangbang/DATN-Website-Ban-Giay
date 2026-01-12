using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

// Address DTOs
public class AddressResponseDto
{
    public int Id { get; set; }
    public string? ProvinceName { get; set; }
    public string? DistrictName { get; set; }
    public string? WardName { get; set; }
    
    // GHN location IDs
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public string? WardCode { get; set; }
    
    public string? Status { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? ReceiverEmail { get; set; }
    public int? AccountId { get; set; }
}

public class AddressCreateDto
{
    [Required(ErrorMessage = "Province name is required")]
    public string ProvinceName { get; set; } = null!;

    [Required(ErrorMessage = "District name is required")]
    public string DistrictName { get; set; } = null!;

    [Required(ErrorMessage = "Ward name is required")]
    public string WardName { get; set; } = null!;
    
    // GHN location IDs
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public string? WardCode { get; set; }

    public string? Status { get; set; }

    [Required(ErrorMessage = "Receiver name is required")]
    public string ReceiverName { get; set; } = null!;

    [Required(ErrorMessage = "Receiver phone is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string ReceiverPhone { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? ReceiverEmail { get; set; }
    
    public int? AccountId { get; set; }
}

public class AddressUpdateDto
{
    public string? ProvinceName { get; set; }
    public string? DistrictName { get; set; }
    public string? WardName { get; set; }
    
    // GHN location IDs
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public string? WardCode { get; set; }
    
    public string? Status { get; set; }
    public string? ReceiverName { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? ReceiverPhone { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? ReceiverEmail { get; set; }
}
