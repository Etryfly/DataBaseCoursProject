using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace DBKP.Models
{
    public class ApplicationContext : DbContext
    {
        
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MoneyModel> Money { get; set; }
        public DbSet<GameStatsModel> GameStats { get; set; }
        public DbSet<AchievementsModel> Achievement { get; set; }
        public DbSet<BetsModel> Bets { get; set; }
        public DbSet<CardModel> Card { get; set; }
        public DbSet<HandModel> Hand { get; set; }
        public DbSet<HandCardModel> HandCard { get; set; }
                
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
            optionsBuilder.UseLoggerFactory(DblLoggerFactory);
        }
        
        public static readonly ILoggerFactory DblLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new DBLoggerProvider());    // указываем наш провайдер логгирования
        });
    }
}