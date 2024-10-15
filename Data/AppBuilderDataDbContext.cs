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
            modelBuilder.Entity<Trainer>()
                .HasMany(t => t.PersonalTrainingSessions)
                .WithOne(pts => pts.Trainer)
                .HasForeignKey(pts => pts.TrainerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Trainer>()
                .HasMany(t => t.GroupSessions)
                .WithOne(pts => pts.Trainer)
                .HasForeignKey(pts => pts.TrainerId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Member>()
                .HasMany(t => t.GroupSessions)
                .WithMany(gs => gs.Members);

            modelBuilder.Entity<Member>()
                .HasMany(t => t.PersonalTrainingSessions)
                .WithOne(ps => ps.Member)
                .HasForeignKey(ps=>ps.MemberId)
                .OnDelete(DeleteBehavior.Cascade);


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

            modelBuilder.Entity<DietPlan>()
                .HasMany(dp => dp.Meals)
                .WithMany(m=>m.DietPlans);

            modelBuilder.Entity<Meal>()
                .HasMany(m => m.DietPlans)
                .WithMany(dp => dp.Meals);
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
                        SessionDate = DateTime.Today.AddHours(9 + i * 2),
                        Duration = 60
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
                        SessionDate = DateTime.Today.AddHours(10), // Each trainer's session is on a different weekday
                        Duration = 60, // 1-hour session
                        MaxParticipants = 3, // 3 members per session
                        Members = members.Take(3).ToList() // Assign the first three members to the session
                    });
                }

                // Add the group sessions to the database
                GroupSessions.AddRange(groupSessions);
            }


            if (!DietPlans.Any())
            {
                // Seed Meals
                var meals = new List<Meal>
                {
                    new Meal
                    {
                        MealId = 1,
                        Name = "Oatmeal with Berries",
                        Recipe = "Combine oats, almond milk, and top with fresh berries.",
                        Type = "Breakfast",
                        Calories = 350,
                        Protein = 10,
                        Carbs = 45,
                        Fat = 8,
                        PicUrl = "https://joybauer.com/wp-content/uploads/2017/12/Oatmeal-with-berries2.jpg"
                    },
                    new Meal
                    {
                        MealId = 2,
                        Name = "Grilled Chicken Salad",
                        Recipe = "Grill chicken breast and serve over mixed greens with olive oil and lemon dressing.",
                        Type = "Lunch",
                        Calories = 400,
                        Protein = 30,
                        Carbs = 15,
                        Fat = 20,
                        PicUrl = "https://www.crunchycreamysweet.com/wp-content/uploads/2018/06/easy-grilled-chicken-salad-1.jpg"
                    },
                    new Meal
                    {
                        MealId = 3,
                        Name = "Salmon with Quinoa",
                        Recipe = "Bake salmon and serve with cooked quinoa and steamed broccoli.",
                        Type = "Dinner",
                        Calories = 500,
                        Protein = 40,
                        Carbs = 35,
                        Fat = 22,
                        PicUrl = "https://www.eatingwell.com/thmb/f8nLtKqcDmCcg0_UNhT2Z5mVhO8=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/8110444-fe18d867dddf4b7ca90ac3c05dff42a0.jpg"
                    },
                    new Meal
                    {
                        MealId = 4,
                        Name = "Greek Yogurt with Almonds",
                        Recipe = "Serve Greek yogurt topped with a handful of almonds and a drizzle of honey.",
                        Type = "Snack",
                        Calories = 200,
                        Protein = 15,
                        Carbs = 10,
                        Fat = 12,
                        PicUrl =  "https://bebeloveokazu.com/wp-content/uploads/2013/03/Greek-Yogurt-Almond-Granola-Honey-2.jpg"
                    }
                };

                Meals.AddRange(meals);

                // Seed Diet Plans
                var dietPlans = new List<DietPlan>
                {
                    new DietPlan
                    {
                        DietPlanId = 1,
                        Name = "Weight Loss Plan",
                        BodyType = "Ectomorph",
                        Meals = new List<Meal>
                        {
                            meals[0], // Oatmeal with Berries (Breakfast)
                            meals[1], // Grilled Chicken Salad (Lunch)
                            meals[3]  // Greek Yogurt with Almonds (Snack)
                        }
                    },
                    new DietPlan
                    {
                        DietPlanId = 2,
                        Name = "Muscle Gain Plan",
                        BodyType = "Mesomorph",
                        Meals = new List<Meal>
                        {
                            meals[1], // Grilled Chicken Salad (Lunch)
                            meals[2], // Salmon with Quinoa (Dinner)
                            meals[3]  // Greek Yogurt with Almonds (Snack)
                        }
                    },
                    new DietPlan
                    {
                        DietPlanId = 3,
                        Name = "Balanced Nutrition Plan",
                        BodyType = "Endomorph",
                        Meals = new List<Meal>
                        {
                            meals[0], // Oatmeal with Berries (Breakfast)
                            meals[1], // Grilled Chicken Salad (Lunch)
                            meals[2]  // Salmon with Quinoa (Dinner)
                        }
                    }
                };

                DietPlans.AddRange(dietPlans);
            }

            // Check if there are any Training Programs and Exercises
            if (!TrainingPrograms.Any() && !Exercises.Any())
            {
                // Seed Exercises
                var exercises = new List<Exercise>
                {
                    new Exercise { ExerciseId = 1, Name = "Bench Press", RepsRange = "8-12 reps", MuscleGroupTarget = "Chest", PicUrl = "https://cdn.shopify.com/s/files/1/1497/9682/files/2_3b458939-656d-42a7-ab89-3f827f6b9a83.jpg?v=1648826582" },
                    new Exercise { ExerciseId = 2, Name = "Overhead Press", RepsRange = "8-12 reps", MuscleGroupTarget = "Shoulders", PicUrl = "https://www.lyfta.app/_next/image?url=%2Fthumbnails%2F04261201.jpg&w=3840&q=20" },
                    new Exercise { ExerciseId = 3, Name = "Tricep Dips", RepsRange = "8-12 reps", MuscleGroupTarget = "Triceps", PicUrl="https://cdn.shopify.com/s/files/1/1497/9682/files/Benefits_of_Mastering_Tricep_Dips.jpg?v=1687254157&width=750" },
                    new Exercise { ExerciseId = 4, Name = "Pull-ups", RepsRange = "8-12 reps", MuscleGroupTarget = "Back", PicUrl="https://www.burnthefatinnercircle.com/members/images/1930.png?cb=20240911131701" },
                    new Exercise { ExerciseId = 5, Name = "Barbell Row", RepsRange = "8-12 reps", MuscleGroupTarget = "Back", PicUrl="https://blog.myarsenalstrength.com/hs-fs/hubfs/Bent%20over%20row%20exercise.png" },
                    new Exercise { ExerciseId = 6, Name = "Bicep Curls", RepsRange = "8-12 reps", MuscleGroupTarget = "Biceps", PicUrl="https://www.endomondo.com/wp-content/uploads/2024/08/Cable-Bicep-Curl-Guide.png" },
                    new Exercise { ExerciseId = 7, Name = "Squats", RepsRange = "8-12 reps", MuscleGroupTarget = "Legs", PicUrl="https://static.strengthlevel.com/images/exercises/squat/squat-800.jpg" },
                    new Exercise { ExerciseId = 8, Name = "Deadlifts", RepsRange = "8-12 reps", MuscleGroupTarget = "Legs", PicUrl="https://www.inspireusafoundation.org/wp-content/uploads/2022/05/sumo-deadlift-form.gif" },
                    new Exercise { ExerciseId = 9, Name = "Leg Press", RepsRange = "8-12 reps", MuscleGroupTarget = "Legs", PicUrl="https://www.inspireusafoundation.org/wp-content/uploads/2022/03/single-leg-leg-press.gif" },
                    new Exercise { ExerciseId = 10, Name = "Lunges", RepsRange = "8-12 reps", MuscleGroupTarget = "Legs", PicUrl="https://homeworkouts.org/wp-content/uploads/anim-forward-lunges.gif"},
                    new Exercise { ExerciseId = 11, Name = "Leg Curls", RepsRange = "8-12 reps", MuscleGroupTarget = "Hamstrings", PicUrl="https://www.lyfta.app/thumbnails/05991201.jpg" },
                    new Exercise { ExerciseId = 12, Name = "Calf Raises", RepsRange = "15-20 reps", MuscleGroupTarget = "Calves", PicUrl="https://www.lyfta.app/_next/image?url=https%3A%2F%2Flyfta.app%2Fimages%2Fexercises%2F01111101.png&w=3840&q=20" }
                };
                        Exercises.AddRange(exercises);

                // Seed Training Programs
                var trainingPrograms = new List<TrainingProgram>
                {
                    new TrainingProgram { TrainingProgramId = 1, Title = "Push Program", Exercises = new List<Exercise>{ exercises[0], exercises[1], exercises[2] } },
                    new TrainingProgram { TrainingProgramId = 2, Title = "Pull Program", Exercises = new List<Exercise>{ exercises[3], exercises[4], exercises[5] } },
                    new TrainingProgram { TrainingProgramId = 3, Title = "Legs Program", Exercises = new List<Exercise>{ exercises[6], exercises[7], exercises[8], exercises[9], exercises[10], exercises[11] } }
                };
                
                TrainingPrograms.AddRange(trainingPrograms);

            }
            // Save changes to the database
            SaveChanges();
        }


    }
}
