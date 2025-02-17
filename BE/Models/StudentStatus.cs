using System;
using System.ComponentModel.DataAnnotations;

namespace BE.Models;

public class StudentStatus
{
    [Key]
    [Required]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Tên tình trạng không được để trống")]
    [MinLength(3, ErrorMessage = "Tên tình trạng phải có ít nhất 3 ký tự")]
    public string Name { get; set; } = string.Empty;
}
