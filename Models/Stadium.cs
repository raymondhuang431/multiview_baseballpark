using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mutiview_BaseballPark.Models
{
    [Table("stadiums")]
    public class Stadium
    {
        [Key]
        [Column("stadium_id")]
        public int StadiumId { get; set; }

        [Column("stadium_name")]
        public string StadiumName { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("country")]
        public string Country { get; set; }

        [Column("capacity")]
        public int? Capacity { get; set; }

        [Column("opened_date")]
        public DateTime? OpenedDate { get; set; }

        [Column("surface_type")]
        public string SurfaceType { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("home_team")]
        public string HomeTeam { get; set; }
    }
} 