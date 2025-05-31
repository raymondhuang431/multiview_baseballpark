using System;

namespace Mutiview_BaseballPark.Models
{
    public class StadiumViewModel
    {
        public int StadiumId { get; set; }
        public string StadiumName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        // 其他你可能想在主頁顯示的 Stadium 屬性可以加在這裡

        public string MainImageUrl { get; set; } // 球場正面圖的 Firebase URL
    }
} 