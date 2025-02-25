using System;
using BE.Exceptions.Student;
using BE.Config; // Giả sử EmailSetting được định nghĩa trong BE.Config
using Microsoft.Extensions.Options;
using BE.Interface;

public class ValidateStudentEmail: IValidateStudentEmail
{
    private readonly EmailSetting _emailSetting;

    public ValidateStudentEmail(IOptions<EmailSetting> emailSetting)
    {
        _emailSetting = emailSetting.Value;
    }

    /// <summary>
    /// Kiểm tra email có kết thúc bằng tên miền cho phép không.
    /// Hàm này trả về true nếu hợp lệ, false nếu không hợp lệ.
    /// </summary>
    /// <param name="email">Email cần kiểm tra</param>
    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }
        return email.EndsWith(_emailSetting.AllowedDomain, StringComparison.OrdinalIgnoreCase);
    }

    public string GetAllowedDomain()
    {
        return _emailSetting.AllowedDomain;
    }
}
