// ErrorPopup.tsx
import React from "react";

interface ErrorPopupProps {
  message: string;
  onClose: () => void;
}

const ErrorPopup: React.FC<ErrorPopupProps> = ({ message, onClose }) => {
  return (
    <div className="error-popup-overlay">
      <div className="error-popup">
        <div className="error-popup-header">
          <h4>Error</h4>
          <button onClick={onClose} className="close-btn">
            ×
          </button>
        </div>
        <div className="error-popup-body">
          <p>{message}</p>
        </div>
      </div>
    </div>
  );
};

export default ErrorPopup;
