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
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupSession>()
               .HasOne(gs => gs.Trainer)
               .WithMany(t => t.GroupSessions)
               .HasForeignKey(pts => pts.TrainerId)
               .OnDelete(DeleteBehavior.NoAction);
        }

        // Seed Method
        public void Seed()
        {
            // Check if data already exists in each table
            if (!Memberships.Any())
            {
                // Seed Memberships
                var memberships = new List<Membership>();
                var membershipTypes = new[] { "Standard", "Premium", "Ultimate" };
                var membershipCardImages = new[] { "https://i.pinimg.com/originals/64/84/79/648479153d80440b56e0bdc59be624f8.png",
                                                  "https://i.pinimg.com/originals/ab/19/38/ab1938fdc1eab11424ab8272a476e20e.png",
                                                  "https://i.pinimg.com/originals/3f/37/29/3f37299ce618797a262c36c949276dba.png"};
                for (int i = 1; i <= 3; i++)
                {
                    memberships.Add(new Membership
                    {
                        MembershipId = i,
                        Type = membershipTypes[i-1],
                        Price = 50 * i,
                        DurationInDays = 30,
                        PhotoUrl = membershipCardImages[i-1]
                    });
                }
                Memberships.AddRange(memberships);
            }

            if (!Members.Any())
            {
                // Seed Members
                var members = new List<Member>();
                for (int i = 1; i <= 5; i++)
                {
                    members.Add(new Member
                    {
                        MemberId = i,
                        FullName = $"Member {i}",
                        Email = $"member{i}@example.com",
                        Password = "password123",
                        ProfilePicUrl = "https://hips.hearstapps.com/hmg-prod/images/chris-64a7f18b4e9a3.png?crop=0.855xw:1.00xh;0.0238xw,0&resize=640:*",
                        DateOfBirth = DateTime.Now.AddYears(-30).AddDays(i * -365),
                        MembershipId = i % 2 == 0 ? 2 : 1,
                        RegistrationDate = DateTime.Now.AddMonths(-i * 3),
                        IsActive = i % 2 == 0
                    });
                }
                Members.AddRange(members);
            }

            if (!Trainers.Any())
            {
                // Seed Trainers
                var trainers = new List<Trainer>();
                var trainerNames = new[] { "Bob Walker", "Son Johnson", "Cathy Davis" };
                var trainerImages = new[] { "https://media.istockphoto.com/id/675179390/photo/muscular-trainer-writing-on-clipboard.jpg?s=612x612&w=0&k=20&c=9NKx1AwVMpPY0YBlk5H-hxx2vJSCu1Wc78BKRM9wFq0=", 
                                            "https://bryanuniversity.edu/wp-content/uploads/personal-train-undergrad-cert@2x-scaled.jpg",
                                            "https://media.istockphoto.com/id/856797530/photo/portrait-of-a-beautiful-woman-at-the-gym.jpg?s=612x612&w=0&k=20&c=0wMa1MYxt6HHamjd66d5__XGAKbJFDFQyu9LCloRsYU="};
                for (int i = 1; i <= trainerNames.Length; i++)
                {
                    trainers.Add(new Trainer
                    {
                        TrainerId = i,
                        FullName = trainerNames[i - 1],
                        Description = $"Trainer {i} Description",
                        Email = $"trainer{i}@example.com",
                        YearsOfExperience = 5 + i,
                        PictureUrl = trainerImages[i-1]
                    });
                }
                Trainers.AddRange(trainers);
            }

            if (!PersonalTrainingSessions.Any())
            {
                // Seed Personal Training Sessions
                var sessions = new List<PersonalTrainingSession>();
                for (int i = 1; i <= 5; i++)
                {
                    sessions.Add(new PersonalTrainingSession
                    {
                        SessionId = i,
                        MemberId = i,
                        TrainerId = (i % 3) + 1, // Cycling through trainers
                        SessionDate = DateTime.Now.AddDays(i).Date.AddHours(7 + i),
                        Duration = 45 + (i * 15)
                    });
                }
                PersonalTrainingSessions.AddRange(sessions);
            }


            // Seed Group Sessions
            if (!GroupSessions.Any())
            {
                // Retrieve all trainers and members from the database
                var trainers = Trainers.ToList();
                var members = Members.ToList();

                // Create a list to hold the group sessions
                var groupSessions = new List<GroupSession>();

                // Loop through each trainer
                foreach (var trainer in trainers)
                {
                    // Generate a session for each trainer
                    groupSessions.Add(new GroupSession
                    {
                        GroupSessionId = trainer.TrainerId, // Ensure unique ID (can be handled automatically by DB)
                        SessionName = $"{trainer.FullName}'s Weekly Session",
                        TrainerId = trainer.TrainerId,
                        SessionDate = DateTime.Now.AddDays((int)DayOfWeek.Monday + trainer.TrainerId), // Each trainer's session is on a different weekday
                        Duration = 60, // 1-hour session
                        MaxParticipants = 3, // 3 members per session
                        Members = members.Take(3).ToList() // Assign the first three members to the session
                    });
                }

                // Add the group sessions to the database
                GroupSessions.AddRange(groupSessions);
            }

            // Save changes to the database
            SaveChanges();
        }


    }
}
