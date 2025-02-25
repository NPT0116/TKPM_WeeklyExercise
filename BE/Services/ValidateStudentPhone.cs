using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using BE.Config;
using BE.Exceptions.Student;
using BE.Interface;

namespace BE.Utils
{
    public class ValidateStudentPhone : IValidateStudentPhone
    {
        private readonly PhoneSetting _phoneSetting;

        public ValidateStudentPhone(IOptions<PhoneSetting> phoneSetting)
        {
            _phoneSetting = phoneSetting.Value;
        }

        /// <summary>
        /// Kiểm tra xem số điện thoại có khớp với định dạng được cấu hình không.
        /// </summary>
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }
            return Regex.IsMatch(phone, _phoneSetting.AllowedPattern);
        }

        /// <summary>
        /// Kiểm tra số điện thoại và ném ngoại lệ nếu không hợp lệ.
        /// </summary>
        public void EnsureValidPhone(string phone, bool useCustomException = false)
        {
            if (!IsValidPhone(phone))
            {
                if (useCustomException)
                {
                    throw new StudentPhoneFormatError(_phoneSetting.AllowedPattern);
                }
                else
                {
                    throw new ArgumentException($"Số điện thoại không hợp lệ. Định dạng cho phép: {_phoneSetting.AllowedPattern}");
                }
            }
        }

        public string GetAllowedPattern()
        {
            return _phoneSetting.AllowedPattern;
        }
    }
}
