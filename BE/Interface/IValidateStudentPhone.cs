using System;

namespace BE.Interface
{
    public interface IValidateStudentPhone
    {
        /// <summary>
        /// Kiểm tra xem số điện thoại có hợp lệ theo định dạng cho phép không.
        /// </summary>
        bool IsValidPhone(string phone);

        /// <summary>
        /// Kiểm tra số điện thoại, nếu không hợp lệ sẽ ném ngoại lệ.
        /// </summary>
        /// <param name="phone">Số điện thoại cần kiểm tra</param>
        /// <param name="useCustomException">
        /// Nếu true, ném ngoại lệ StudentPhoneFormatError; nếu false, ném ArgumentException.
        /// </param>
        void EnsureValidPhone(string phone, bool useCustomException = false);

        /// <summary>
        /// Lấy định dạng hợp lệ (regex pattern) được cấu hình.
        /// </summary>
        string GetAllowedPattern();
    }
}
