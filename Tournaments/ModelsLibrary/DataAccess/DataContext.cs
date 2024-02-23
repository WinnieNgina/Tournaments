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
            modelBuilder.Entity<PlayerTeamModel>()
                .HasKey(ptm => new { ptm.PlayerId, ptm.TeamId });

            // Configure the foreign key relationships
            modelBuilder.Entity<PlayerTeamModel>()
                .HasOne(ptm => ptm.Player)
                .WithMany(p => p.Teams)
                .HasForeignKey(ptm => ptm.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PlayerTeamModel>()
                .HasOne(ptm => ptm.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(ptm => ptm.TeamId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<MatchUpModel>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.MatchUps)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Cascade to NoAction
            modelBuilder.Entity<ParticipationModel>()
                .HasOne(p => p.Tournament)
                .WithMany(t => t.Participations)
                .HasForeignKey(p => p.TournamentId)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Cascade to NoAction

            modelBuilder.Entity<ReviewModel>()
                .HasOne(r => r.Tournament)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.TournamentId)
                .OnDelete(DeleteBehavior.NoAction); // Changed from Cascade to NoAction

            modelBuilder.Entity<TeamModel>()
                .HasIndex(t => t.Name)
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<CoachModel>("Coach")
                .HasValue<PlayerModel>("Player")
                .HasValue<TournamentOrganizerModel>("TournamentOrganizer");
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
