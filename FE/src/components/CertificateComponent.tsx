// src/components/CertificateComponent.tsx
import React, { useState } from "react";

interface CertificateRequest {
  studentId: string;
  purpose: string;
  format: string; // "pdf" or "html"
}

const CertificateComponent: React.FC = () => {
  const [studentId, setStudentId] = useState("");
  const [purpose, setPurpose] = useState("");
  const [customPurpose, setCustomPurpose] = useState("");
  const [format, setFormat] = useState("pdf");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    // If user selects "Xác nhận lý do khác", append the custom input.
    const finalPurpose =
      purpose === "Xác nhận lý do khác:"
        ? `${purpose} ${customPurpose}`
        : purpose;

    const requestBody: CertificateRequest = {
      studentId,
      purpose: finalPurpose,
      format,
    };

    try {
      const response = await fetch(
        "http://localhost:5230/api/certificate/export",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(requestBody),
        }
      );
      console.log(response);
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Error exporting certificate");
      }

      if (format === "pdf") {
        // If PDF, expect a blob.
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = "certificate.pdf";
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(url);
      } else {
        // For HTML, open in a new window.
        const htmlContent = await response.text();
        const newWindow = window.open();
        if (newWindow) {
          newWindow.document.write(htmlContent);
        }
      }
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Unexpected error occurred"
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="certificate-component">
      <h2>Xuất Giấy Xác Nhận Tình Trạng Sinh Viên</h2>
      {error && <div className="error-message">{error}</div>}
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="studentId">Mã số sinh viên (MSSV): </label>
          <input
            type="text"
            id="studentId"
            value={studentId}
            onChange={(e) => setStudentId(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="purpose">Mục đích xác nhận: </label>
          <select
            id="purpose"
            value={purpose}
            onChange={(e) => setPurpose(e.target.value)}
            required
          >
            <option value="">Chọn mục đích</option>
            <option value="Xác nhận đang học để vay vốn ngân hàng">
              Xác nhận đang học để vay vốn ngân hàng
            </option>
            <option value="Xác nhận làm thủ tục tạm hoãn nghĩa vụ quân sự">
              Xác nhận làm thủ tục tạm hoãn nghĩa vụ quân sự
            </option>
            <option value="Xác nhận làm hồ sơ xin việc / thực tập">
              Xác nhận làm hồ sơ xin việc / thực tập
            </option>
            <option value="Xác nhận lý do khác:">Xác nhận lý do khác:</option>
          </select>
        </div>
        {purpose === "Xác nhận lý do khác:" && (
          <div>
            <label htmlFor="customPurpose">Ghi rõ mục đích: </label>
            <input
              type="text"
              id="customPurpose"
              value={customPurpose}
              onChange={(e) => setCustomPurpose(e.target.value)}
              required
            />
          </div>
        )}
        <div>
          <label htmlFor="format">Loại tài liệu: </label>
          <select
            id="format"
            value={format}
            onChange={(e) => setFormat(e.target.value)}
            required
          >
            <option value="pdf">PDF</option>
            <option value="html">HTML</option>
          </select>
        </div>
        <button type="submit" disabled={loading}>
          {loading ? "Đang xử lý..." : "Xuất giấy"}
        </button>
      </form>
    </div>
  );
};

export default CertificateComponent;
