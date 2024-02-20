using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;
using ModelsLibrary.Utilities;

namespace ModelsLibrary.DataAccess
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<CoachModel> Coach { get; set; }
        public DbSet<MatchUpEntryModel> Entries { get; set; }
        public DbSet<MatchUpModel> MatchUp { get; set; }
        public DbSet<NextOfKinModel> NextOfKin { get; set; }
        public DbSet<ParticipationModel> Participation { get; set; }
        public DbSet<PlayerModel> Player { get; set; }
        public DbSet<PrizeModel> Prize { get; set; }
        public DbSet<ReviewModel> Review { get; set; }
        public DbSet<TeamModel> Team { get; set; }
        public DbSet<TournamentModel> Tournament { get; set; }
        public DbSet<TournamentOrganizerModel> Organizer { get; set; }
        public DbSet<TournamentPrizeModel> TournamentPrizes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = RoleNames.Player, ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "PLAYER" },
                new IdentityRole() { Name = RoleNames.Organizer, ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "ORGANIZER" },
                new IdentityRole() { Name = RoleNames.Coach, ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "COACH" }
                );
        }
    }
}
