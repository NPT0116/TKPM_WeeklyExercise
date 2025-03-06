using System;
using System.Net;

namespace BE.Exceptions.Faculty;

public class FacultyCantDelete: BaseException
{
    public FacultyCantDelete(int id) : base($"Faculty with id {id} can't be deleted because it has students", HttpStatusCode.BadRequest)
    {
    }
}
