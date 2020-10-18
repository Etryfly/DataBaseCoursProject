using System.ComponentModel.DataAnnotations;

namespace DBKP.Models
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