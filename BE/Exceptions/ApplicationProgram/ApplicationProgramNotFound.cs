using System;
using System.Net;

namespace BE.Exceptions.ApplicationProgram;

public class ApplicationProgramNotFound : BaseException
{
    public ApplicationProgramNotFound(int applicationProgramId)
        : base($"Application program with ID {applicationProgramId} not found.", HttpStatusCode.BadRequest)
    {
    }
}
