﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelsLibrary.DataAccess;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240304051650_InitialCreation")]
    partial class InitialCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "6282ac12-9f84-4415-bfb3-0799426bbabc",
                            ConcurrencyStamp = "3038fd04-a8b0-46fc-9b77-4c1325f4f601",
                            Name = "Player",
                            NormalizedName = "PLAYER"
                        },
                        new
                        {
                            Id = "b4dcd32c-03ca-4483-8d62-5547e69b9b5e",
                            ConcurrencyStamp = "d3e1a86d-80ae-4006-831a-7be0674cda27",
                            Name = "Organizer",
                            NormalizedName = "ORGANIZER"
                        },
                        new
                        {
                            Id = "0f9c62d8-2bd7-44b2-9054-dbb16e38dbf5",
                            ConcurrencyStamp = "b01fe558-e032-48ac-b46c-11bde57959b4",
                            Name = "Coach",
                            NormalizedName = "COACH"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ModelsLibrary.Models.MatchUpEntryModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MatchUpId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentMatchupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<string>("TeamCompetingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ParentMatchupId");

                    b.HasIndex("TeamCompetingId");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("ModelsLibrary.Models.MatchUpModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("MatchUpRound")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TournamentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WinningTeamId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.HasIndex("WinningTeamId");

                    b.ToTable("MatchUp");
                });

            modelBuilder.Entity("ModelsLibrary.Models.NextOfKinModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ContactInfo")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Relationship")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("NextOfKin");
                });

            modelBuilder.Entity("ModelsLibrary.Models.ParticipationModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("HasAttended")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("TeamId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TournamentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Participation");
                });

            modelBuilder.Entity("ModelsLibrary.Models.PlayerTeamModel", b =>
                {
                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PlayerId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("PlayerTeamModel");
                });

            modelBuilder.Entity("ModelsLibrary.Models.PrizeModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal>("PrizeAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("PrizePercentage")
                        .HasColumnType("float");

                    b.Property<string>("PrizeType")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Prize");
                });

            modelBuilder.Entity("ModelsLibrary.Models.ReviewModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TournamentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ReviewerId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TeamModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CoachId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CoachId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Team");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TournamentModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AverageRating")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<decimal>("EntryFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("OrganizerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("StructureType")
                        .HasColumnType("int");

                    b.Property<int>("TeamsLimit")
                        .HasColumnType("int");

                    b.Property<DateTime>("TournamentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OrganizerId");

                    b.ToTable("Tournament");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TournamentPrizeModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PrizeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TournamentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TournamentPrizeModelId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PrizeId");

                    b.HasIndex("TournamentId");

                    b.HasIndex("TournamentPrizeModelId");

                    b.ToTable("TournamentPrizes");
                });

            modelBuilder.Entity("ModelsLibrary.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AreaOfResidence")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecretKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("UserType").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ModelsLibrary.Models.CoachModel", b =>
                {
                    b.HasBaseType("ModelsLibrary.Models.User");

                    b.Property<string>("Achievements")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("CoachingSpecialization")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("SocialMediaUrl")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("YearsOfExperience")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Coach");
                });

            modelBuilder.Entity("ModelsLibrary.Models.PlayerModel", b =>
                {
                    b.HasBaseType("ModelsLibrary.Models.User");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("NextOfKinId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasIndex("NextOfKinId");

                    b.HasDiscriminator().HasValue("Player");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TournamentOrganizerModel", b =>
                {
                    b.HasBaseType("ModelsLibrary.Models.User");

                    b.Property<string>("OrganizationName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasDiscriminator().HasValue("TournamentOrganizer");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ModelsLibrary.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ModelsLibrary.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsLibrary.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ModelsLibrary.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModelsLibrary.Models.MatchUpEntryModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.MatchUpModel", "ParentMatchup")
                        .WithMany("Entries")
                        .HasForeignKey("ParentMatchupId");

                    b.HasOne("ModelsLibrary.Models.TeamModel", "TeamCompeting")
                        .WithMany()
                        .HasForeignKey("TeamCompetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentMatchup");

                    b.Navigation("TeamCompeting");
                });

            modelBuilder.Entity("ModelsLibrary.Models.MatchUpModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.TournamentModel", "Tournament")
                        .WithMany("MatchUps")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ModelsLibrary.Models.TeamModel", "WinningTeam")
                        .WithMany()
                        .HasForeignKey("WinningTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");

                    b.Navigation("WinningTeam");
                });

            modelBuilder.Entity("ModelsLibrary.Models.ParticipationModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.TeamModel", "Team")
                        .WithMany("Participations")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsLibrary.Models.TournamentModel", "Tournament")
                        .WithMany("Participations")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("ModelsLibrary.Models.PlayerTeamModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.PlayerModel", "Player")
                        .WithMany("Teams")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ModelsLibrary.Models.TeamModel", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ModelsLibrary.Models.ReviewModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.User", "Reviewer")
                        .WithMany("Reviews")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsLibrary.Models.TournamentModel", "Tournament")
                        .WithMany("Reviews")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Reviewer");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TeamModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.CoachModel", "Coach")
                        .WithMany("Teams")
                        .HasForeignKey("CoachId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coach");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TournamentModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.TournamentOrganizerModel", "Organizer")
                        .WithMany("Tournaments")
                        .HasForeignKey("OrganizerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organizer");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TournamentPrizeModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.PrizeModel", "Prize")
                        .WithMany()
                        .HasForeignKey("PrizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsLibrary.Models.TournamentModel", "Tournament")
                        .WithMany("TournamentPrizes")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsLibrary.Models.TournamentPrizeModel", null)
                        .WithMany("TournamentPrizes")
                        .HasForeignKey("TournamentPrizeModelId");

                    b.Navigation("Prize");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("ModelsLibrary.Models.PlayerModel", b =>
                {
                    b.HasOne("ModelsLibrary.Models.NextOfKinModel", "NextOfKin")
                        .WithMany("Players")
                        .HasForeignKey("NextOfKinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NextOfKin");
                });

            modelBuilder.Entity("ModelsLibrary.Models.MatchUpModel", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("ModelsLibrary.Models.NextOfKinModel", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TeamModel", b =>
                {
                    b.Navigation("Participations");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TournamentModel", b =>
                {
                    b.Navigation("MatchUps");

                    b.Navigation("Participations");

                    b.Navigation("Reviews");

                    b.Navigation("TournamentPrizes");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TournamentPrizeModel", b =>
                {
                    b.Navigation("TournamentPrizes");
                });

            modelBuilder.Entity("ModelsLibrary.Models.User", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("ModelsLibrary.Models.CoachModel", b =>
                {
                    b.Navigation("Teams");
                });

            modelBuilder.Entity("ModelsLibrary.Models.PlayerModel", b =>
                {
                    b.Navigation("Teams");
                });

            modelBuilder.Entity("ModelsLibrary.Models.TournamentOrganizerModel", b =>
                {
                    b.Navigation("Tournaments");
                });
#pragma warning restore 612, 618
        }
    }
}
