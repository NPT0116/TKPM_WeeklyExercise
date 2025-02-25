using System;
using System.Net;

namespace BE.Exceptions.Student;

public class StudentNotFound : BaseException
{
    public StudentNotFound(string StudentId) : base($"StudentId not found: {StudentId}", HttpStatusCode.NotFound)
    {
    }
}
