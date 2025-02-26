using System;
using System.Text.Json;
using BE.Data;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Seed;

public class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context, IHostEnvironment env, ILogger logger)
        {
            // Thực hiện migration để đảm bảo database đã được tạo và cập nhật schema mới nhất
            await context.Database.MigrateAsync();

            // Kiểm tra nếu đã có dữ liệu seed rồi thì bỏ qua
            if (await context.Faculties.AnyAsync() &&
                await context.StudentStatuses.AnyAsync() &&
                await context.ApplicationPrograms.AnyAsync() &&
                await context.Students.AnyAsync())
            {
                logger.LogInformation("Database already seeded.");
                return;
            }

            // Đọc file seedData.json từ thư mục gốc của ứng dụng
            var seedDataPath = Path.Combine(env.ContentRootPath, "seedData.json");
            if (!File.Exists(seedDataPath))
            {
                logger.LogWarning("Seed data file not found at: {SeedDataPath}", seedDataPath);
                return;
            }

            var jsonData = await File.ReadAllTextAsync(seedDataPath);
            var seedData = JsonSerializer.Deserialize<SeedDataModel>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (seedData != null)
            {
                if (seedData.Faculties != null)
                    context.Faculties.AddRange(seedData.Faculties);

                if (seedData.StudentStatuses != null)
                    context.StudentStatuses.AddRange(seedData.StudentStatuses);

                if (seedData.ApplicationPrograms != null)
                    context.ApplicationPrograms.AddRange(seedData.ApplicationPrograms);

                if (seedData.Students != null)
                    context.Students.AddRange(seedData.Students);

                await context.SaveChangesAsync();
                logger.LogInformation("Database seeded successfully.");
            }
        }
    }

    // Lớp ánh xạ dữ liệu seed từ file JSON
    public class SeedDataModel
    {
        public List<Faculty> Faculties { get; set; }
        public List<StudentStatus> StudentStatuses { get; set; }
        public List<ApplicationProgram> ApplicationPrograms { get; set; }
        public List<Student> Students { get; set; }
    }