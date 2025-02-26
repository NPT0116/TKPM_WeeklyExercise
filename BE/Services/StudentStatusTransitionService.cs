using System;
using BE.Config;
using BE.Interface;
using Microsoft.Extensions.Options;

namespace BE.Services;

public class StudentStatusTransitionService : IStudentStatusTransitionService
{
 private readonly StudentStatusTransitionConfig _settings;

        public StudentStatusTransitionService(IOptions<StudentStatusTransitionConfig> options)
        {
            _settings = options.Value;
        }

        public bool IsValidTransition(int currentStatusId, int newStatusId)
        {
            if (_settings.AllowedTransitions.TryGetValue(currentStatusId, out List<int> allowed))
            {
                return allowed.Contains(newStatusId);
            }
            // Nếu không có cấu hình cho trạng thái hiện tại thì không cho phép chuyển đổi.
            return false;
        }
}
