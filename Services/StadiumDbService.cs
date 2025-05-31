using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Mutiview_BaseballPark.Models;

namespace Mutiview_BaseballPark.Services
{
    public class StadiumDbService
    {
        private readonly string _connectionString;

        public StadiumDbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Stadium>> GetStadiumsAsync()
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                // 使用 Dapper 執行 SQL 查詢
                // 確保 SQL 中的欄位名稱與 Stadium 模型中的 [Column] 屬性名稱一致
                string sql = "SELECT stadium_id AS StadiumId, stadium_name AS StadiumName, city AS City, country AS Country, capacity AS Capacity, opened_date AS OpenedDate, surface_type AS SurfaceType, status AS Status, home_team AS HomeTeam, main_image_url AS MainImageUrlFilename FROM stadiums order by stadium_id";
                var stadiums = await dbConnection.QueryAsync<Stadium>(sql);
                return stadiums;
            }
        }

        // 你可以在這裡添加其他 Stadium 相關的資料庫操作方法
    }
} 