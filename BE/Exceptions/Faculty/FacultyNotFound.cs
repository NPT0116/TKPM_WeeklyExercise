using System;
using DocumentFormat.OpenXml.Math;

namespace BE.Exceptions.Faculty;

public class FacultyNotFound : BaseException
{
    public FacultyNotFound(int facultyId)
        : base($"Faculty with ID {facultyId} not found.")
    {
    }
}
