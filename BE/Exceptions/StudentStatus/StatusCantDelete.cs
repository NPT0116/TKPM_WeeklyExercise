using System;
using System.Net;

namespace BE.Exceptions.StudentStatus;

public class StatusCantDelete: BaseException
{
    public StatusCantDelete(int id) : base($"Status with id {id} can't be deleted because it has students", HttpStatusCode.BadRequest)
    {
    }
}
