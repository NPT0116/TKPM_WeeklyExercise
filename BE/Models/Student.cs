using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BE.Enums;
using BE.Validations;

namespace BE.Models;

  public class Student
    {
        [Key]
        [Required]
        public string StudentId { get; set; }  // MSSV

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MinLength(3, ErrorMessage = "Họ tên phải có ít nhất 3 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Ngày sinh không hợp lệ")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Nam|Nữ|Khác)$", ErrorMessage = "Giới tính chỉ có thể là Nam, Nữ hoặc Khác")]
        public Gender Gender { get; set; }

        [Required]
        public int FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty Faculty { get; set; }

        [Required]
        public int Course { get; set; }

        [Required]
        public int ProgramId { get; set; }
        [ForeignKey("ProgramId")]
        public ApplicationProgram Program { get; set; }

        public string Address { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(0[2-9][0-9]{8,9})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public StudentStatus Status { get; set; }
    }