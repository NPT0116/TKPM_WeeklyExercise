// src/components/ImportExportPanel.tsx
import React, { useState, useEffect } from "react";

interface AppInfo {
  version: string;
  buildDate: string;
}

interface ImportExportPanelProps {
  onImportSuccess: () => void;
}

const ImportExportPanel: React.FC<ImportExportPanelProps> = ({
  onImportSuccess,
}) => {
  const [appInfo, setAppInfo] = useState<AppInfo | null>(null);
  const [importFile, setImportFile] = useState<File | null>(null);
  const [uploadMessage, setUploadMessage] = useState<string>("");

  // Lấy thông tin version và build date từ backend
  useEffect(() => {
    fetch("http://localhost:5230/api/AppInfo/version")
      .then((res) => res.json())
      .then((data) => setAppInfo(data))
      .catch((err) => {
        console.error("Không lấy được thông tin phiên bản", err);
      });
  }, []);

  // Xuất dữ liệu ra file JSON
  const handleExportJSON = async () => {
    try {
      const response = await fetch(
        "http://localhost:5230/api/students/export/json"
      );
      if (response.ok) {
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.download = "students.json";
        document.body.appendChild(link);
        link.click();
        link.remove();
      } else {
        alert("Xuất JSON thất bại.");
      }
    } catch (err) {
      console.error("Lỗi xuất JSON", err);
      alert("Lỗi xuất JSON");
    }
  };

  // Xuất dữ liệu ra file Excel
  const handleExportExcel = async () => {
    try {
      const response = await fetch(
        "http://localhost:5230/api/students/export/excel"
      );
      if (response.ok) {
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.download = "students.xlsx";
        document.body.appendChild(link);
        link.click();
        link.remove();
      } else {
        alert("Xuất Excel thất bại.");
      }
    } catch (err) {
      console.error("Lỗi xuất Excel", err);
      alert("Lỗi xuất Excel");
    }
  };

  // Xử lý chọn file khi import
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      setImportFile(e.target.files[0]);
    }
  };

  // Xử lý upload file Excel
  const handleImport = async () => {
    if (!importFile) {
      alert("Vui lòng chọn file trước.");
      return;
    }
    const formData = new FormData();
    formData.append("file", importFile);

    try {
      const response = await fetch(
        "http://localhost:5230/api/students/import/excel",
        {
          method: "POST",
          body: formData,
        }
      );
      if (response.ok) {
        setUploadMessage("Import thành công!");
        onImportSuccess();
      } else {
        setUploadMessage("Import thất bại.");
      }
    } catch (err) {
      console.error("Lỗi import", err);
      setUploadMessage("Lỗi import.");
    }
  };

  return (
    <div className="import-export-panel">
      <h2>Chức năng Import/Export dữ liệu</h2>
      <div className="export-section">
        <button onClick={handleExportJSON}>Xuất dữ liệu JSON</button>
        <button onClick={handleExportExcel}>Xuất dữ liệu Excel</button>
      </div>
      <div className="import-section">
        <h3>Nhập dữ liệu từ Excel</h3>
        <input type="file" accept=".xlsx" onChange={handleFileChange} />
        <button onClick={handleImport}>Upload File</button>
        {uploadMessage && <p>{uploadMessage}</p>}
      </div>
      {appInfo && (
        <div className="app-info">
          <p>
            <strong>Version:</strong> {appInfo.version}
          </p>
          <p>
            <strong>Build Date:</strong> {appInfo.buildDate}
          </p>
        </div>
      )}
    </div>
  );
};

export default ImportExportPanel;
