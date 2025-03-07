// src/components/BusinessRuleSettingComponent.tsx
import React, { useState, useEffect } from "react";

export interface BusinessRulesSettings {
  enableEmailValidation: boolean;
  enablePhoneValidation: boolean;
  enableStudentStatusTransition: boolean;
  enableStudentDeletion: boolean;
}

const BusinessRuleSettingComponent: React.FC = () => {
  const [settings, setSettings] = useState<BusinessRulesSettings>({
    enableEmailValidation: true,
    enablePhoneValidation: true,
    enableStudentStatusTransition: true,
    enableStudentDeletion: true,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  // Fetch current settings from the API on component mount.
  useEffect(() => {
    const fetchSettings = async () => {
      try {
        const response = await fetch(
          "http://localhost:5230/api/config/businessrules"
        );
        if (!response.ok) {
          throw new Error("Failed to load business rules");
        }
        const data: BusinessRulesSettings = await response.json();
        setSettings(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Unknown error");
      }
    };

    fetchSettings();
  }, []);

  // Handle checkbox changes.
  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    setSettings((prev) => ({ ...prev, [name]: checked }));
  };

  // Handle form submission to update settings.
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccess(null);

    try {
      const response = await fetch(
        "http://localhost:5230/api/config/businessrules",
        {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(settings),
        }
      );
      if (!response.ok) {
        throw new Error("Failed to update business rules");
      }
      const updatedSettings: BusinessRulesSettings = await response.json();
      setSettings(updatedSettings);
      setSuccess("Business rules updated successfully");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unknown error");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="business-rule-setting-component">
      <h2>Business Rule Settings</h2>
      {error && <div className="error-message">{error}</div>}
      {success && <div className="success-message">{success}</div>}
      <form onSubmit={handleSubmit}>
        <div>
          <input
            type="checkbox"
            id="enableEmailValidation"
            name="enableEmailValidation"
            checked={settings.enableEmailValidation}
            onChange={handleCheckboxChange}
          />
          <label htmlFor="enableEmailValidation">Enable Email Validation</label>
        </div>
        <div>
          <input
            type="checkbox"
            id="enablePhoneValidation"
            name="enablePhoneValidation"
            checked={settings.enablePhoneValidation}
            onChange={handleCheckboxChange}
          />
          <label htmlFor="enablePhoneValidation">Enable Phone Validation</label>
        </div>
        <div>
          <input
            type="checkbox"
            id="enableStudentStatusTransition"
            name="enableStudentStatusTransition"
            checked={settings.enableStudentStatusTransition}
            onChange={handleCheckboxChange}
          />
          <label htmlFor="enableStudentStatusTransition">
            Enable Student Status Transition
          </label>
        </div>
        <div>
          <input
            type="checkbox"
            id="enableStudentDeletion"
            name="enableStudentDeletion"
            checked={settings.enableStudentDeletion}
            onChange={handleCheckboxChange}
          />
          <label htmlFor="enableStudentDeletion">
            Enable Student Deletion (Time Limit)
          </label>
        </div>
        <button type="submit" disabled={loading}>
          {loading ? "Saving..." : "Save Settings"}
        </button>
      </form>
    </div>
  );
};

export default BusinessRuleSettingComponent;
