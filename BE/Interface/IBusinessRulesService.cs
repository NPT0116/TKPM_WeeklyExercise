using System;
using BE.Config;

namespace BE.Interface;

public interface IBusinessRulesService
{
        BusinessRulesSettings GetSettings();
        void UpdateSettings(BusinessRulesSettings newSettings);
}
