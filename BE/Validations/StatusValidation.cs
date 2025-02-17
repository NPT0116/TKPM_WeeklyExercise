using System;
using System.ComponentModel.DataAnnotations;

namespace BE.Validations;

    public class StatusValidation : ValidationAttribute
    {
        private static readonly string[] ValidStatuses = { "Đang học", "Đã tốt nghiệp", "Đã thôi học", "Tạm dừng học" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !ValidStatuses.Contains(value.ToString()))
            {
                return new ValidationResult($"Tình trạng sinh viên không hợp lệ. Giá trị hợp lệ: {string.Join(", ", ValidStatuses)}");
            }
            return ValidationResult.Success;
        }
    }