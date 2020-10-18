using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBKP.Models
{
    [Table("bets", Schema = "dbo")]
    public class BetsModel
    {
        [Key]
        [Column("user_id")]
        public int UserId { set; get; } 
        
        [Column(TypeName="money")]
        public decimal Bet { get; set; }
    }    
}