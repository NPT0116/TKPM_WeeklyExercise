using System;
using BE.Config;
using BE.Interface;

namespace BE.Services;

  public class BusinessRulesService : IBusinessRulesService
    {
        private BusinessRulesSettings _settings;

        public BusinessRulesService()
        {
            // Initialize with all rules enabled by default.
            _settings = new BusinessRulesSettings
            {
                EnableEmailValidation = false,
                EnablePhoneValidation = false,
                EnableStudentStatusTransition = true,
                EnableStudentDeletion = true
            };
        }

        public BusinessRulesSettings GetSettings() => _settings;

        public void UpdateSettings(BusinessRulesSettings newSettings)
        {
            _settings = newSettings;
        }
    }