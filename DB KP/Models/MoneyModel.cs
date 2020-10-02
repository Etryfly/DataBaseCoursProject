using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace DB_KP.Models
{
    public class MoneyModel
    {
        [Key]
        public int Id { set; get; }
        [Column(TypeName="money")]
        public decimal Chips { get; set; }
    }
}