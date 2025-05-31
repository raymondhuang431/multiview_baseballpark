using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace Mutiview_BaseballPark.Services
{
    public class FirebaseService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        private readonly ILogger<FirebaseService> _logger;
        private readonly IMemoryCache _cache;

        public FirebaseService(IWebHostEnvironment environment, ILogger<FirebaseService> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;

            // 從環境變數或檔案讀取認證
            GoogleCredential credential;
            var firebaseCredentials = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
            
            if (!string.IsNullOrEmpty(firebaseCredentials))
            {
                // 從環境變數讀取認證
                var credentialsJson = JsonDocument.Parse(firebaseCredentials);
                credential = GoogleCredential.FromJson(firebaseCredentials);
            }
            else
            {
                // 從檔案讀取認證（本地開發用）
                credential = GoogleCredential.FromFile("firebase-credentials.json");
            }
            
            // 初始化 Firebase Admin SDK
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = credential
                });
            }

            // 初始化 Storage Client
            _storageClient = StorageClient.Create(credential);
            _bucketName = "multiviewbaseballpark.firebasestorage.app";
        }

        public string GetImageUrl(string imagePath)
        {
            _logger.LogInformation("Attempting to get URL for image path: {ImagePath}", imagePath);
            
            // 嘗試從快取中獲取 URL
            if (_cache.TryGetValue(imagePath, out string cachedUrl))
            {
                _logger.LogInformation("Returning cached URL for image path: {ImagePath}", imagePath);
                return cachedUrl;
            }

            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    throw new Exception($"Image path is empty");
                }

                // 檢查檔案是否存在
                try
                {
                    _logger.LogInformation("Checking Firebase Storage object: bucket='{BucketName}', path='{ImagePath}'", _bucketName, imagePath);
                    var obj = _storageClient.GetObject(_bucketName, imagePath);
                    if (obj == null)
                    {
                        throw new Exception($"找不到圖片: {imagePath}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"無法存取圖片: {imagePath}, 錯誤: {ex.Message}");
                }

                // 使用 Firebase Storage 的官方 URL 格式
                var encodedPath = Uri.EscapeDataString(imagePath);
                var url = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{encodedPath}?alt=media";

                // 將 URL 存入快取，設定一個過期時間 (例如 24 小時)
                _cache.Set(imagePath, url, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });

                _logger.LogInformation("Successfully generated and cached URL for image path: {ImagePath}", imagePath);
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting image URL: {ErrorMessage}", ex.Message);
                throw new Exception($"Error getting image URL: {ex.Message}");
            }
        }
    }
} 