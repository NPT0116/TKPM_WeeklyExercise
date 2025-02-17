using System;
using System.ComponentModel.DataAnnotations;

namespace BE.Models;

public class ApplicationProgram
{
        [Key]
        public int ProgramId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
}
