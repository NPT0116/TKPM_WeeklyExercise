using System;

namespace BE.Exceptions.ApplicationProgram;

public class ApplicationProgramNotFound : BaseException
{
    public ApplicationProgramNotFound(int applicationProgramId)
        : base($"Application program with ID {applicationProgramId} not found.")
    {
    }
}
