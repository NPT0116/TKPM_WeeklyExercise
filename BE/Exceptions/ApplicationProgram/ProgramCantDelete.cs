using System;

namespace BE.Exceptions.ApplicationProgram;

public class ProgramCantDelete: BaseException
{
    public ProgramCantDelete(int id) : base($"Program with id {id} can't be deleted because it has students")
    {
    }
}

