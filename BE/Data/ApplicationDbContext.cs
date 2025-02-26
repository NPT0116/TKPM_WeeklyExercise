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
        }
}

