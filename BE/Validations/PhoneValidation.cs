using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BE.Validations;

    public class PhoneValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Số điện thoại không được để trống");

            string phoneNumber = value.ToString();
            string pattern = @"^(0[2-9][0-9]{8,9})$"; // Định dạng số điện thoại Việt Nam

            if (!Regex.IsMatch(phoneNumber, pattern))
            {
                return new ValidationResult("Số điện thoại không hợp lệ. Định dạng hợp lệ: 0xxxxxxxxx hoặc 0xxxxxxxxxx");
            }
            return ValidationResult.Success;
        }
    }