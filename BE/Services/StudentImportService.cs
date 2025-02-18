using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dto;
using BE.Interface;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

namespace BE.Services
{
    public class StudentImportService : IStudentImportService
    {
        private readonly IStudentRepository _studentRepo;

        public StudentImportService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public async Task ImportStudentsFromExcelAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Invalid file format. Please upload an Excel file (.xlsx).");

            var importedStudents = new List<StudentCreateDto>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (var workbook = new XLWorkbook(stream))
                {
                    // Try to get the worksheet named "Students". If not found, use the first worksheet.
                    IXLWorksheet worksheet;
                    if (workbook.Worksheets.Any(ws => ws.Name.Equals("Students", StringComparison.OrdinalIgnoreCase)))
                    {
                        worksheet = workbook.Worksheet("Students");
                    }
                    else
                    {
                        worksheet = workbook.Worksheets.First();
                    }

                    // Assuming the first row is header, data starts from row 2.
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                    foreach (var row in rows)
                    {
                        // Skip entirely empty rows (optional).
                        // If there's no data at all in the row, continue.
                        if (row.Cells().All(c => string.IsNullOrWhiteSpace(c.GetString())))
                            continue;

                        // 1) StudentId
                        var studentId = row.Cell(1).GetString()?.Trim();
                        if (string.IsNullOrWhiteSpace(studentId))
                            throw new FormatException($"StudentId is empty in row {row.RowNumber()}.");

                        // 2) FullName
                        var fullName = row.Cell(2).GetString()?.Trim();
                        if (string.IsNullOrWhiteSpace(fullName))
                            throw new FormatException($"FullName is empty in row {row.RowNumber()}.");

                        // 3) DateOfBirth
                        var dateString = row.Cell(3).GetString()?.Trim();
                        if (string.IsNullOrWhiteSpace(dateString) ||
                            !DateTime.TryParse(dateString, out var parsedDate))
                        {
                            throw new FormatException($"Invalid or empty DateOfBirth in row {row.RowNumber()}. Value: '{dateString}'");
                        }
                        // Mark as UTC if your DB requires it
                        parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);

                        // 4) Gender
                        var genderString = row.Cell(4).GetString()?.Trim();
                        var gender = genderString switch
                        {
                            "Nam" => BE.Enums.Gender.Male,
                            "Nữ"  => BE.Enums.Gender.Female,
                            _     => BE.Enums.Gender.Other
                        };

                        // 5) FacultyId
                        var facultyString = row.Cell(5).GetString()?.Trim();
                        if (!int.TryParse(facultyString, out var facultyId))
                        {
                            throw new FormatException($"Invalid or empty FacultyId in row {row.RowNumber()}. Value: '{facultyString}'");
                        }

                        // 6) Course
                        var courseString = row.Cell(6).GetString()?.Trim();
                        if (!int.TryParse(courseString, out var course))
                        {
                            throw new FormatException($"Invalid or empty Course in row {row.RowNumber()}. Value: '{courseString}'");
                        }

                        // 7) ProgramId
                        var programString = row.Cell(7).GetString()?.Trim();
                        if (!int.TryParse(programString, out var programId))
                        {
                            throw new FormatException($"Invalid or empty ProgramId in row {row.RowNumber()}. Value: '{programString}'");
                        }

                        // 8) Address
                        var address = row.Cell(8).GetString()?.Trim();

                        // 9) Email
                        var email = row.Cell(9).GetString()?.Trim();

                        // 10) PhoneNumber
                        var phoneString = row.Cell(10).GetString()?.Trim();

                        // 11) StatusId
                        var statusString = row.Cell(11).GetString()?.Trim();
                        if (!int.TryParse(statusString, out var statusId))
                        {
                            throw new FormatException($"Invalid or empty StatusId in row {row.RowNumber()}. Value: '{statusString}'");
                        }

                        var student = new StudentCreateDto
                        {
                            StudentId = studentId,
                            FullName = fullName,
                            DateOfBirth = parsedDate,
                            Gender = gender,
                            FacultyId = facultyId,
                            Course = course,
                            ProgramId = programId,
                            Address = address,
                            Email = email,
                            PhoneNumber = phoneString,
                            StatusId = statusId
                        };

                        importedStudents.Add(student);
                    }
                }
            }

            // Create students from the imported list.
            foreach (var student in importedStudents)
            {
                await _studentRepo.CreateAsync(student);
            }
        }
    }
}
