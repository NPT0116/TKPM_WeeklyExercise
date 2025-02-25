using System;

namespace BE.Interface;

public interface IValidateStudentEmail
{
    bool IsValidEmail(string email);
    string GetAllowedDomain();
}
