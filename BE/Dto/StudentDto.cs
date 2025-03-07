using System;
using BE.Enums;

namespace BE.Dto;

public class StudentCreateDto
{
    public string StudentId { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public int FacultyId { get; set; }
    public int Course { get; set; }
    public int ProgramId { get; set; }
    public string Address { get; set; } = "";
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int StatusId { get; set; }
}

public class StudentUpdateDto
{
    public string StudentId { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public int FacultyId { get; set; }
    public int Course { get; set; }
    public int ProgramId { get; set; }
    public string Address { get; set; } = "";
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int StatusId { get; set; }
}

public class StudentDto
{
    public string StudentId { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public facultyDto Faculty { get; set; }
    public int Course { get; set; }
    public applicationProgramDto Program { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public studentStatusDto Status { get; set; }
    public DateTime CreatedAt { get; set; }
}