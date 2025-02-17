using System;
using System.ComponentModel.DataAnnotations;

namespace BE.Models;

public class Faculty
{
    [Key]
    [Required]
    public int FacultyId { get; set; } 

    [Required(ErrorMessage = "Tên khoa không được để trống")]
    [MinLength(3, ErrorMessage = "Tên khoa phải có ít nhất 3 ký tự")]
    public string Name { get; set; } = string.Empty;
}
