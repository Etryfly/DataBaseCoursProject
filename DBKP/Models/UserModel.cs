using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace DBKP.Models
{
    public class UserModel
    {
        [Key]
        public int Id { set; get; }
        public string Login { set; get; }
        public string Password { set; get; }
        
        public int Role_id { set; get; }
        public MoneyModel Money;
        public GameStatsModel GameStats;

    }
}