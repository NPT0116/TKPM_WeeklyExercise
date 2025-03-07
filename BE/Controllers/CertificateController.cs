using BE.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/certificate")]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        // POST: api/certificate/export
       [HttpPost("export")]
        public async Task<IActionResult> ExportCertificate([FromBody] CertificateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.StudentId) || string.IsNullOrWhiteSpace(request.Purpose))
            {
                return BadRequest("StudentId and Purpose are required.");
            }

            // Generate certificate HTML content.
            var certificateContent = await _certificateService.GenerateCertificateContentAsync(request.StudentId, request.Purpose);

            // Check desired format: pdf or html. Default to html if not specified.
            var format = request.Format?.ToLower() ?? "html";
            if (format == "pdf")
            {
                byte[] pdfBytes = _certificateService.ConvertToPdf(certificateContent);
                return File(pdfBytes, "application/pdf", "certificate.pdf");
            }
            else
            {
                return Content(certificateContent, "text/html");
            }
        }

    }

    public class CertificateRequest
    {
        /// <summary>
        /// Student's MSSV.
        /// </summary>
        public string StudentId { get; set; }
        /// <summary>
        /// Purpose of the certificate.
        /// Options:
        /// - "Xác nhận đang học để vay vốn ngân hàng"
        /// - "Xác nhận làm thủ tục tạm hoãn nghĩa vụ quân sự"
        /// - "Xác nhận làm hồ sơ xin việc / thực tập"
        /// - "Xác nhận lý do khác: [Ghi rõ]"
        /// </summary>
        public string Purpose { get; set; }
        /// <summary>
        /// Desired output format: "pdf", "docx", or "html" (default).
        /// </summary>
        public string Format { get; set; }
    }
}
