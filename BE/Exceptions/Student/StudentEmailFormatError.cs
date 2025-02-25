using System;
using System.Net;

namespace BE.Exceptions.Student;

public class StudentEmailFormatError : BaseException
{
    
    public StudentEmailFormatError(string emailFormat) : base($"Email phải kết thúc bằng {emailFormat}", HttpStatusCode.BadRequest)
    {
    }
}
