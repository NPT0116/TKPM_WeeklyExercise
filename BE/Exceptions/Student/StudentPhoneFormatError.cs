using System;
using System.Net;

namespace BE.Exceptions.Student;

public class StudentPhoneFormatError: BaseException
{
    public StudentPhoneFormatError(string phoneFormat) : base($"Phone format is not valid. Allowed format: {phoneFormat}", HttpStatusCode.BadRequest)
    {
    }
}
