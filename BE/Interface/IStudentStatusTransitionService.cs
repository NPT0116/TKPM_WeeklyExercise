using System;

namespace BE.Interface;

public interface IStudentStatusTransitionService
{
        bool IsValidTransition(int currentStatusId, int newStatusId);

}
