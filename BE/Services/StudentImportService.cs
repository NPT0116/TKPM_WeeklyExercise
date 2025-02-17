using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BE.Dto;
using BE.Interface;
using ClosedXML.Excel;

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
                // Convert data from columns (adjust these indices if your template changes)
                var student = new StudentCreateDto
                {
                    StudentId = row.Cell(1).GetString(),
                    FullName = row.Cell(2).GetString(),
                    DateOfBirth = DateTime.SpecifyKind(DateTime.Parse(row.Cell(3).GetString()), DateTimeKind.Utc),
                    Gender = row.Cell(4).GetString() switch
                    {
                        "Nam" => BE.Enums.Gender.Male,
                        "Nữ"  => BE.Enums.Gender.Female,
                        _     => BE.Enums.Gender.Other,
                    },
                    // For FacultyId, ProgramId, and StatusId, adjust if your Excel contains names instead of IDs.
                    FacultyId = int.Parse(row.Cell(5).GetString()),
                    Course = int.Parse(row.Cell(6).GetString()),
                    ProgramId = int.Parse(row.Cell(7).GetString()),
                    Address = row.Cell(8).GetString(),
                    Email = row.Cell(9).GetString(),
                    PhoneNumber = row.Cell(10).GetString(),
                    StatusId = int.Parse(row.Cell(11).GetString())
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
