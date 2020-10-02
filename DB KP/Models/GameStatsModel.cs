using System.ComponentModel.DataAnnotations;

namespace DB_KP.Models
{
    public class GameStatsModel
    {
        [Key]
        public int Id { set; get; }
        public int Loses { set; get; }
        public int Wins { set; get; }
        public int chips_earned { set; get; }
    }
}