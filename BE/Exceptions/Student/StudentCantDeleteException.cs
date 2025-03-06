using System;
using System.Net;

namespace BE.Exceptions.Student;

public class StudentCantDeleteException : BaseException
{
    public StudentCantDeleteException(string message) : base(message, HttpStatusCode.BadRequest)
    {

    }
}
