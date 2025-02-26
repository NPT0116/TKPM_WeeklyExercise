using System;
using System.Net;

namespace BE.Exceptions.StudentStatus;

public class StudentStatusTransisentError : BaseException
{
    public StudentStatusTransisentError(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
}
