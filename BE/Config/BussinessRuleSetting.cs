using System;

namespace BE.Config
{
    public class BusinessRulesSettings
    {
        public bool EnableEmailValidation { get; set; }
        public bool EnablePhoneValidation { get; set; }
        public bool EnableStudentStatusTransition { get; set; }
        public bool EnableStudentDeletion { get; set; }
    }
}
