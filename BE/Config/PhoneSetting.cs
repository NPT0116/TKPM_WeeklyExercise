using System;

namespace BE.Config;

  public class PhoneSetting
    {
        // Ví dụ: Cho Việt Nam, số điện thoại hợp lệ là:
        // - Bắt đầu bằng +84 theo sau là [3,5,7,8,9] và 8 chữ số, hoặc
        // - Bắt đầu bằng 0 theo sau là [3,5,7,8,9] và 8 chữ số.
        public string AllowedPattern { get; set; } = @"^(?:\+84[35789]\d{8}|0[35789]\d{8})$";
    }