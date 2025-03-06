using System;
using System.Text;
using System.Threading.Tasks;
using BE.Controllers;
using BE.Exceptions.Student;
using BE.Interface;
using DinkToPdf; // You can remove this if you're no longer using DinkToPdf
using SelectPdf;

namespace BE.Services
{
    public class CertificateService : ICertificateService
    {
        // Remove the DinkToPdf converter if not needed
        // private readonly IConverter _converter;
        private readonly IStudentRepository _studentRepository;
        // If you're not using DinkToPdf anymore, you can remove the converter injection
        public CertificateService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            // _converter = converter;
        }

        public async Task<string> GenerateCertificateContentAsync(string studentId, string purpose)
        {
            var studentInfo = await _studentRepository.GetByIdAsync(studentId);
            if (studentInfo == null)
            {
                throw new StudentNotFound(studentId);
            }

            string template = @"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <title>Giấy Xác Nhận Tình Trạng Sinh Viên</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 40px; }}
        .header {{ text-align: center; margin-bottom: 20px; }}
        .content {{ margin: 0 40px; }}
        .signature {{ margin-top: 40px; text-align: right; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>TRƯỜNG ĐẠI HỌC KHOA HỌC TỰ NHIÊN</h1>
        <p>PHÒNG ĐÀO TẠO</p>
        <p>📍 Địa chỉ: [Địa chỉ trường] | 📞 Điện thoại: [Số điện thoại] | 📧 Email: [Email liên hệ]</p>
    </div>
    <h2 style='text-align: center;'>GIẤY XÁC NHẬN TÌNH TRẠNG SINH VIÊN</h2>
    <p>Trường Đại học Khoa Học Tự Nhiên:</p>
    <div class='content'>
        <h3>1. Thông tin sinh viên:</h3>
        <ul>
            <li>Họ và tên: {1}</li>
            <li>Mã số sinh viên: {0}</li>
            <li>Ngày sinh: {2}</li>
            <li>Giới tính: {3}</li>
            <li>Khoa: {4}</li>
            <li>Khóa: {5}</li>
            <li>Chương trình đào tạo: {6}</li>
            <li>Địa chỉ: {7}</li>
            <li>Email: {8}</li>
            <li>Số điện thoại: {9}</li>
            <li>Tình trạng sinh viên: {10}</li>
        </ul>
        <h3>2. Mục đích xác nhận:</h3>
        <p>{11}</p>
    </div>
    <p>Giấy xác nhận có hiệu lực đến ngày: {12}</p>
    <p>📍 Xác nhận của Trường Đại học Khoa Học Tự Nhiên</p>
    <p>📅 Ngày cấp: {12}</p>
    <div class='signature'>
        <p>🖋 Trưởng Phòng Đào Tạo</p>
        <p>(Ký, ghi rõ họ tên, đóng dấu)</p>
    </div>
</body>
</html>";

            string issueDate = DateTime.Now.ToString("dd/MM/yyyy");

            // Render the certificate content using string.Format.
            string renderedContent = string.Format(template,
                studentInfo.StudentId,      // {0}
                studentInfo.FullName,       // {1}
                studentInfo.DateOfBirth,    // {2}
                studentInfo.Gender,         // {3}
                studentInfo.Faculty.Name,        // {4}
                studentInfo.Course,         // {5}
                studentInfo.Program.Name,        // {6}
                studentInfo.Address,        // {7}
                studentInfo.Email,          // {8}
                studentInfo.PhoneNumber,    // {9}
                studentInfo.Status.Name,         // {10}
                purpose,                    // {11}
                issueDate                   // {12}
            );
            return await Task.FromResult(renderedContent);
        }

        public byte[] ConvertToPdf(string certificateContent)
        {
            // Create an instance of the SelectPdf converter
            var converter = new HtmlToPdf();

            // Optional: Set options (e.g., margins)
            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;

            // Convert the HTML string to a PDF document
            PdfDocument doc = converter.ConvertHtmlString(certificateContent);

            // Save the PDF document to a byte array
            byte[] pdfBytes = doc.Save();

            // Close the document to free resources
            doc.Close();

            return pdfBytes;
        }

        public byte[] ConvertToDocx(string certificateContent)
        {
            // For demonstration, this method still returns the HTML content as bytes.
            // In production, you should implement conversion using a library like Open XML SDK or DocX.
            return Encoding.UTF8.GetBytes(certificateContent);
        }
    }
}
