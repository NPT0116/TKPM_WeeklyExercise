using System;
using BE.Enums;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Data;



public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }

    public DbSet<Faculty> Faculties { get; set; }

    public DbSet<StudentStatus> StudentStatuses { get; set; }

    public DbSet<ApplicationProgram> ApplicationPrograms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed dữ liệu mặc định cho Faculty (Khoa)
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty { FacultyId = 1, Name = "Khoa Luật" },
                new Faculty { FacultyId = 2, Name = "Khoa Tiếng Anh thương mại" },
                new Faculty { FacultyId = 3, Name = "Khoa Tiếng Nhật" },
                new Faculty { FacultyId = 4, Name = "Khoa Tiếng Pháp" }
            );

            // Seed dữ liệu mặc định cho StudentStatus (Tình trạng sinh viên)
            modelBuilder.Entity<StudentStatus>().HasData(
                new StudentStatus { StatusId = 1, Name = "Đang học" },
                new StudentStatus { StatusId = 2, Name = "Đã tốt nghiệp" },
                new StudentStatus { StatusId = 3, Name = "Đã thôi học" },
                new StudentStatus { StatusId = 4, Name = "Tạm dừng học" }
            );

            // Seed dữ liệu mặc định cho ApplicationProgram (Chương trình đào tạo)
            modelBuilder.Entity<ApplicationProgram>().HasData(
                new ApplicationProgram { ProgramId = 1, Name = "Chất lượng cao" },
                new ApplicationProgram { ProgramId = 2, Name = "Tiên tiến" },
                new ApplicationProgram { ProgramId = 3, Name = "Đại trà" }
            );

            // Seed dữ liệu mẫu cho Student
            modelBuilder.Entity<Student>().HasData(
                new Student 
                { 
                    StudentId = "22127389",
                    FullName = "Nguyễn Phúc Thành",
                    DateOfBirth = new DateTime(2004, 1, 16).ToUniversalTime(),
                    Gender = Gender.Male,
                    FacultyId = 1,
                    Course = 2022,
                    ProgramId = 1,
                    Address = "Bình Chánh",
                    Email = "thanh1612004@gmail.com",
                    PhoneNumber = "0915361073",
                    StatusId = 1
                }
            );
        }
}

