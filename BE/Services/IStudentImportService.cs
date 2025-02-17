using System;

namespace BE.Services;

public interface IStudentImportService
{
    Task ImportStudentsFromExcelAsync(IFormFile file);

}
