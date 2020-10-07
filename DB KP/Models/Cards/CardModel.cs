using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB_KP.Models
{

    public class CardModel
    {
        [Column("suit_id")]
        public int SuitId { set; get; }
        public int Rank { set; get; }
        public int Score { set; get; }
        [Key]
        [Column("card_id")]
        public int CardId { set; get; }
    }
}