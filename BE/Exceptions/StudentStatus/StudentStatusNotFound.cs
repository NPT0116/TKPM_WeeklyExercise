using System;

namespace BE.Exceptions.StudentStatus;

public class StudentStatusNotFound: BaseException
{
    public StudentStatusNotFound(int studentStatusId)
        : base($"Student status with ID {studentStatusId} not found.")
    {
    }
}
