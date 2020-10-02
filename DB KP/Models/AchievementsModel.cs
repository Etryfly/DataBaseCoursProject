using System.ComponentModel.DataAnnotations;

namespace DB_KP.Models
{
    public class AchievementsModel
    {
        [Key]
        public int UserId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
    }
}