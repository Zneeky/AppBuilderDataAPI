using Microsoft.EntityFrameworkCore;
using AppBuilderDataAPI.Data.Models;
using System;

namespace AppBuilderDataAPI.Data
{
    public class AppBuilderDataDbContext : DbContext
    {
        public AppBuilderDataDbContext(DbContextOptions<AppBuilderDataDbContext> options) : base(options)
        {
        }

        public DbSet<DietPlan> DietPlans { get; set; }
        public DbSet<Exercise> Exercises { get; set; } = null!;
        public DbSet<GroupSession> GroupSessions { get; set; }
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Meal> Meals { get; set; } = null!;
        public DbSet<Membership> Memberships { get; set; } = null!;
        public DbSet<PersonalTrainingSession> PersonalTrainingSessions { get; set; } = null!;
        public DbSet<Trainer> Trainers { get; set; } = null!;
        public DbSet<TrainingProgram> TrainingPrograms { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalTrainingSession>()
                .HasOne(pts => pts.Member)
                .WithMany(m => m.PersonalTrainingSessions)
                .HasForeignKey(pts => pts.MemberId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PersonalTrainingSession>()
                .HasOne(pts => pts.Trainer)
                .WithMany(t => t.PersonalTrainingSessions)
                .HasForeignKey(pts => pts.TrainerId)
                .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<GroupSession>()
               .HasOne(gs => gs.Trainer)
               .WithMany(t => t.GroupSessions)
               .HasForeignKey(pts => pts.TrainerId)
               .OnDelete(DeleteBehavior.NoAction); ;


            // Seed Memberships (required for the foreign key MembershipId in Member)
            modelBuilder.Entity<Membership>().HasData(
                new Membership { MembershipId = 1, Type = "Standard", Price = 50, DurationInDays = 30 },
                new Membership { MembershipId = 2, Type = "Premium", Price = 100, DurationInDays = 30 }
            );

            // Seed Members
            modelBuilder.Entity<Member>().HasData(
                new Member
                {
                    MemberId = 1,
                    FullName = "John Doe",
                    Email = "john.doe@example.com",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    MembershipId = 1, // Standard Membership
                    RegistrationDate = DateTime.Now.AddYears(-1), // Registered 1 year ago
                    IsActive = true
                },
                new Member
                {
                    MemberId = 2,
                    FullName = "Jane Smith",
                    Email = "jane.smith@example.com",
                    DateOfBirth = new DateTime(1990, 3, 22),
                    MembershipId = 2, // Premium Membership
                    RegistrationDate = DateTime.Now.AddMonths(-6), // Registered 6 months ago
                    IsActive = true
                },
                new Member
                {
                    MemberId = 3,
                    FullName = "Michael Brown",
                    Email = "michael.brown@example.com",
                    DateOfBirth = new DateTime(1982, 8, 10),
                    MembershipId = 1, // Standard Membership
                    RegistrationDate = DateTime.Now.AddMonths(-3), // Registered 3 months ago
                    IsActive = true
                },
                new Member
                {
                    MemberId = 4,
                    FullName = "Alice Johnson",
                    Email = "alice.johnson@example.com",
                    DateOfBirth = new DateTime(1995, 1, 17),
                    MembershipId = 2, // Premium Membership
                    RegistrationDate = DateTime.Now.AddYears(-2), // Registered 2 years ago
                    IsActive = false // Inactive member
                },
                new Member
                {
                    MemberId = 5,
                    FullName = "Chris Evans",
                    Email = "chris.evans@example.com",
                    DateOfBirth = new DateTime(1988, 11, 5),
                    MembershipId = 1, // Standard Membership
                    RegistrationDate = DateTime.Now.AddYears(-1).AddMonths(-2), // Registered 14 months ago
                    IsActive = true
                }
            );


            modelBuilder.Entity<Trainer>().HasData(
                new Trainer
                {
                    TrainerId = 1,
                    FullName = "Alice Walker",
                    Description = "Alice specializes in strength training and rehabilitation. With over 10 years of experience, she is dedicated to helping clients achieve their fitness goals.",
                    Email = "alice.walker@example.com",
                    YearsOfExperience = 10,
                    PictureUrl = "https://example.com/images/alice-walker.jpg"
                },
                new Trainer
                {
                    TrainerId = 2,
                    FullName = "Bob Johnson",
                    Description = "Bob is a certified personal trainer with a passion for helping clients build endurance and improve cardiovascular health.",
                    Email = "bob.johnson@example.com",
                    YearsOfExperience = 7,
                    PictureUrl = "https://example.com/images/bob-johnson.jpg"
                },
                new Trainer
                {
                    TrainerId = 3,
                    FullName = "Cathy Davis",
                    Description = "Cathy is a yoga instructor and personal trainer with 5 years of experience, focusing on flexibility, balance, and mental well-being.",
                    Email = "cathy.davis@example.com",
                    YearsOfExperience = 5,
                    PictureUrl = "https://example.com/images/cathy-davis.jpg"
                }
            );

            // Seed Personal Training Sessions (for next week)
            modelBuilder.Entity<PersonalTrainingSession>().HasData(
                new PersonalTrainingSession
                {
                    SessionId = 1,
                    MemberId = 1, // John Doe
                    TrainerId = 1, // Alice Walker
                    SessionDate = DateTime.Now.AddDays(1).Date.AddHours(9), // Tomorrow at 9:00 AM
                    Duration = 60 // 1-hour session
                },
                new PersonalTrainingSession
                {
                    SessionId = 2,
                    MemberId = 2, // Jane Smith
                    TrainerId = 2, // Bob Johnson
                    SessionDate = DateTime.Now.AddDays(2).Date.AddHours(10), // 2 days later at 10:00 AM
                    Duration = 90 // 1.5-hour session
                },
                new PersonalTrainingSession
                {
                    SessionId = 3,
                    MemberId = 3, // Michael Brown
                    TrainerId = 3, // Cathy Davis
                    SessionDate = DateTime.Now.AddDays(3).Date.AddHours(11), // 3 days later at 11:00 AM
                    Duration = 60 // 1-hour session
                },
                new PersonalTrainingSession
                {
                    SessionId = 4,
                    MemberId = 1, // John Doe
                    TrainerId = 2, // Bob Johnson
                    SessionDate = DateTime.Now.AddDays(4).Date.AddHours(8), // 4 days later at 8:00 AM
                    Duration = 120 // 2-hour session
                },
                new PersonalTrainingSession
                {
                    SessionId = 5,
                    MemberId = 4, // Alice Johnson
                    TrainerId = 1, // Alice Walker
                    SessionDate = DateTime.Now.AddDays(5).Date.AddHours(7), // 5 days later at 7:00 AM
                    Duration = 45 // 45-minute session
                }
            );

        }

    }
}
