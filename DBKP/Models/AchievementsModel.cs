using System.ComponentModel.DataAnnotations;

namespace DBKP.Models
{
    public class AchievementsModel
    {
        [Key]
        public int UserId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
    }
}