// src/components/ImportExportPanel.tsx
import React from "react";
import * as XLSX from "xlsx";
import { Student } from "../interface";

interface ImportExportPanelProps {
  students: Student[];
  onImport: (students: Student[]) => void;
}

const ImportExportPanel: React.FC<ImportExportPanelProps> = ({
  students,
  onImport,
}) => {
  // Export danh sách sinh viên sang file JSON
  const exportToJSON = () => {
    const dataStr = JSON.stringify(students, null, 2);
    const blob = new Blob([dataStr], { type: "application/json" });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = "students.json";
    link.click();
    URL.revokeObjectURL(url);
  };

  // Export danh sách sinh viên sang file Excel
  const exportToExcel = () => {
    // Chuyển đổi dữ liệu thành dạng object (định dạng theo ý bạn)
    const wsData = students.map((s) => ({
      StudentId: s.studentId,
      FullName: s.fullName,
      DateOfBirth: s.dateOfBirth, // Bạn có thể format lại nếu cần
      Gender: s.gender, // Giả sử là "Nam"/"Nữ" hay số, tùy theo định dạng của bạn
      Faculty: s.faculty?.name,
      Course: s.course,
      Program: s.program?.name,
      Address: s.address,
      Email: s.email,
      PhoneNumber: s.phoneNumber,
      Status: s.status?.name,
    }));
    const ws = XLSX.utils.json_to_sheet(wsData);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Students");
    XLSX.writeFile(wb, "students.xlsx");
  };

  // Import dữ liệu từ file JSON
  const importFromJSON = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (evt) => {
        try {
          const text = evt.target?.result;
          if (typeof text === "string") {
            const data = JSON.parse(text);
            // Giả sử file JSON chứa mảng các đối tượng sinh viên theo định dạng bạn quy định
            onImport(data);
          }
        } catch (error) {
          console.error("Lỗi import JSON", error);
        }
      };
      reader.readAsText(file);
    }
  };

  // Import dữ liệu từ file Excel
  const importFromExcel = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (evt) => {
        try {
          const data = evt.target?.result;
          if (data) {
            const workbook = XLSX.read(data, { type: "binary" });
            const sheetName = workbook.SheetNames[0];
            const worksheet = workbook.Sheets[sheetName];
            // Chuyển đổi worksheet thành mảng đối tượng
            const jsonData: any[] = XLSX.utils.sheet_to_json(worksheet, {
              header: 1,
            });
            // Giả sử hàng đầu tiên là header
            const headers = jsonData[0] as string[];
            const importedStudents = jsonData.slice(1).map((row) => {
              let obj: any = {};
              headers.forEach((header, index) => {
                obj[header] = row[index];
              });
              return obj;
            });
            onImport(importedStudents);
          }
        } catch (error) {
          console.error("Lỗi import Excel", error);
        }
      };
      reader.readAsBinaryString(file);
    }
  };

  return (
    <div className="import-export-panel">
      <h3>Import/Export Dữ liệu</h3>
      <div className="export-section">
        <button onClick={exportToJSON}>Export JSON</button>
        <button onClick={exportToExcel}>Export Excel</button>
      </div>
      <div className="import-section">
        <div>
          <label htmlFor="jsonImport">Import JSON:</label>
          <input
            id="jsonImport"
            type="file"
            accept=".json"
            onChange={importFromJSON}
          />
        </div>
        <div>
          <label htmlFor="excelImport">Import Excel:</label>
          <input
            id="excelImport"
            type="file"
            accept=".xlsx, .xls"
            onChange={importFromExcel}
          />
        </div>
      </div>
    </div>
  );
};

export default ImportExportPanel;
