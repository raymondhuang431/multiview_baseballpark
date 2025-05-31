using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mutiview_BaseballPark.Models
{
    [Table("images")]
    public class Image
    {
        [Key]
        [Column("image_id")]
        public int Id { get; set; }

        [Required]
        [Column("stadium_id")]
        public int StadiumId { get; set; }

        [Required]
        [StringLength(255)]
        [Column("filename")]
        public string Filename { get; set; }

        [Required]
        [Column("upload_date")]
        public DateTime UploadDate { get; set; }

        [StringLength(50)]
        [Column("section")]
        public string Section { get; set; }

        [StringLength(50)]
        [Column("row")]
        public string Row { get; set; }

        [StringLength(50)]
        [Column("seat_number")]
        public string SeatNumber { get; set; }

        [Required]
        [StringLength(100)]
        [Column("created_by")]
        public string CreatedBy { get; set; }

        // 根據需要，你可以添加導航屬性，例如 Stadium
        // public Stadium Stadium { get; set; }
    }
} 