using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBKP.Models
{
    public class GameStatsModel
    {
        [Key]
        public int Id { set; get; }
        public int Loses { set; get; }
        public int Wins { set; get; }
        [Column(TypeName="money")]
        public decimal chips_earned { set; get; }
        [Column(TypeName="money")]
        public decimal chips_loosed { set; get; }
        [Column(TypeName="money")]
        public decimal payed { get; set; }
    }
}