// StudentForm.tsx
import React, { useState, useEffect } from "react";
import {
  Student,
  Faculty,
  StudentStatus,
  StudentRequest,
  program,
} from "../interface";
import { ApiResponse } from "../api"; // Adjust the import path as needed

interface StudentFormProps {
  student: Student | null;
  faculties: Faculty[];
  statuses: StudentStatus[];
  error: React.ReactNode | null;
  onSubmit: (student: StudentRequest) => Promise<ApiResponse<Student>>;
  onClose: () => void;
  programs: program[];
}

const sampleStudent: StudentRequest = {
  studentId: "22127333",
  fullName: "Nguyễn Văn A",
  dateOfBirth: "2004-01-16T00:00:00Z",
  gender: 0,
  facultyId: 1,
  course: 2022,
  programId: 1,
  address: "123 Nguyễn Văn Cừ, Quận 5, TP.HCM",
  email: "22127333@student.hcmus.edu.vn",
  phoneNumber: "0123456789",
  statusId: 1,
};

const StudentForm: React.FC<StudentFormProps> = ({
  student,
  faculties,
  statuses,
  error,
  onSubmit,
  onClose,
  programs,
}) => {
  const [formData, setFormData] = useState<StudentRequest>({
    studentId: "",
    fullName: "",
    dateOfBirth: new Date().toISOString().split("T")[0],
    gender: 0,
    facultyId: faculties[0]?.facultyId || 1,
    course: new Date().getFullYear(),
    programId: programs[0]?.programId || 1,
    address: "",
    email: "",
    phoneNumber: "",
    statusId: statuses[0]?.statusId || 1,
  });

  const [fieldErrors, setFieldErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (student) {
      setFormData({
        ...student,
        // When editing, use related properties (if available) or defaults
        statusId: student.status?.statusId || statuses[0]?.statusId || 1,
        programId: student.program?.id || programs[0]?.programId || 1,
        facultyId: student.faculty?.id || faculties[0]?.facultyId || 1,
      });
    } else {
      setFormData((prev) => ({
        ...prev,
        facultyId: faculties[0]?.facultyId || 1,
        statusId: statuses[0]?.statusId || 1,
        programId: programs[0]?.programId || 1,
        course: new Date().getFullYear(),
      }));
    }
  }, [student, faculties, statuses, programs]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const date = new Date(formData.dateOfBirth);
      if (isNaN(date.getTime())) {
        throw new Error("Invalid date format");
      }

      const submissionData: StudentRequest = {
        ...formData,
        dateOfBirth: date.toISOString(),
      };

      console.log(
        "Form submitted with data:",
        JSON.stringify(submissionData, null, 2)
      );

      const response = await onSubmit(submissionData);
      if (!response.succeeded) {
        const newFieldErrors: Record<string, string> = {};
        if (
          response.errors &&
          response.errors.includes("Student with this ID already exists.")
        ) {
          newFieldErrors.studentId = "Student ID already exists.";
        }
        setFieldErrors(newFieldErrors);
      } else {
        setFieldErrors({});
      }
    } catch (error) {
      console.error("Form submission error:", error);
      setFieldErrors({});
    }
  };

  return (
    <div className="student-form">
      <h2>{student ? "Sửa Sinh Viên" : "Thêm Sinh Viên Mới"}</h2>
      {error && <div className="error-message">{error}</div>}
      <button
        type="button"
        onClick={() => setFormData(sampleStudent)}
        className="fill-sample-btn"
      >
        Điền mẫu thử
      </button>
      <form onSubmit={handleSubmit}>
        {/* MSSV */}
        <div className="form-group">
          <label>MSSV:</label>
          <input
            type="text"
            value={formData.studentId}
            onChange={(e) =>
              setFormData({ ...formData, studentId: e.target.value })
            }
            required
          />
          {fieldErrors.studentId && (
            <div className="error-message">{fieldErrors.studentId}</div>
          )}
        </div>

        {/* Full name */}
        <div className="form-group">
          <label>Họ và tên:</label>
          <input
            type="text"
            value={formData.fullName}
            onChange={(e) =>
              setFormData({ ...formData, fullName: e.target.value })
            }
            required
          />
          {fieldErrors.fullName && (
            <div className="error-message">{fieldErrors.fullName}</div>
          )}
        </div>

        {/* Date of Birth */}
        <div className="form-group">
          <label>Ngày sinh:</label>
          <input
            type="date"
            value={formData.dateOfBirth.split("T")[0]}
            onChange={(e) =>
              setFormData({ ...formData, dateOfBirth: e.target.value })
            }
            required
          />
          {fieldErrors.dateOfBirth && (
            <div className="error-message">{fieldErrors.dateOfBirth}</div>
          )}
        </div>

        {/* Gender */}
        <div className="form-group">
          <label>Giới tính:</label>
          <select
            value={formData.gender}
            onChange={(e) =>
              setFormData({ ...formData, gender: Number(e.target.value) })
            }
            required
          >
            <option value={0}>Nam</option>
            <option value={1}>Nữ</option>
          </select>
        </div>

        {/* Faculty */}
        <div className="form-group">
          <label>Khoa:</label>
          <select
            value={formData.facultyId}
            onChange={(e) =>
              setFormData({ ...formData, facultyId: Number(e.target.value) })
            }
            required
          >
            {faculties.length > 0 ? (
              faculties.map((faculty) => (
                <option key={faculty.facultyId} value={faculty.facultyId}>
                  {faculty.name}
                </option>
              ))
            ) : (
              <option value="">Loading faculties...</option>
            )}
          </select>
          {fieldErrors.facultyId && (
            <div className="error-message">{fieldErrors.facultyId}</div>
          )}
        </div>

        {/* Course */}
        <div className="form-group">
          <label>Khóa:</label>
          <input
            type="number"
            value={formData.course}
            onChange={(e) =>
              setFormData({ ...formData, course: Number(e.target.value) })
            }
            required
          />
          {fieldErrors.course && (
            <div className="error-message">{fieldErrors.course}</div>
          )}
        </div>

        {/* Program */}
        <div className="form-group">
          <label>Chương trình:</label>
          <select
            value={formData.programId}
            onChange={(e) =>
              setFormData({ ...formData, programId: Number(e.target.value) })
            }
            required
          >
            {programs.length > 0 ? (
              programs.map((prog) => (
                <option key={prog.programId} value={prog.programId}>
                  {prog.name}
                </option>
              ))
            ) : (
              <option value="">Loading programs...</option>
            )}
          </select>
          {fieldErrors.programId && (
            <div className="error-message">{fieldErrors.programId}</div>
          )}
        </div>

        {/* Address */}
        <div className="form-group">
          <label>Địa chỉ:</label>
          <input
            type="text"
            value={formData.address}
            onChange={(e) =>
              setFormData({ ...formData, address: e.target.value })
            }
            required
          />
          {fieldErrors.address && (
            <div className="error-message">{fieldErrors.address}</div>
          )}
        </div>

        {/* Email */}
        <div className="form-group">
          <label>Email:</label>
          <input
            type="email"
            value={formData.email}
            onChange={(e) =>
              setFormData({ ...formData, email: e.target.value })
            }
            required
          />
          {fieldErrors.email && (
            <div className="error-message">{fieldErrors.email}</div>
          )}
        </div>

        {/* Phone Number */}
        <div className="form-group">
          <label>Số điện thoại:</label>
          <input
            type="tel"
            value={formData.phoneNumber}
            onChange={(e) =>
              setFormData({ ...formData, phoneNumber: e.target.value })
            }
            required
          />
          {fieldErrors.phoneNumber && (
            <div className="error-message">{fieldErrors.phoneNumber}</div>
          )}
        </div>

        {/* Status */}
        <div className="form-group">
          <label>Tình trạng:</label>
          <select
            value={formData.statusId}
            onChange={(e) =>
              setFormData({ ...formData, statusId: Number(e.target.value) })
            }
            required
          >
            {statuses.length > 0 ? (
              statuses.map((status) => (
                <option key={status.statusId} value={status.statusId}>
                  {status.name}
                </option>
              ))
            ) : (
              <option value="">Loading statuses...</option>
            )}
          </select>
          {fieldErrors.statusId && (
            <div className="error-message">{fieldErrors.statusId}</div>
          )}
        </div>

        {/* Form Actions */}
        <div className="form-actions">
          <button type="submit">{student ? "Cập Nhật" : "Thêm"}</button>
          <button type="button" onClick={onClose}>
            Hủy
          </button>
        </div>
      </form>
    </div>
  );
};

export default StudentForm;
