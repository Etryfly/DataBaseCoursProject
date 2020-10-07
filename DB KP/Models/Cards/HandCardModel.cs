using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB_KP.Models
{
    [Table("hand_card")]
    public class HandCardModel
    {
        [Key]
        [Column("hand_card_id")]
        public int HandCardId { set; get; }
        [Column("hand_id")]
        public int HandId { set; get; }
        [Column("card_id")]
        public int CardId { set; get; }
    }
}