﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationPrograms",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationPrograms", x => x.ProgramId);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyId);
                });

            migrationBuilder.CreateTable(
                name: "StudentStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStatuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    FacultyId = table.Column<int>(type: "integer", nullable: false),
                    Course = table.Column<int>(type: "integer", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_ApplicationPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "ApplicationPrograms",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_StudentStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StudentStatuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationPrograms",
                columns: new[] { "ProgramId", "Name" },
                values: new object[,]
                {
                    { 1, "Chất lượng cao" },
                    { 2, "Tiên tiến" },
                    { 3, "Đại trà" }
                });

            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "FacultyId", "Name" },
                values: new object[,]
                {
                    { 1, "Khoa Luật" },
                    { 2, "Khoa Tiếng Anh thương mại" },
                    { 3, "Khoa Tiếng Nhật" },
                    { 4, "Khoa Tiếng Pháp" }
                });

            migrationBuilder.InsertData(
                table: "StudentStatuses",
                columns: new[] { "StatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Đang học" },
                    { 2, "Đã tốt nghiệp" },
                    { 3, "Đã thôi học" },
                    { 4, "Tạm dừng học" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Address", "Course", "DateOfBirth", "Email", "FacultyId", "FullName", "Gender", "PhoneNumber", "ProgramId", "StatusId" },
                values: new object[] { "22127389", "Bình Chánh", 2022, new DateTime(2004, 1, 15, 17, 0, 0, 0, DateTimeKind.Utc), "thanh1612004@gmail.com", 1, "Nguyễn Phúc Thành", 1, "0915361073", 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Students_FacultyId",
                table: "Students",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProgramId",
                table: "Students",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StatusId",
                table: "Students",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "ApplicationPrograms");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "StudentStatuses");
        }
    }
}
