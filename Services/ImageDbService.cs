using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Mutiview_BaseballPark.Models;

namespace Mutiview_BaseballPark.Services
{
    public class ImageDbService
    {
        private readonly string _connectionString;

        public ImageDbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Image>> GetImagesByStadiumIdAsync(int stadiumId)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT image_id AS Id, stadium_id AS StadiumId, filename, upload_date AS UploadDate, section, row, seat_number AS SeatNumber, created_by AS CreatedBy FROM images WHERE stadium_id = @StadiumId";
                var images = await dbConnection.QueryAsync<Image>(sql, new { StadiumId = stadiumId });
                return images;
            }
        }

        // 你可以在這裡添加其他資料庫操作方法，例如 AddImageAsync 等
    }
} 