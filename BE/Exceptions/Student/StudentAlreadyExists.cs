using System;

namespace BE.Exceptions.Student;

public class StudentAlreadyExists: BaseException
{
    public StudentAlreadyExists(string studentId) : base($"Student with ID {studentId} already exists.", System.Net.HttpStatusCode.BadRequest)
    {
    }
}
