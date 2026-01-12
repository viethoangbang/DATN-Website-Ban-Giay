using Business.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CloudinaryService> _logger;

    public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        var cloudName = _configuration["Cloudinary:CloudName"];
        var apiKey = _configuration["Cloudinary:ApiKey"];
        var apiSecret = _configuration["Cloudinary:ApiSecret"];

        _logger.LogInformation($"Loading Cloudinary config - CloudName: {cloudName}, ApiKey: {apiKey?.Substring(0, Math.Min(5, apiKey?.Length ?? 0))}...");

        if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
        {
            var errorMsg = "Cloudinary configuration is missing. Please check appsettings.json";
            _logger.LogError(errorMsg);
            throw new InvalidOperationException(errorMsg);
        }

        try
        {
            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
            _logger.LogInformation("Cloudinary service initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Cloudinary service");
            throw;
        }
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string? folder = null)
    {
        try
        {
            _logger.LogInformation($"Uploading image: {fileName} to folder: {folder ?? "sneaker-poly"}");
            
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, imageStream),
                Folder = folder ?? "sneaker-poly",
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            _logger.LogInformation($"Upload result status: {uploadResult.StatusCode}, PublicId: {uploadResult.PublicId}");

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK && uploadResult.SecureUrl != null)
            {
                _logger.LogInformation($"Image uploaded successfully: {uploadResult.SecureUrl}");
                return uploadResult.SecureUrl.ToString();
            }

            var errorMsg = uploadResult.Error?.Message ?? "Unknown error";
            _logger.LogError($"Cloudinary upload failed: {errorMsg}");
            throw new Exception($"Cloudinary upload failed: {errorMsg}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error uploading image to Cloudinary: {ex.Message}");
            throw new Exception($"Error uploading image to Cloudinary: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        try
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting image from Cloudinary: {ex.Message}", ex);
        }
    }
}

