using System;

namespace BE.Services;

    public interface IStudentExportService
    {
        Task<byte[]> ExportStudentsToExcelAsync();
        Task<byte[]> ExportStudentsToJsonAsync();
    }