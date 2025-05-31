using System;

namespace Mutiview_BaseballPark.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public int StadiumId { get; set; }
        public string StadiumName { get; set; }
        public string Filename { get; set; }
        public DateTime UploadDate { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public string SeatNumber { get; set; }
        public string CreatedBy { get; set; }
        public string ImageUrl { get; set; } // 新增的圖片 URL 屬性
    }
} 