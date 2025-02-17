// Purpose: Implementation of IStudentExportService interface. This class is responsible for exporting students to Excel and JSON file format.
using BE.Interface;
using ClosedXML.Excel;

namespace BE.Services
{
    public class StudentExportService : IStudentExportService
    {
        private readonly IStudentRepository _studentRepo;

        public StudentExportService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public async Task<byte[]> ExportStudentsToExcelAsync()
        {
            // Lấy danh sách sinh viên (StudentDto) từ repository
            var students = await _studentRepo.GetAllAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Students");

                // Tạo header
                worksheet.Cell(1, 1).Value = "StudentId";
                worksheet.Cell(1, 2).Value = "FullName";
                worksheet.Cell(1, 3).Value = "DateOfBirth";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Faculty";
                worksheet.Cell(1, 6).Value = "Course";
                worksheet.Cell(1, 7).Value = "Program";
                worksheet.Cell(1, 8).Value = "Address";
                worksheet.Cell(1, 9).Value = "Email";
                worksheet.Cell(1, 10).Value = "PhoneNumber";
                worksheet.Cell(1, 11).Value = "Status";

                int row = 2;
                foreach (var student in students)
                {
                    worksheet.Cell(row, 1).Value = student.StudentId;
                    worksheet.Cell(row, 2).Value = student.FullName;
                    worksheet.Cell(row, 3).Value = student.DateOfBirth.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 4).Value = student.Gender.ToString();
                    worksheet.Cell(row, 5).Value = student.Faculty?.Name;
                    worksheet.Cell(row, 6).Value = student.Course;
                    worksheet.Cell(row, 7).Value = student.Program?.Name;
                    worksheet.Cell(row, 8).Value = student.Address;
                    worksheet.Cell(row, 9).Value = student.Email;
                    worksheet.Cell(row, 10).Value = student.PhoneNumber;
                    worksheet.Cell(row, 11).Value = student.Status?.Name;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<byte[]> ExportStudentsToJsonAsync()
        {
            var students = await _studentRepo.GetAllAsync();
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(students, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            return System.Text.Encoding.UTF8.GetBytes(jsonContent);
        }
    }
}
