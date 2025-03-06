using System;

namespace BE.Interface;

 public interface ICertificateService
    {
        /// <summary>
        /// Generate certificate content using a template.
        /// </summary>
        /// <param name="studentId">The student's MSSV.</param>
        /// <param name="purpose">The purpose of the certificate.</param>
        /// <returns>Certificate content as a string (HTML/Markdown)</returns>
        Task<string> GenerateCertificateContentAsync(string studentId, string purpose);

        /// <summary>
        /// Convert certificate content (HTML/Markdown) into a PDF document.
        /// </summary>
        byte[] ConvertToPdf(string certificateContent);

        /// <summary>
        /// Convert certificate content (HTML/Markdown) into a DOCX document.
        /// </summary>
        byte[] ConvertToDocx(string certificateContent);
    }