using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace Mutiview_BaseballPark.Services
{
    public class FirebaseService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public FirebaseService(IWebHostEnvironment environment)
        {
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
            try
            {
                // 檢查檔案是否存在
                var obj = _storageClient.GetObject(_bucketName, imagePath);
                if (obj == null)
                {
                    throw new Exception($"找不到圖片: {imagePath}");
                }

                // 使用 Firebase Storage 的官方 URL 格式
                var encodedPath = Uri.EscapeDataString(imagePath);
                var url = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{encodedPath}?alt=media";
                return url;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting image URL: {ex.Message}");
            }
        }
    }
} 