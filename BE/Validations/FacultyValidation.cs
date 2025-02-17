using System;
using System.ComponentModel.DataAnnotations;

namespace BE.Validations;

  public class FacultyValidation : ValidationAttribute
    {
        private static readonly string[] ValidFaculties = { "Khoa Luật", "Khoa Tiếng Anh thương mại", "Khoa Tiếng Nhật", "Khoa Tiếng Pháp" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !ValidFaculties.Contains(value.ToString()))
            {
                return new ValidationResult($"Khoa không hợp lệ. Các khoa hợp lệ là: {string.Join(", ", ValidFaculties)}");
            }
            return ValidationResult.Success;
        }
    }